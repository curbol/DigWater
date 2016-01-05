using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class HydroParticle : MonoBehaviour, IHeatable, IPushable
{
    public Action OnDeath { get; set; }

    public float BirthTime { get; private set; }

    public HydroState State { get; private set; }

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer SpriteRenderer
    {
        get
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponentsInChildren<SpriteRenderer>().FirstOrDefault();
            }

            return spriteRenderer;
        }
    }

    private Rigidbody2D rigidBody;
    private Rigidbody2D RigidBody
    {
        get
        {
            if (rigidBody == null)
            {
                rigidBody = GetComponent<Rigidbody2D>();
            }

            return rigidBody;
        }
    }

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

    public float Age
    {
        get
        {
            return Time.time - BirthTime;
        }
    }

    public float MapX
    {
        get
        {
            return transform.position.x + MapManager.Map.SizeX / 2F;
        }
    }

    public float MapY
    {
        get
        {
            return transform.position.y + MapManager.Map.SizeY / 2F;
        }
    }

    private float temperature;
    public float Temperature
    {
        get
        {
            return temperature;
        }

        set
        {
            temperature = Mathf.Clamp(value, 0, HydroManager.MaximumTemperature);
        }
    }

    public float PercentToVaporizationPoint
    {
        get
        {
            return Mathf.Clamp(Temperature / HydroManager.VaporizationPoint, 0, 1);
        }
    }

    public float PercentToCloudLevel
    {
        get
        {
            return Mathf.Clamp(MapY / HydroManager.CloudProperties.CloudLevel, 0, 1);
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
            return MapY >= HydroManager.CloudProperties.CloudLevelLowerBound && MapY <= HydroManager.CloudProperties.CloudLevelUpperBound;
        }
    }

    public bool InCloudEqualibriumZone
    {
        get
        {
            return MapY >= HydroManager.CloudProperties.EquilibriumZoneLowerBound && MapY <= HydroManager.CloudProperties.EquilibriumZoneUpperBound;
        }
    }

    public bool InMapRegion
    {
        get
        {
            return MapY >= 0 && MapY <= MapManager.Map.SizeY && MapX >= 0 && MapX <= MapManager.Map.SizeX;
        }
    }

    public void AddHeat(float value)
    {
        Temperature += value;
    }

    public void AddForce(Vector2 force)
    {
        RigidBody.AddForce(force);
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
        RigidBody.velocity = Vector2.zero;

        StartCoroutine(UpdateState());
        StartCoroutine(UpdateTemperature());
        StartCoroutine(UpdateColor());
        StartCoroutine(UpdateRotation());
        StartCoroutine(UpdateVelocityScale());
        StartCoroutine(UpdateDeath());
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
        Vector2 direction = MapY < HydroManager.CloudProperties.CloudLevel ? Vector2.up : Vector2.down;
        RigidBody.velocity = direction * Mathf.Lerp(HydroManager.VaporProperties.MaximumVelocity, HydroManager.CloudProperties.MaximumVelocity, PercentToCloudLevel) + new Vector2(RigidBody.velocity.x, 0);
    }

    private void RunCloudBehavior()
    {
        // Update cluster transition progress
        bool enoughNeighbors = GetNeighbors(HydroManager.CloudProperties.NeighborSearchRadius).Length >= HydroManager.CloudProperties.MinimumNeighborCount;
        float amountToFade = HydroManager.CloudProperties.FadeRate * Time.fixedDeltaTime;
        CloudFadePercent += enoughNeighbors ? amountToFade : -amountToFade;

        // Update velocity
        if (MapY < HydroManager.CloudProperties.EquilibriumZoneLowerBound)
        {
            RigidBody.gravityScale = RigidBody.velocity.y < HydroManager.CloudProperties.MaximumVelocity ? -HydroManager.CloudProperties.BaseAcceleration : 0;
        }
        else if (MapY > HydroManager.CloudProperties.EquilibriumZoneUpperBound)
        {
            RigidBody.gravityScale = RigidBody.velocity.y > -HydroManager.CloudProperties.MaximumVelocity ? HydroManager.CloudProperties.BaseAcceleration : 0;
        }
        else
        {
            RigidBody.gravityScale = 0;
        }
    }

    private void ResetAsState(HydroState state)
    {
        if (state == HydroState.Water)
        {
            State = HydroState.Water;
            RigidBody.gravityScale = 1;
            RigidBody.angularDrag = 0;
            gameObject.layer = LayerMask.NameToLayer("Metaball");
        }
        else if (state == HydroState.Vapor)
        {
            State = HydroState.Vapor;
            RigidBody.velocity = Vector2.zero;
            RigidBody.gravityScale = 0;
            RigidBody.angularDrag = 0;
            gameObject.layer = LayerMask.NameToLayer("Vapor");
        }
        else if (state == HydroState.Cloud)
        {
            State = HydroState.Cloud;
            cloudFadePercent = 0;
            RigidBody.gravityScale = 0;
            RigidBody.angularDrag = HydroManager.CloudProperties.Drag;
            gameObject.layer = LayerMask.NameToLayer("Cloud");
        }
    }

    private IEnumerator UpdateTemperature()
    {
        while (true)
        {
            AddHeat(HydroManager.AmbientTemperatureChange);

            yield return null;
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

    private IEnumerator UpdateRotation()
    {
        while (true)
        {
            if (SpriteRenderer != null && RigidBody.velocity != Vector2.zero)
            {
                SpriteRenderer.transform.rotation = Quaternion.LookRotation(Vector3.forward, RigidBody.velocity);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator UpdateVelocityScale()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (SpriteRenderer != null && RigidBody.velocity.magnitude < 0.5F)
            {
                SpriteRenderer.transform.localScale = Vector3.one;
                continue;
            }

            Vector2 scale = Vector2.one;
            float scaleModifier = Mathf.Min(Mathf.Abs(RigidBody.velocity.y) * (HydroManager.Deformability / 100), 0.5F);
            scale.x -= scaleModifier;
            scale.y += scaleModifier;

            if (SpriteRenderer != null)
                SpriteRenderer.transform.localScale = scale;
        }
    }

    private IEnumerator UpdateDeath()
    {
        while (true)
        {
            if (!InMapRegion)
            {
                if (OnDeath != null)
                    OnDeath();

                Destroy(gameObject);
            }

            yield return new WaitForSeconds(2);
        }
    }
}