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
        Rigidbody.angularDrag = HydroManager.GetProperties<EvaporationProperties>().Physics.AngularDrag;
        Rigidbody.mass = HydroManager.GetProperties<EvaporationProperties>().Physics.Mass;
        HeatableObject.HeatPenetration = HydroManager.GetProperties<EvaporationProperties>().HeatPenetration;
        MoleculeVibration.EnergyLevel = HydroManager.GetProperties<HeatProperties>().MaximumEnergyLevel;
    }

    private void InitializeAsCloud()
    {
        gameObject.layer = LayerMask.NameToLayer("Cloud");
        Rigidbody.angularDrag = HydroManager.GetProperties<CondensationProperties>().Physics.AngularDrag;
        Rigidbody.mass = HydroManager.GetProperties<CondensationProperties>().Physics.Mass;
        HeatableObject.HeatPenetration = HydroManager.GetProperties<CondensationProperties>().HeatPenetration;
        MoleculeVibration.EnergyLevel = Mathf.Lerp(HydroManager.GetProperties<HeatProperties>().MinimumEnergyLevel, HydroManager.GetProperties<HeatProperties>().MaximumEnergyLevel, 0.5F);
    }

    protected override void UpdatePhysicsBehavior()
    {
        SetCloudLevelGravityScale();

        if (CloudFadePercent < clusterFadeThreshold)
            AttractToCloudParticles();
    }

    private void SetCloudLevelGravityScale()
    {
        if (Rigidbody.transform.MapY() < HydroManager.GetProperties<CondensationProperties>().EquilibriumZoneLowerBound)
        {
            Rigidbody.gravityScale = Rigidbody.velocity.y < HydroManager.GetProperties<CondensationProperties>().Physics.HorizontalMaxVelocity ? -HydroManager.GetProperties<CondensationProperties>().Physics.GravityScale : 0;
        }
        else if (Rigidbody.transform.MapY() > HydroManager.GetProperties<CondensationProperties>().EquilibriumZoneUpperBound)
        {
            Rigidbody.gravityScale = Rigidbody.velocity.y > -HydroManager.GetProperties<CondensationProperties>().Physics.HorizontalMaxVelocity ? HydroManager.GetProperties<CondensationProperties>().Physics.GravityScale : 0;
        }
        else
        {
            Rigidbody.gravityScale = 0;
        }
    }

    private void AttractToCloudParticles()
    {
        Vector2 averageVector = Vector2.zero;
        Collider2D[] colliders = ((Vector2)SpriteRenderer.transform.position).GetNeighbors(HydroManager.GetProperties<CondensationProperties>().NeighborSearchRadius);
        foreach (Collider2D collider in colliders)
        {
            averageVector += (Vector2)(collider.transform.position - transform.position);
        }

        Vector2 direction = averageVector.normalized;
        Rigidbody.AddForce(direction * HydroManager.GetProperties<CondensationProperties>().Physics.GravityScale);
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

        bool enoughNeighbors = ((Vector2)SpriteRenderer.transform.position).GetNeighbors(HydroManager.GetProperties<CondensationProperties>().NeighborSearchRadius).Length >= HydroManager.GetProperties<CondensationProperties>().MinimumNeighborCount;
        float amountToFade = HydroManager.GetProperties<CondensationProperties>().FadeRate * Time.fixedDeltaTime;
        CloudFadePercent += enoughNeighbors ? amountToFade : -amountToFade;

        if (SpriteRenderer == null)
            return;

        SpriteRenderer.color = Color.Lerp(HydroManager.GetProperties<EvaporationProperties>().Color, HydroManager.GetProperties<CondensationProperties>().Color, CloudFadePercent);
    }

    protected override void UpdateTemperatureBehavior()
    {
        if (CloudFadePercent >= clusterFadeThreshold)
            HeatableObject.AddHeat(HydroManager.GetProperties<HeatProperties>().CurrentAmbientTemperatureChange * Time.deltaTime);
    }
}