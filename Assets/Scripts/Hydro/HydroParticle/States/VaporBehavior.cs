using UnityEngine;

public class VaporBehavior : StateBehaviorBase
{
    public override void InitializeState()
    {
        return;
    }

    public override void RunPhysicsBehavior(Rigidbody2D rigidbody)
    {
        if (rigidbody == null)
            return;

        rigidbody.velocity = rigidbody.velocity.SetX(0);
        rigidbody.gravityScale = 0;
        rigidbody.angularDrag = 0;

        float percentToCloudLevel = Mathf.Clamp(rigidbody.transform.MapY() / HydroManager.CloudProperties.CloudLevel, 0, 1);
        float minSpeedY = HydroManager.VaporProperties.MaximumVelocity;
        float maxSpeedY = HydroManager.CloudProperties.MaximumVelocity;
        float currentSpeedY = Mathf.Lerp(minSpeedY, maxSpeedY, percentToCloudLevel);
        float directionY = rigidbody.transform.MapY() < HydroManager.CloudProperties.CloudLevel ? 1 : -1;

        rigidbody.velocity = new Vector2(rigidbody.velocity.x, directionY * currentSpeedY);
    }

    public override void RunGraphicsBehavior(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer == null)
            return;

        spriteRenderer.color = HydroManager.VaporProperties.Color;
    }
}