using UnityEngine;

public class CloudBehavior : HydroStateBehavior
{
    private const float clusterFadeThreshold = 0.75F;

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
        Rigidbody.gravityScale = 0;
        CloudFadePercent = 0;

        InitializeAsVapor();
    }

    private void InitializeAsVapor()
    {
        gameObject.layer = LayerMask.NameToLayer("Vapor");
        Rigidbody.angularDrag = 0;
        Rigidbody.mass = 0.1F;
        HeatableObject.HeatPenetration = HydroManager.VaporProperties.HeatPenetration;
        MoleculeVibration.EnergyLevel = HydroManager.HeatProperties.MaximumEnergyLevel;
    }

    private void InitializeAsCloud()
    {
        gameObject.layer = LayerMask.NameToLayer("Cloud");
        Rigidbody.angularDrag = HydroManager.CloudProperties.Drag;
        Rigidbody.mass = 0.3F;
        HeatableObject.HeatPenetration = HydroManager.CloudProperties.HeatPenetration;
        MoleculeVibration.EnergyLevel = Mathf.Lerp(HydroManager.HeatProperties.MinimumEnergyLevel, HydroManager.HeatProperties.MaximumEnergyLevel, 0.5F);
    }

    public override void RunPhysicsBehavior()
    {
        SetCloudLevelGravityScale();

        if (CloudFadePercent < clusterFadeThreshold)
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
        Rigidbody.AddForce(direction * HydroManager.CloudProperties.BaseAcceleration);
    }

    public override void RunGraphicsBehavior()
    {
        if (gameObject.layer != LayerMask.NameToLayer("Cloud") && CloudFadePercent > 0.5F)
        {
            InitializeAsCloud();
        }
        else if (gameObject.layer != LayerMask.NameToLayer("Vapor") && CloudFadePercent <= 0.5F)
        {
            InitializeAsVapor();
        }

        bool enoughNeighbors = ((Vector2)SpriteRenderer.transform.position).GetNeighbors(HydroManager.CloudProperties.NeighborSearchRadius).Length >= HydroManager.CloudProperties.MinimumNeighborCount;
        float amountToFade = HydroManager.CloudProperties.FadeRate * Time.fixedDeltaTime;
        CloudFadePercent += enoughNeighbors ? amountToFade : -amountToFade;

        if (SpriteRenderer == null)
            return;

        SpriteRenderer.color = Color.Lerp(HydroManager.VaporProperties.Color, HydroManager.CloudProperties.Color, CloudFadePercent);
    }

    public override void RunTemperatureBehavior()
    {
        if (CloudFadePercent >= clusterFadeThreshold)
            HeatableObject.AddHeat(HydroManager.HeatProperties.CurrentAmbientTemperatureChange * Time.deltaTime);
    }
}