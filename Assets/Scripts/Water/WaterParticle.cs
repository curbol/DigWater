using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class WaterParticle : MonoBehaviour
{
    private float birthTime;

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

    private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer
    {
        get
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            return spriteRenderer;
        }
    }

    public WaterState State { get; private set; }

    public float Age
    {
        get
        {
            return Time.time - birthTime;
        }
    }

    [SerializeField]
    private Color waterColor;
    public Color WaterColor
    {
        get
        {
            return waterColor;
        }
    }

    [SerializeField]
    private Color vaporColor;
    public Color VaporColor
    {
        get
        {
            return vaporColor;
        }
    }

    [Range(0, 10)]
    [SerializeField]
    private float deformability;
    public float Deformability
    {
        get
        {
            return deformability;
        }
    }

    [SerializeField]
    private float maximumAge = 5;
    public float MaximumAge
    {
        get
        {
            return maximumAge;
        }
    }

    [SerializeField]
    private float heatRate;
    public float HeatRate
    {
        get
        {
            return heatRate;
        }
    }

    [SerializeField]
    private float coolRate;
    public float CoolRate
    {
        get
        {
            return coolRate;
        }
    }

    public float Temperature { get; private set; }

    [SerializeField]
    private float maxTemperature;
    public float MaxTemperature
    {
        get
        {
            return maxTemperature;
        }
    }

    [SerializeField]
    private float vaporizationPoint;
    public float VaporizationPoint
    {
        get
        {
            return vaporizationPoint;
        }
    }

    [SerializeField]
    private float vaporAcceleration;
    public float VaporAcceleration
    {
        get
        {
            return vaporAcceleration;
        }
    }

    [SerializeField]
    private float vaporMaximumVelocity;
    public float VaporMaximumVelocity
    {
        get
        {
            return vaporMaximumVelocity;
        }
    }

    [SerializeField]
    private float cloudLevel;
    public float CloudLevel
    {
        get
        {
            return cloudLevel;
        }
    }

    public Action OnDeath { get; set; }

    public void Heat(float? heatIncrease = null)
    {
        Temperature += heatIncrease ?? HeatRate;
        Temperature = Mathf.Clamp(Temperature, 0, MaxTemperature);
    }

    public void Cool(float? heatDecrease = null)
    {
        Heat(-(heatDecrease ?? CoolRate));
    }

    private void Start()
    {
        birthTime = Time.time;
        RigidBody.velocity = Vector2.zero;
        UpdateState();
    }

    private void Update()
    {
        Cool();
        UpdateState();
        SetDeath();
    }

    private void UpdateState()
    {
        float vaporizationProgress = Mathf.Clamp(Temperature / VaporizationPoint, 0, 1);
        SpriteRenderer.color = Color.Lerp(WaterColor, VaporColor, vaporizationProgress);

        bool stateChanged = false;

        if (State == WaterState.Water && vaporizationProgress >= 1)
        {
            State = WaterState.Vapor;
            transform.rotation = Quaternion.identity;
            RigidBody.velocity -= new Vector2(RigidBody.velocity.x, 0);
            stateChanged = true;
        }
        else if (State == WaterState.Vapor && vaporizationProgress < 1)
        {
            State = WaterState.Water;
            stateChanged = true;
        }

        if (stateChanged)
        {
            birthTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        if (State == WaterState.Water)
        {
            SetDirection();
        }
        else if (State == WaterState.Vapor)
        {
            Evaporate();
        }

        SetVelocityScale();
    }

    private void SetDirection()
    {
        if (RigidBody.velocity != Vector2.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, RigidBody.velocity);
        }
    }

    private void SetVelocityScale()
    {
        if (RigidBody.velocity.magnitude < 0.5F)
        {
            transform.localScale = Vector3.one;
            return;
        }

        Vector2 scale = Vector2.one;
        float scaleModifier = Mathf.Min(Mathf.Abs(RigidBody.velocity.y) * (deformability / 100), 0.5F);
        scale.x -= scaleModifier;
        scale.y += scaleModifier;

        transform.localScale = scale;
    }

    private void Evaporate()
    {
        bool belowMaximumLift = transform.TransformDirection(RigidBody.velocity).y < 0 || RigidBody.velocity.y < VaporMaximumVelocity;
        bool belowCloudLevel = transform.position.y < CloudLevel;

        if (belowCloudLevel)
        {
            RigidBody.gravityScale = belowMaximumLift ? -VaporAcceleration : 0;
        }
        else
        {
            RigidBody.gravityScale = 0.1F;
        }
    }

    private void SetDeath()
    {
        if (Age > maximumAge)
        {
            if (OnDeath != null)
            {
                OnDeath();
            }

            Destroy(gameObject);
        }
    }
}