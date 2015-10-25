using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class WaterParticle : MonoBehaviour
{
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

    private void Start()
    {
        birthTime = Time.time;
    }

	private void Update()
	{
        SetVelocityScale();
        //SetDeathScale();
        SetDeath();
    }

    private void SetVelocityScale()
    {
        Vector2 scale = Vector2.one;
        scale.x += Mathf.Abs(RigidBody.velocity.x) * (deformability / 100);
        scale.y += Mathf.Abs(RigidBody.velocity.y) * (deformability / 100);
        transform.localScale = scale;
    }

    private void SetDeathScale()
    {
        float scaleFactor = 1.0f - (Age / maximumAge);
        Vector2 scale = transform.localScale;
        scale.x *= scaleFactor;
        scale.y *= scaleFactor;
        transform.localScale = scale;
    }

    private void SetDeath()
    {
        if (Age > maximumAge)
        {
            Destroy(gameObject);
        }
    }
}