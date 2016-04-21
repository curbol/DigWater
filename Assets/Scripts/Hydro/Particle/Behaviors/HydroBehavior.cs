using System;
using System.Collections;
using UnityEngine;

public abstract class HydroBehavior : MonoBehaviour
{
    public Action OnStartBehavior;
    public Action OnStopBehavior;

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

    private PhysicsProperties physics;
    public PhysicsProperties Physics
    {
        get
        {
            return physics;
        }

        set
        {
            physics = value;

            Rigidbody.angularDrag = physics.AngularDrag;
            Rigidbody.drag = physics.LinearDrag;
            Rigidbody.gravityScale = physics.GravityScale;
            Rigidbody.mass = physics.Mass;
        }
    }

    public abstract void InitializeState();

    public void StartBehavior()
    {
        if (OnStartBehavior != null)
            OnStartBehavior();

        StartCoroutine("RunPhysicsBehavior");
        StartCoroutine("RunGraphicsBehavior");
    }

    public void StopBehavior()
    {
        if (OnStopBehavior != null)
            OnStopBehavior();

        StopAllCoroutines();
    }

    private static void EnforceMaximumVelocities(Rigidbody2D rigidBody, PhysicsProperties physics)
    {
        if (rigidBody == null || physics == null)
            return;

        if (rigidBody.velocity.x > physics.HorizontalMaxVelocity || rigidBody.velocity.y > physics.VerticalMaxVelocity)
        {
            float velocityX = Mathf.Min(rigidBody.velocity.x, physics.HorizontalMaxVelocity);
            float velocityY = Mathf.Min(rigidBody.velocity.y, physics.VerticalMaxVelocity);
            rigidBody.velocity = new Vector2(velocityX, velocityY);
        }
    }

    private IEnumerator RunPhysicsBehavior()
    {
        while (true)
        {
            EnforceMaximumVelocities(Rigidbody, Physics);
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

    protected abstract void UpdatePhysicsBehavior();

    protected abstract void UpdateGraphicsBehavior();
}