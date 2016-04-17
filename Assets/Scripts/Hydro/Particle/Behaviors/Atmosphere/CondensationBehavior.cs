using System.Collections;
using UnityEngine;

public class CondensationBehavior : HydroBehavior
{
    private const float clusterFadeThreshold = 0.75F;

    [SerializeField]
    private Heatable heatableObject;
    public Heatable HeatableObject
    {
        get
        {
            if (heatableObject == null)
                heatableObject = gameObject.GetSafeComponent<Heatable>();

            return heatableObject;
        }
    }

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
        SpriteRenderer.color = EvaporationManager.Color;
        Physics = CondensationManager.Physics;
        CloudFadePercent = 0;

        OnStartBehavior += () =>
        {
            StartCoroutine("RunTemperatureBehavior");
        };

        OnStopBehavior += () =>
        {
            StopCoroutine("RunTemperatureBehavior");
        };
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
            Rigidbody.gravityScale = -CondensationManager.Physics.GravityScale;
        }
        else if (Rigidbody.transform.MapY() > CondensationManager.EquilibriumZoneUpperBound)
        {
            Rigidbody.gravityScale = CondensationManager.Physics.GravityScale;
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
        bool enoughNeighbors = ((Vector2)SpriteRenderer.transform.position).GetNeighbors(CondensationManager.NeighborSearchRadius).Length >= CondensationManager.MinimumNeighborCount;
        float amountToFade = CondensationManager.FadeRate * Time.fixedDeltaTime;
        CloudFadePercent += enoughNeighbors ? amountToFade : -amountToFade;

        if (SpriteRenderer == null)
            return;

        SpriteRenderer.color = Color.Lerp(EvaporationManager.Color, CondensationManager.Color, CloudFadePercent);
    }

    private IEnumerator RunTemperatureBehavior()
    {
        while (true)
        {
            if (CloudFadePercent >= clusterFadeThreshold)
                HeatableObject.AddHeat(HeatManager.AmbientHeatRate * Time.deltaTime);

            yield return new WaitForFixedUpdate();
        }
    }
}