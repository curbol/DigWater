using UnityEngine;

public class WaterBehavior : StateBehaviorBase
{
    public override void InitializeState()
    {
        return;
    }

    public override void RunPhysicsBehavior(Rigidbody2D rigidBody)
    {
        rigidBody.gravityScale = 1;
        rigidBody.angularDrag = 0;
    }

    public override void RunGraphicsBehavior(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.color = Color.Lerp(HydroManager.VaporProperties.Color, HydroManager.LiquidProperties.Color, (1 - PercentToVaporizationPoint) + 0.6F);
    }
}