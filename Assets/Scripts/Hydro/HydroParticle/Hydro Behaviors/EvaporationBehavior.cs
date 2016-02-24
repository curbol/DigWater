﻿using System.Collections;
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
        Rigidbody.gravityScale = HydroManager.Vapor.Physics.GravityScale;
        Rigidbody.angularDrag = HydroManager.Vapor.Physics.AngularDrag;
        Rigidbody.mass = HydroManager.Vapor.Physics.Mass;
        HeatableObject.HeatPenetration = HydroManager.Vapor.HeatPenetration;
        MoleculeVibration.EnergyLevel = HydroManager.Heat.MaximumEnergyLevel;

        previousColor = SpriteRenderer.color;
        colorFadePercent = 0;

        StopCoroutine("FadeInColor");
        StartCoroutine("FadeInColor");
    }

    protected override void UpdatePhysicsBehavior()
    {
        if (Rigidbody == null)
            return;

        if (Rigidbody.transform.MapY() < HydroManager.Cloud.CloudLevelLowerBound)
        {
            float percentToCloudLevel = Mathf.Clamp(Rigidbody.transform.MapY() / HydroManager.Cloud.CloudLevel, 0, 1);
            float minSpeedY = HydroManager.Vapor.Physics.MaximumVelocity;
            float maxSpeedY = HydroManager.Cloud.Physics.MaximumVelocity;
            float currentSpeedY = Mathf.Lerp(minSpeedY, maxSpeedY, percentToCloudLevel);
            float directionY = Rigidbody.transform.MapY() < HydroManager.Cloud.CloudLevel ? 1 : -1;

            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, directionY * currentSpeedY);
        }
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

    protected override void UpdateGraphicsBehavior()
    {
        if (SpriteRenderer == null || colorFadePercent < 1)
            return;

        SpriteRenderer.color = HydroManager.Vapor.Color;
    }

    protected override void UpdateTemperatureBehavior()
    {
        return;
    }

    private IEnumerator FadeInColor()
    {
        while (colorFadePercent < 1)
        {
            SpriteRenderer.color = Color.Lerp(previousColor, HydroManager.Vapor.Color, Mathf.Clamp01(colorFadePercent));
            colorFadePercent += colorFadeRate * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}