using UnityEngine;

public abstract class HydroStateBehavior : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigidBody;
    public Rigidbody2D Rigidbody
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
    public SpriteRenderer SpriteRenderer
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
    private Heatable heatableObject;
    public Heatable HeatableObject
    {
        get
        {
            if (heatableObject == null)
                heatableObject = gameObject.GetSafeComponent<Heatable>();

            return heatableObject;
        }
    }

    public abstract void InitializeState();

    public abstract void RunPhysicsBehavior();

    public abstract void RunGraphicsBehavior();

    public abstract void RunTemperatureBehavior();
}