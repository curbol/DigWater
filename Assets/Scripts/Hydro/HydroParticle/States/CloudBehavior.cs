using UnityEngine;

public class CloudBehavior : StateBehaviorBase
{
    public override void InitializeState()
    {
        return;
    }

    public override void RunPhysicsBehavior(Rigidbody2D rigidBody)
    {
        cloudFadePercent = 0;
        rigidBody.gravityScale = 0;
        rigidBody.angularDrag = HydroManager.CloudProperties.Drag;

        // Update velocity
        if (rigidBody.transform.MapY() < HydroManager.CloudProperties.EquilibriumZoneLowerBound)
        {
            rigidBody.gravityScale = rigidBody.velocity.y < HydroManager.CloudProperties.MaximumVelocity ? -HydroManager.CloudProperties.BaseAcceleration : 0;
        }
        else if (rigidBody.transform.MapY() > HydroManager.CloudProperties.EquilibriumZoneUpperBound)
        {
            rigidBody.gravityScale = rigidBody.velocity.y > -HydroManager.CloudProperties.MaximumVelocity ? HydroManager.CloudProperties.BaseAcceleration : 0;
        }
        else
        {
            rigidBody.gravityScale = 0;
        }
    }

    public override void RunGraphicsBehavior(SpriteRenderer spriteRenderer)
    {
        // Update cluster transition progress
        bool enoughNeighbors = GetNeighbors(HydroManager.CloudProperties.NeighborSearchRadius).Length >= HydroManager.CloudProperties.MinimumNeighborCount;
        float amountToFade = HydroManager.CloudProperties.FadeRate * Time.fixedDeltaTime;
        CloudFadePercent += enoughNeighbors ? amountToFade : -amountToFade;

        spriteRenderer.color = Color.Lerp(HydroManager.VaporProperties.Color, HydroManager.CloudProperties.Color, CloudFadePercent);
    }
}