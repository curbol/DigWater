using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
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

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer
    {
        get
        {
            if (spriteRenderer.transform.parent == null)
            {
                spriteRenderer.transform.parent = transform;
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

            RigidBody.velocity = Vector2.zero;
            RigidBody.gravityScale = -vaporAcceleration;

            stateChanged = true;
        }
        else if (State == WaterState.Vapor && vaporizationProgress < 1)
        {
            State = WaterState.Water;

            RigidBody.gravityScale = 1;

            stateChanged = true;
        }

        if (stateChanged)
        {
            birthTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        if (State == WaterState.Vapor)
        {
            Evaporate();
        }

        SetDirection();
        SetVelocityScale();
    }

    private void SetDirection()
    {
        if (RigidBody.velocity != Vector2.zero)
        {
            SpriteRenderer.transform.rotation = Quaternion.LookRotation(Vector3.forward, RigidBody.velocity);
        }
    }

    private void SetVelocityScale()
    {
        if (RigidBody.velocity.magnitude < 0.5F)
        {
            SpriteRenderer.transform.localScale = Vector3.one;
            return;
        }

        Vector2 scale = Vector2.one;
        float scaleModifier = Mathf.Min(Mathf.Abs(RigidBody.velocity.y) * (deformability / 100), 0.5F);
        scale.x -= scaleModifier;
        scale.y += scaleModifier;

        SpriteRenderer.transform.localScale = scale;
    }

    private void Evaporate()
    {
        bool belowMaximumVelocity = RigidBody.velocity.y < VaporMaximumVelocity;
        bool belowCloudLevel = transform.position.y < CloudLevel;

        if (transform.position.y < CloudLevel / 2)
        {
            RigidBody.velocity -= new Vector2(RigidBody.velocity.x, 0);
        }

        if (belowCloudLevel)
        {
            RigidBody.gravityScale = belowMaximumVelocity ? -VaporAcceleration : 0;
        }
        else
        {
            RigidBody.gravityScale = VaporAcceleration;
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