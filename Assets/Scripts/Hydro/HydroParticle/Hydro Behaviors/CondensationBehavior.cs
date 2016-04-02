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
        Rigidbody.angularDrag = EvaporationManager.Physics.AngularDrag;
        Rigidbody.mass = EvaporationManager.Physics.Mass;
        HeatableObject.HeatPenetration = EvaporationManager.HeatPenetration;
        MoleculeVibration.EnergyLevel = 1;
    }

    private void InitializeAsCloud()
    {
        gameObject.layer = LayerMask.NameToLayer("Cloud");
        Rigidbody.angularDrag = CondensationManager.Physics.AngularDrag;
        Rigidbody.mass = CondensationManager.Physics.Mass;
        HeatableObject.HeatPenetration = CondensationManager.HeatPenetration;
        MoleculeVibration.EnergyLevel = 0.5F;
    }

    protected override void UpdatePhysicsBehavior()
    {
        SetCloudLevelGravityScale();

        if (CloudFadePercent < clusterFadeThreshold)
            AttractToCloudParticles();
    }

    private void SetCloudLevelGravityScale()
    {
        if (Rigidbody.transform.MapY() < CondensationManager.EquilibriumZoneLowerBound)
        {
            Rigidbody.gravityScale = Rigidbody.velocity.y < CondensationManager.Physics.HorizontalMaxVelocity ? -CondensationManager.Physics.GravityScale : 0;
        }
        else if (Rigidbody.transform.MapY() > CondensationManager.EquilibriumZoneUpperBound)
        {
            Rigidbody.gravityScale = Rigidbody.velocity.y > -CondensationManager.Physics.HorizontalMaxVelocity ? CondensationManager.Physics.GravityScale : 0;
        }
        else
        {
            Rigidbody.gravityScale = 0;
        }
    }

    private void AttractToCloudParticles()
    {
        Vector2 averageVector = Vector2.zero;
        Collider2D[] colliders = ((Vector2)SpriteRenderer.transform.position).GetNeighbors(CondensationManager.NeighborSearchRadius);
        foreach (Collider2D collider in colliders)
        {
            averageVector += (Vector2)(collider.transform.position - transform.position);
        }

        Vector2 direction = averageVector.normalized;
        Rigidbody.AddForce(direction * CondensationManager.Physics.GravityScale);
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

        bool enoughNeighbors = ((Vector2)SpriteRenderer.transform.position).GetNeighbors(CondensationManager.NeighborSearchRadius).Length >= CondensationManager.MinimumNeighborCount;
        float amountToFade = CondensationManager.FadeRate * Time.fixedDeltaTime;
        CloudFadePercent += enoughNeighbors ? amountToFade : -amountToFade;

        if (SpriteRenderer == null)
            return;

        SpriteRenderer.color = Color.Lerp(EvaporationManager.Color, CondensationManager.Color, CloudFadePercent);
    }

    protected override void UpdateTemperatureBehavior()
    {
        if (CloudFadePercent >= clusterFadeThreshold)
            HeatableObject.AddHeat(HeatManager.AmbientHeatRate * Time.deltaTime);
    }
}