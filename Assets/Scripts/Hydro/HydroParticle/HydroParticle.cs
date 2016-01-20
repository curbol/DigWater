using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Vibration))]
public class HydroParticle : MonoBehaviour
{
    private static readonly System.Random random = new System.Random();

    public Action OnDeath { get; set; }

    public float BirthTime { get; private set; }

    public HydroState State { get; private set; }

    [SerializeField]
    private Rigidbody2D rigidBody;
    private Rigidbody2D Rigidbody
    {
        get
        {
            if (rigidBody == null)
            {
                rigidBody = gameObject.GetSafeComponent<Rigidbody2D>();
            }

            return rigidBody;
        }
    }

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer SpriteRenderer
    {
        get
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.GetSafeComponent<SpriteRenderer>();
            }

            return spriteRenderer;
        }
    }

    [SerializeField]
    private CircleCollider2D circleCollider;
    private CircleCollider2D CircleCollider
    {
        get
        {
            if (circleCollider == null)
            {
                circleCollider = GetComponent<CircleCollider2D>();
            }

            return circleCollider;
        }
    }

    [SerializeField]
    private Vibration vibration;
    private Vibration Vibration
    {
        get
        {
            if (vibration == null)
            {
                vibration = GetComponent<Vibration>();
            }

            return vibration;
        }
    }

    [SerializeField]
    private Heatable heatableObject;
    private Heatable HeatableObject
    {
        get
        {
            if (heatableObject == null)
            {
                heatableObject = GetComponent<Heatable>();
            }

            return heatableObject;
        }
    }

    public float Age
    {
        get
        {
            return Time.time - BirthTime;
        }
    }

    private float energyLevelAdjustment;
    private float EnergyLevelAdjustment
    {
        get
        {
            if (energyLevelAdjustment == default(float))
                energyLevelAdjustment = Mathf.Lerp(-HydroManager.EnergyLevelDeviation, HydroManager.EnergyLevelDeviation, (float)random.NextDouble());

            return energyLevelAdjustment;
        }
    }

    public float EnergyLevel
    {
        get
        {
            float energyLevel = Mathf.Lerp(HydroManager.MinimumEnergyLevel, HydroManager.MaximumEnergyLevel, PercentToVaporizationPoint);

            return energyLevel + EnergyLevelAdjustment;
        }
    }

    public float PercentToVaporizationPoint
    {
        get
        {
            return Mathf.Clamp(HeatableObject.Temperature / HydroManager.VaporizationPoint, 0, 1);
        }
    }

    public float PercentToCloudLevel
    {
        get
        {
            return Mathf.Clamp(transform.MapY() / HydroManager.CloudProperties.CloudLevel, 0, 1);
        }
    }

    private float cloudFadePercent;
    private float CloudFadePercent
    {
        get
        {
            return cloudFadePercent;
        }

        set
        {
            cloudFadePercent = Mathf.Clamp(value, 0, 1);
        }
    }

    public bool InCloudZone
    {
        get
        {
            return transform.MapY() >= HydroManager.CloudProperties.CloudLevelLowerBound && transform.MapY() <= HydroManager.CloudProperties.CloudLevelUpperBound;
        }
    }

    public bool InCloudEqualibriumZone
    {
        get
        {
            return transform.MapY() >= HydroManager.CloudProperties.EquilibriumZoneLowerBound && transform.MapY() <= HydroManager.CloudProperties.EquilibriumZoneUpperBound;
        }
    }

    public Collider2D[] GetNeighbors(float radius)
    {
        return Physics2D.OverlapCircleAll(transform.position, radius);
    }

    public Collider2D[] GetNeighbors(float radius, LayerMask layerMask)
    {
        return Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
    }

    private void Start()
    {
        BirthTime = Time.time;
        Rigidbody.velocity = Vector2.zero;
        HeatableObject.MaximumTemperature = HydroManager.MaximumTemperature;
        HeatableObject.HeatPenetration = 0.85F;

        StartCoroutine(UpdateState());
        StartCoroutine(UpdateTemperature());
        StartCoroutine(UpdateColor());
    }

    private void FixedUpdate()
    {
        if (State == HydroState.Vapor)
        {
            RunVaporBehavior();
        }
        else if (State == HydroState.Cloud)
        {
            RunCloudBehavior();
        }
    }

    private void RunVaporBehavior()
    {
        Vector2 direction = transform.MapY() < HydroManager.CloudProperties.CloudLevel ? Vector2.up : Vector2.down;
        Rigidbody.velocity = direction * Mathf.Lerp(HydroManager.VaporProperties.MaximumVelocity, HydroManager.CloudProperties.MaximumVelocity, PercentToCloudLevel) + new Vector2(Rigidbody.velocity.x, 0);
    }

    private void RunCloudBehavior()
    {
        // Update cluster transition progress
        bool enoughNeighbors = GetNeighbors(HydroManager.CloudProperties.NeighborSearchRadius).Length >= HydroManager.CloudProperties.MinimumNeighborCount;
        float amountToFade = HydroManager.CloudProperties.FadeRate * Time.fixedDeltaTime;
        CloudFadePercent += enoughNeighbors ? amountToFade : -amountToFade;

        // Update velocity
        if (transform.MapY() < HydroManager.CloudProperties.EquilibriumZoneLowerBound)
        {
            Rigidbody.gravityScale = Rigidbody.velocity.y < HydroManager.CloudProperties.MaximumVelocity ? -HydroManager.CloudProperties.BaseAcceleration : 0;
        }
        else if (transform.MapY() > HydroManager.CloudProperties.EquilibriumZoneUpperBound)
        {
            Rigidbody.gravityScale = Rigidbody.velocity.y > -HydroManager.CloudProperties.MaximumVelocity ? HydroManager.CloudProperties.BaseAcceleration : 0;
        }
        else
        {
            Rigidbody.gravityScale = 0;
        }
    }

    private void ResetAsState(HydroState state)
    {
        if (state == HydroState.Water)
        {
            State = HydroState.Water;
            Rigidbody.gravityScale = 1;
            Rigidbody.angularDrag = 0;
            gameObject.layer = LayerMask.NameToLayer("Metaball");
        }
        else if (state == HydroState.Vapor)
        {
            State = HydroState.Vapor;
            Rigidbody.velocity = new Vector2(0, Rigidbody.velocity.y);
            Rigidbody.gravityScale = 0;
            Rigidbody.angularDrag = 0;
            gameObject.layer = LayerMask.NameToLayer("Vapor");
        }
        else if (state == HydroState.Cloud)
        {
            State = HydroState.Cloud;
            cloudFadePercent = 0;
            Rigidbody.gravityScale = 0;
            Rigidbody.angularDrag = HydroManager.CloudProperties.Drag;
            gameObject.layer = LayerMask.NameToLayer("Cloud");
        }
    }

    private IEnumerator UpdateTemperature()
    {
        while (true)
        {
            yield return null;

            if (State == HydroState.Cloud && CloudFadePercent < 0.5F)
                continue;

            HeatableObject.AddHeat(HydroManager.AmbientTemperatureChange);

            Vibration.EnergyLevel = EnergyLevel;
        }
    }

    private IEnumerator UpdateState()
    {
        while (true)
        {
            if (State != HydroState.Water && PercentToVaporizationPoint < 1)
            {
                ResetAsState(HydroState.Water);
            }
            else if (State != HydroState.Vapor && !InCloudZone && PercentToVaporizationPoint >= 1)
            {
                ResetAsState(HydroState.Vapor);
            }
            else if (State != HydroState.Cloud && InCloudZone && PercentToVaporizationPoint >= 1)
            {
                ResetAsState(HydroState.Cloud);
            }

            yield return null;
        }
    }

    private IEnumerator UpdateColor()
    {
        while (true)
        {
            if (State == HydroState.Water && SpriteRenderer != null)
            {
                SpriteRenderer.color = Color.Lerp(HydroManager.VaporProperties.Color, HydroManager.LiquidProperties.Color, (1 - PercentToVaporizationPoint) + 0.6F);
            }
            else if (State == HydroState.Vapor && SpriteRenderer != null)
            {
                SpriteRenderer.color = HydroManager.VaporProperties.Color;
            }
            else if (State == HydroState.Cloud && SpriteRenderer != null)
            {
                SpriteRenderer.color = Color.Lerp(HydroManager.VaporProperties.Color, HydroManager.CloudProperties.Color, CloudFadePercent);
            }

            yield return new WaitForEndOfFrame();
        }
    }
}