using System;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class WaterParticle : MonoBehaviour
{
    private float birthTime;

    public Action OnDeath { get; set; }

    public SpriteRenderer SpriteRenderer
    {
        get
        {
            return GetComponentsInChildren<SpriteRenderer>().FirstOrDefault();
        }
    }

    private Rigidbody2D rigidBody;
    public Rigidbody2D RigidBody
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
    public CircleCollider2D CircleCollider
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
            return Time.time - birthTime;
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

    public float WaterToVaporTransitionRatio
    {
        get
        {
            return Mathf.Clamp(Temperature / WaterManager.VaporizationPoint, 0, 1);
        }
    }

    public float VaporToCloudLevelTransitionRatio
    {
        get
        {
            return Mathf.Clamp(Mathf.Abs(WaterManager.CloudLevel - MapY) / WaterManager.CloudLevel, 0, 1);
        }
    }

    private float cloudClusterTransitionRatio;
    private float CloudClusterTransitionRatio
    {
        get
        {
            return cloudClusterTransitionRatio;
        }

        set
        {
            cloudClusterTransitionRatio = Mathf.Clamp(value, 0, 1);
        }
    }

    public bool InCloudZone
    {
        get
        {
            return MapY >= WaterManager.CloudLevel - WaterManager.CloudLevelBuffer && MapY <= WaterManager.CloudLevel + WaterManager.CloudLevelBuffer;
        }
    }

    public bool InCloudEqualibriumZone
    {
        get
        {
            return MapY >= WaterManager.CloudLevel - WaterManager.CloudLevelEquilibriumZoneSize && MapY <= WaterManager.CloudLevel + WaterManager.CloudLevelEquilibriumZoneSize;
        }
    }

    public bool InMapRegion
    {
        get
        {
            return MapY >= 0 && MapY <= MapManager.Map.SizeY && MapX >= 0 && MapX <= MapManager.Map.SizeX;
        }
    }

    public WaterState State { get; private set; }

    public float Temperature { get; private set; }

    public void Heat()
    {
        Heat(null);
    }

    public void Heat(float? heatPercent)
    {
        Temperature += heatPercent != null ? Mathf.Clamp((float)heatPercent, 0, 1) * WaterManager.HeatRate : WaterManager.HeatRate;
        Temperature = Mathf.Clamp(Temperature, 0, WaterManager.MaximumTemperature);
    }

    public void Cool()
    {
        Cool(null);
    }

    public void Cool(float? coolPercent)
    {
        Temperature -= coolPercent != null ? Mathf.Clamp((float)coolPercent, 0, 1) * WaterManager.CoolRate : WaterManager.CoolRate;
        Temperature = Mathf.Clamp(Temperature, 0, WaterManager.MaximumTemperature);
    }

    public Collider2D[] GetNeighbors(float radius, LayerMask layerMask)
    {
        return Physics2D.OverlapCircleAll(transform.position, radius);
    }

    private void Start()
    {
        birthTime = Time.time;
        RigidBody.velocity = Vector2.zero;
        UpdateState();
        SetColor();
    }

    private void Update()
    {
        Cool();
        UpdateState();
        SetColor();
        UpdateDeath();
    }

    private void ResetAsState(WaterState state)
    {
        if (state == WaterState.Water)
        {
            State = WaterState.Water;
            RigidBody.gravityScale = 1;
            RigidBody.angularDrag = 0;
            gameObject.layer = LayerMask.NameToLayer("Metaball");
        }
        else if (state == WaterState.Vapor)
        {
            State = WaterState.Vapor;
            RigidBody.velocity = Vector2.zero;
            RigidBody.gravityScale = 0;
            RigidBody.angularDrag = 0;
            gameObject.layer = LayerMask.NameToLayer("Vapor");
        }
        else if (state == WaterState.Cloud)
        {
            State = WaterState.Cloud;
            cloudClusterTransitionRatio = 0;
            RigidBody.gravityScale = 0;
            RigidBody.angularDrag = WaterManager.CloudDrag;
            gameObject.layer = LayerMask.NameToLayer("Cloud");
        }
    }

    private void UpdateState()
    {
        if (State == WaterState.Water && WaterToVaporTransitionRatio >= 1)
        {
            ResetAsState(WaterState.Vapor);
        }
        else if (WaterToVaporTransitionRatio < 1)
        {
            ResetAsState(WaterState.Water);
        }
        else if (State == WaterState.Vapor && InCloudZone)
        {
            ResetAsState(WaterState.Cloud);
        }
        else if (State == WaterState.Cloud && !InCloudZone)
        {
            ResetAsState(WaterState.Vapor);
        }
    }

    private void SetColor()
    {
        if (State == WaterState.Water && SpriteRenderer != null)
        {
            SpriteRenderer.color = Color.Lerp(WaterManager.VaporColor, WaterManager.WaterColor, (1 - WaterToVaporTransitionRatio) + 0.6F);
        }
        else if (State == WaterState.Vapor && SpriteRenderer != null)
        {
            SpriteRenderer.color = WaterManager.VaporColor;
        }
        else if (State == WaterState.Cloud && SpriteRenderer != null)
        {
            SpriteRenderer.color = Color.Lerp(WaterManager.VaporColor, WaterManager.CloudColor, CloudClusterTransitionRatio + 0.3F);
        }
    }

    private void FixedUpdate()
    {
        if (State == WaterState.Vapor)
        {
            RunVaporBehavior();
        }
        else if (State == WaterState.Cloud)
        {
            RunCloudBehavior();
        }

        UpdateRotation();
        UpdateVelocityScale();
    }

    private void RunVaporBehavior()
    {
        Vector2 direction = MapY < WaterManager.CloudLevel ? Vector2.up : Vector2.down;
        RigidBody.velocity = direction * Mathf.Lerp(0, WaterManager.VaporMaximumVelocity, VaporToCloudLevelTransitionRatio) + new Vector2(RigidBody.velocity.x, 0);
    }

    private void RunCloudBehavior()
    {
        // Update cluster transition progress
        if (CloudClusterTransitionRatio < 1 && GetNeighbors(WaterManager.CloudClusterRadius, LayerMask.NameToLayer("Cloud")).Length > WaterManager.CloudClusterMinimumCount)
        {
            CloudClusterTransitionRatio += WaterManager.CloudClusterTransitionRate * Time.fixedDeltaTime;
        }
        else if (CloudClusterTransitionRatio > 0)
        {
            CloudClusterTransitionRatio -= WaterManager.CloudClusterTransitionRate * Time.fixedDeltaTime;
        }

        // Update velocity
        if (MapY < WaterManager.CloudLevel - WaterManager.CloudLevelEquilibriumZoneSize)
        {
            RigidBody.gravityScale = RigidBody.velocity.y < WaterManager.VaporMaximumVelocity ? -WaterManager.VaporAcceleration : 0;
        }
        else if (MapY > WaterManager.CloudLevel + WaterManager.CloudLevelEquilibriumZoneSize)
        {
            RigidBody.gravityScale = RigidBody.velocity.y > -WaterManager.VaporMaximumVelocity ? WaterManager.VaporAcceleration : 0;
        }
        else
        {
            RigidBody.gravityScale = 0;
        }
    }

    private void UpdateRotation()
    {
        if (SpriteRenderer != null && RigidBody.velocity != Vector2.zero)
        {
            SpriteRenderer.transform.rotation = Quaternion.LookRotation(Vector3.forward, RigidBody.velocity);
        }
    }

    private void UpdateVelocityScale()
    {
        if (SpriteRenderer != null && RigidBody.velocity.magnitude < 0.5F)
        {
            SpriteRenderer.transform.localScale = Vector3.one;
            return;
        }

        Vector2 scale = Vector2.one;
        float scaleModifier = Mathf.Min(Mathf.Abs(RigidBody.velocity.y) * (WaterManager.Deformability / 100), 0.5F);
        scale.x -= scaleModifier;
        scale.y += scaleModifier;

        if (SpriteRenderer != null)
            SpriteRenderer.transform.localScale = scale;
    }

    private void UpdateDeath()
    {
        if (!InMapRegion)
        {
            if (OnDeath != null)
                OnDeath();

            Destroy(gameObject);
        }
    }
}