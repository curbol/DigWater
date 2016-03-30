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
        Rigidbody.gravityScale = HydroManager.GetProperties<EvaporationProperties>().Physics.GravityScale;
        Rigidbody.angularDrag = HydroManager.GetProperties<EvaporationProperties>().Physics.AngularDrag;
        Rigidbody.mass = HydroManager.GetProperties<EvaporationProperties>().Physics.Mass;
        HeatableObject.HeatPenetration = HydroManager.GetProperties<EvaporationProperties>().HeatPenetration;
        MoleculeVibration.EnergyLevel = HydroManager.GetProperties<HeatProperties>().MaximumEnergyLevel;

        previousColor = SpriteRenderer.color;
        colorFadePercent = 0;

        StopCoroutine("FadeInColor");
        StartCoroutine("FadeInColor");
    }

    protected override void UpdatePhysicsBehavior()
    {
        if (Rigidbody == null)
            return;

        if (Rigidbody.transform.MapY() < HydroManager.GetProperties<CondensationProperties>().CloudLevelLowerBound)
        {
            float percentToCloudLevel = Mathf.Clamp(Rigidbody.transform.MapY() / HydroManager.GetProperties<CondensationProperties>().CloudLevel, 0, 1);
            float minSpeedY = HydroManager.GetProperties<EvaporationProperties>().Physics.HorizontalMaxVelocity;
            float maxSpeedY = HydroManager.GetProperties<CondensationProperties>().Physics.HorizontalMaxVelocity;
            float currentSpeedY = Mathf.Lerp(minSpeedY, maxSpeedY, percentToCloudLevel);
            float directionY = Rigidbody.transform.MapY() < HydroManager.GetProperties<CondensationProperties>().CloudLevel ? 1 : -1;

            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, directionY * currentSpeedY);
        }
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

    protected override void UpdateGraphicsBehavior()
    {
        if (SpriteRenderer == null || colorFadePercent < 1)
            return;

        SpriteRenderer.color = HydroManager.GetProperties<EvaporationProperties>().Color;
    }

    protected override void UpdateTemperatureBehavior()
    {
        return;
    }

    private IEnumerator FadeInColor()
    {
        while (colorFadePercent < 1)
        {
            SpriteRenderer.color = Color.Lerp(previousColor, HydroManager.GetProperties<EvaporationProperties>().Color, Mathf.Clamp01(colorFadePercent));
            colorFadePercent += colorFadeRate * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}