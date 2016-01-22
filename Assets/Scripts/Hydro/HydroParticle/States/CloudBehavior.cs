using UnityEngine;

public class CloudBehavior : HydroStateBehavior
{
    private float cloudFadePercent;
    private float CloudFadePercent
    {
        get
        {
            return cloudFadePercent;
        }

        set
        {
            cloudFadePercent = Mathf.Clamp(value, 0, 1);
        }
    }

    public override void InitializeState()
    {
        gameObject.layer = LayerMask.NameToLayer("Cloud");
        Rigidbody.gravityScale = 0;
        Rigidbody.angularDrag = HydroManager.CloudProperties.Drag;
        HeatableObject.HeatPenetration = 0.75F;
        CloudFadePercent = 0;
    }

    public override void RunPhysicsBehavior()
    {
        SetCloudLevelGravityScale();

        if (CloudFadePercent < 0.5F)
            AttractToCloudParticles();
    }

    private void SetCloudLevelGravityScale()
    {
        if (Rigidbody.transform.MapY() < HydroManager.CloudProperties.EquilibriumZoneLowerBound)
        {
            Rigidbody.gravityScale = Rigidbody.velocity.y < HydroManager.CloudProperties.MaximumVelocity ? -HydroManager.CloudProperties.BaseAcceleration : 0;
        }
        else if (Rigidbody.transform.MapY() > HydroManager.CloudProperties.EquilibriumZoneUpperBound)
        {
            Rigidbody.gravityScale = Rigidbody.velocity.y > -HydroManager.CloudProperties.MaximumVelocity ? HydroManager.CloudProperties.BaseAcceleration : 0;
        }
        else
        {
            Rigidbody.gravityScale = 0;
        }
    }

    private void AttractToCloudParticles()
    {
        Vector2 averageVector = Vector2.zero;
        Collider2D[] colliders = ((Vector2)SpriteRenderer.transform.position).GetNeighbors(HydroManager.CloudProperties.NeighborSearchRadius);
        foreach (Collider2D collider in colliders)
        {
            averageVector += (Vector2)(collider.transform.position - transform.position);
        }

        Vector2 direction = averageVector.normalized;
        Rigidbody.AddForce(direction * 0.1F);
    }

    public override void RunGraphicsBehavior()
    {
        bool enoughNeighbors = ((Vector2)SpriteRenderer.transform.position).GetNeighbors(HydroManager.CloudProperties.NeighborSearchRadius).Length >= HydroManager.CloudProperties.MinimumNeighborCount;
        float amountToFade = HydroManager.CloudProperties.FadeRate * Time.fixedDeltaTime;
        CloudFadePercent += enoughNeighbors ? amountToFade : -amountToFade;

        SpriteRenderer.color = Color.Lerp(HydroManager.VaporProperties.Color, HydroManager.CloudProperties.Color, CloudFadePercent);
    }

    public override void RunTemperatureBehavior()
    {
        if (CloudFadePercent > 0.5F)
            HeatableObject.AddHeat(HydroManager.AmbientTemperatureChange * Time.deltaTime);
    }
}