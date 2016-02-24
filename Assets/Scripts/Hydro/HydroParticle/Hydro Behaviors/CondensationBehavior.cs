using UnityEngine;

public class CondensationBehavior : HydroBehavior
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
        Rigidbody.angularDrag = HydroManager.Vapor.Physics.AngularDrag;
        Rigidbody.mass = HydroManager.Vapor.Physics.Mass;
        HeatableObject.HeatPenetration = HydroManager.Vapor.HeatPenetration;
        MoleculeVibration.EnergyLevel = HydroManager.Heat.MaximumEnergyLevel;
    }

    private void InitializeAsCloud()
    {
        gameObject.layer = LayerMask.NameToLayer("Cloud");
        Rigidbody.angularDrag = HydroManager.Cloud.Physics.AngularDrag;
        Rigidbody.mass = HydroManager.Cloud.Physics.Mass;
        HeatableObject.HeatPenetration = HydroManager.Cloud.HeatPenetration;
        MoleculeVibration.EnergyLevel = Mathf.Lerp(HydroManager.Heat.MinimumEnergyLevel, HydroManager.Heat.MaximumEnergyLevel, 0.5F);
    }

    protected override void UpdatePhysicsBehavior()
    {
        SetCloudLevelGravityScale();

        if (CloudFadePercent < clusterFadeThreshold)
            AttractToCloudParticles();
    }

    private void SetCloudLevelGravityScale()
    {
        if (Rigidbody.transform.MapY() < HydroManager.Cloud.EquilibriumZoneLowerBound)
        {
            Rigidbody.gravityScale = Rigidbody.velocity.y < HydroManager.Cloud.Physics.MaximumVelocity ? -HydroManager.Cloud.Physics.GravityScale : 0;
        }
        else if (Rigidbody.transform.MapY() > HydroManager.Cloud.EquilibriumZoneUpperBound)
        {
            Rigidbody.gravityScale = Rigidbody.velocity.y > -HydroManager.Cloud.Physics.MaximumVelocity ? HydroManager.Cloud.Physics.GravityScale : 0;
        }
        else
        {
            Rigidbody.gravityScale = 0;
        }
    }

    private void AttractToCloudParticles()
    {
        Vector2 averageVector = Vector2.zero;
        Collider2D[] colliders = ((Vector2)SpriteRenderer.transform.position).GetNeighbors(HydroManager.Cloud.NeighborSearchRadius);
        foreach (Collider2D collider in colliders)
        {
            averageVector += (Vector2)(collider.transform.position - transform.position);
        }

        Vector2 direction = averageVector.normalized;
        Rigidbody.AddForce(direction * HydroManager.Cloud.Physics.GravityScale);
    }

    protected override void UpdateGraphicsBehavior()
    {
        if (gameObject.layer != LayerMask.NameToLayer("Cloud") && CloudFadePercent > 0.5F)
        {
            InitializeAsCloud();
        }
        else if (gameObject.layer != LayerMask.NameToLayer("Vapor") && CloudFadePercent <= 0.5F)
        {
            InitializeAsVapor();
        }

        bool enoughNeighbors = ((Vector2)SpriteRenderer.transform.position).GetNeighbors(HydroManager.Cloud.NeighborSearchRadius).Length >= HydroManager.Cloud.MinimumNeighborCount;
        float amountToFade = HydroManager.Cloud.FadeRate * Time.fixedDeltaTime;
        CloudFadePercent += enoughNeighbors ? amountToFade : -amountToFade;

        if (SpriteRenderer == null)
            return;

        SpriteRenderer.color = Color.Lerp(HydroManager.Vapor.Color, HydroManager.Cloud.Color, CloudFadePercent);
    }

    protected override void UpdateTemperatureBehavior()
    {
        if (CloudFadePercent >= clusterFadeThreshold)
            HeatableObject.AddHeat(HydroManager.Heat.CurrentAmbientTemperatureChange * Time.deltaTime);
    }
}