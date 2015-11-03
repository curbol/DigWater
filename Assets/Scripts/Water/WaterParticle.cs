using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class WaterParticle : MonoBehaviour
{
    public Action OnDeath { get; set; }
    private float birthTime;

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

    [Range(0,10)]
    public float deformability = 3;
    public float maximumAge = 5;

    public float Age
    {
        get
        {
            return Time.time - birthTime;
        }
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
        SetDeath();
    }

    private void FixedUpdate()
    {
        SetDirection();
        SetVelocityScale();
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

    private void SetDirection()
    {
        if (RigidBody.velocity != Vector2.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, RigidBody.velocity);
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