using UnityEngine;

public abstract class StateBehaviorBase
{
    public abstract void InitializeState();

    public abstract void RunPhysicsBehavior(Rigidbody2D rigidBody);

    public abstract void RunGraphicsBehavior(SpriteRenderer spriteRenderer);
}