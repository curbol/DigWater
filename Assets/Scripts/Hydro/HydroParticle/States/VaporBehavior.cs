﻿using System.Collections;
using UnityEngine;

public class VaporBehavior : HydroStateBehavior
{
    private Color previousColor;
    private float colorFadePercent;
    private const float colorFadeRate = 0.8F;

    public override void InitializeState()
    {
        gameObject.layer = LayerMask.NameToLayer("Vapor");
        Rigidbody.velocity = Rigidbody.velocity.SetX(0);
        Rigidbody.gravityScale = 0;
        Rigidbody.angularDrag = 0;
        Rigidbody.mass = 0.1F;
        HeatableObject.HeatPenetration = HydroManager.VaporProperties.HeatPenetration;
        MoleculeVibration.EnergyLevel = HydroManager.HeatProperties.MaximumEnergyLevel;

        previousColor = SpriteRenderer.color;
        colorFadePercent = 0;

        StopCoroutine("FadeInColor");
        StartCoroutine("FadeInColor");
    }

    public override void RunPhysicsBehavior()
    {
        if (Rigidbody == null)
            return;

        if (Rigidbody.transform.MapY() < HydroManager.CloudProperties.CloudLevelLowerBound)
        {
            float percentToCloudLevel = Mathf.Clamp(Rigidbody.transform.MapY() / HydroManager.CloudProperties.CloudLevel, 0, 1);
            float minSpeedY = HydroManager.VaporProperties.MaximumVelocity;
            float maxSpeedY = HydroManager.CloudProperties.MaximumVelocity;
            float currentSpeedY = Mathf.Lerp(minSpeedY, maxSpeedY, percentToCloudLevel);
            float directionY = Rigidbody.transform.MapY() < HydroManager.CloudProperties.CloudLevel ? 1 : -1;

            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, directionY * currentSpeedY);
        }
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

    public override void RunGraphicsBehavior()
    {
        if (SpriteRenderer == null || colorFadePercent < 1)
            return;

        SpriteRenderer.color = HydroManager.VaporProperties.Color;
    }

    public override void RunTemperatureBehavior()
    {
        return;
    }

    private IEnumerator FadeInColor()
    {
        while (colorFadePercent < 1)
        {
            SpriteRenderer.color = Color.Lerp(previousColor, HydroManager.VaporProperties.Color, Mathf.Clamp01(colorFadePercent));
            colorFadePercent += colorFadeRate * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}