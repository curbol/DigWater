using System.Collections;
using UnityEngine;

public abstract class HydroBehavior : MonoBehaviour
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

    public void StartBehavior()
    {
        StartCoroutine("RunPhysicsBehavior");
        StartCoroutine("RunGraphicsBehavior");
        StartCoroutine("RunTemperatureBehavior");
    }

    public void StopBehavior()
    {
        StopCoroutine("RunPhysicsBehavior");
        StopCoroutine("RunGraphicsBehavior");
        StopCoroutine("RunTemperatureBehavior");
    }

    private IEnumerator RunPhysicsBehavior()
    {
        while (true)
        {
            UpdatePhysicsBehavior();

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator RunGraphicsBehavior()
    {
        while (true)
        {
            UpdateGraphicsBehavior();

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator RunTemperatureBehavior()
    {
        while (true)
        {
            UpdateTemperatureBehavior();

            yield return new WaitForFixedUpdate();
        }
    }

    protected abstract void UpdatePhysicsBehavior();

    protected abstract void UpdateGraphicsBehavior();

    protected abstract void UpdateTemperatureBehavior();
}