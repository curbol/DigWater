using UnityEngine;

public class VaporBehavior : HydroStateBehavior
{
    public override void InitializeState()
    {
        gameObject.layer = LayerMask.NameToLayer("Vapor");
        Rigidbody.velocity = Rigidbody.velocity.SetX(0);
        Rigidbody.gravityScale = 0;
        Rigidbody.angularDrag = 0;
        HeatableObject.HeatPenetration = 0.95F;
    }

    public override void RunPhysicsBehavior()
    {
        if (Rigidbody == null)
            return;

        float percentToCloudLevel = Mathf.Clamp(Rigidbody.transform.MapY() / HydroManager.CloudProperties.CloudLevel, 0, 1);
        float minSpeedY = HydroManager.VaporProperties.MaximumVelocity;
        float maxSpeedY = HydroManager.CloudProperties.MaximumVelocity;
        float currentSpeedY = Mathf.Lerp(minSpeedY, maxSpeedY, percentToCloudLevel);
        float directionY = Rigidbody.transform.MapY() < HydroManager.CloudProperties.CloudLevel ? 1 : -1;

        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, directionY * currentSpeedY);
    }

    public override void RunGraphicsBehavior()
    {
        if (SpriteRenderer == null)
            return;

        SpriteRenderer.color = HydroManager.VaporProperties.Color;
    }

    public override void RunTemperatureBehavior()
    {
        return;
    }
}