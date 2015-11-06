using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class WaterParticle : MonoBehaviour
{
    private WaterState state;
    private float birthTime;
    private Rigidbody2D rigidBody;

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

        set
        {
            maximumAge = value;
        }
    }

    [SerializeField]
    private float temperature;
    public float Temperature
    {
        get
        {
            return temperature;
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

    public float Age
    {
        get
        {
            return Time.time - birthTime;
        }
    }

    public Action OnDeath { get; set; }

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

    public void Heat(float? heatIncrease = null)
    {
        temperature += heatIncrease ?? HeatRate;
        temperature = Mathf.Max(0, temperature);

        Debug.DrawLine(transform.position, transform.position + Vector3.up * 0.5F, Color.Lerp(new Color(255, 255, 0, 0), new Color(255, 0, 0), temperature / VaporizationPoint));

        if (state == WaterState.Water && temperature >= VaporizationPoint)
        {
            state = WaterState.Vapor;
        }
        else if (state == WaterState.Vapor && temperature < VaporizationPoint)
        {
            state = WaterState.Water;
        }
    }

    public void Cool(float? heatDecrease = null)
    {
        Heat(-(heatDecrease ?? CoolRate));
    }

    public void Initialize()
    {
        birthTime = Time.time;
        RigidBody.velocity = Vector2.zero;
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        Cool();
        SetDeath();
    }

    private void FixedUpdate()
    {
        if (state == WaterState.Water)
        {
            SetDirection();
        }
        else if (state == WaterState.Vapor)
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
        transform.rotation = Quaternion.identity;
        RigidBody.velocity -= new Vector2(RigidBody.velocity.x, 0);
        RigidBody.gravityScale = RigidBody.velocity.y < VaporMaximumVelocity ? -VaporAcceleration : 0;
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