using System.Collections;
using UnityEngine;

public class EvaporationBehavior : HydroBehavior
{
    private Color previousColor;
    private float colorFadePercent;
    private const float colorFadeRate = 0.8F;

    public override void InitializeState()
    {
        gameObject.layer = LayerMask.NameToLayer("Vapor");
        Rigidbody.velocity = Rigidbody.velocity.SetX(0);
        Rigidbody.gravityScale = EvaporationManager.Physics.GravityScale;
        Rigidbody.angularDrag = EvaporationManager.Physics.AngularDrag;
        Rigidbody.mass = EvaporationManager.Physics.Mass;
        HeatableObject.HeatPenetration = EvaporationManager.HeatPenetration;
        MoleculeVibration.EnergyLevel = 1;

        previousColor = SpriteRenderer.color;
        colorFadePercent = 0;

        StopCoroutine("FadeInColor");
        StartCoroutine("FadeInColor");
    }

    protected override void UpdatePhysicsBehavior()
    {
        if (Rigidbody == null)
            return;

        if (Rigidbody.transform.MapY() < CondensationManager.CloudLevelLowerBound)
        {
            float percentToCloudLevel = Mathf.Clamp(Rigidbody.transform.MapY() / CondensationManager.CloudLevel, 0, 1);
            float minSpeedY = EvaporationManager.Physics.HorizontalMaxVelocity;
            float maxSpeedY = CondensationManager.Physics.HorizontalMaxVelocity;
            float currentSpeedY = Mathf.Lerp(minSpeedY, maxSpeedY, percentToCloudLevel);
            float directionY = Rigidbody.transform.MapY() < CondensationManager.CloudLevel ? 1 : -1;

            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, directionY * currentSpeedY);
        }
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

    protected override void UpdateGraphicsBehavior()
    {
        if (SpriteRenderer == null || colorFadePercent < 1)
            return;

        SpriteRenderer.color = EvaporationManager.Color;
    }

    protected override void UpdateTemperatureBehavior()
    {
        return;
    }

    private IEnumerator FadeInColor()
    {
        while (colorFadePercent < 1)
        {
            SpriteRenderer.color = Color.Lerp(previousColor, EvaporationManager.Color, Mathf.Clamp01(colorFadePercent));
            colorFadePercent += colorFadeRate * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}