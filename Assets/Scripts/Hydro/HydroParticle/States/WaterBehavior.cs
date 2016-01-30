using System.Collections;
using UnityEngine;

public class WaterBehavior : HydroStateBehavior
{
    private const float colorFadeRate = 0.2F;

    private float colorFadePercent;

    public float PercentToVaporizationPoint
    {
        get
        {
            return Mathf.Clamp(HeatableObject.Temperature / HydroManager.HeatProperties.VaporizationPoint, 0, 1);
        }
    }

    public override void InitializeState()
    {
        gameObject.layer = LayerMask.NameToLayer("Metaball");
        Rigidbody.gravityScale = 1;
        Rigidbody.angularDrag = 0;
        HeatableObject.HeatPenetration = HydroManager.LiquidProperties.HeatPenetration;
        MoleculeVibration.EnergyLevel = HydroManager.HeatProperties.MinimumEnergyLevel;

        colorFadePercent = 0;
        StartCoroutine(SetUnheatable());
        StartCoroutine(FadeInColor());
    }

    public override void RunPhysicsBehavior()
    {
        return;
    }

    public override void RunGraphicsBehavior()
    {
        if (SpriteRenderer == null || colorFadePercent < 1)
            return;

        SpriteRenderer.color = HydroManager.LiquidProperties.Color;
    }

    public override void RunTemperatureBehavior()
    {
        HeatableObject.AddHeat(HydroManager.HeatProperties.AmbientTemperatureChange * Time.deltaTime);
    }

    private IEnumerator SetUnheatable()
    {
        while (colorFadePercent < 1)
        {
            HeatableObject.Temperature = 0;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeInColor()
    {
        int previousLayer = gameObject.layer;

        if (transform.InCloudRegion())
            gameObject.layer = LayerMask.NameToLayer("Rain");

        Color startingColor = HydroManager.LiquidProperties.Color;
        startingColor.a = 0.35F;

        while (colorFadePercent < 1)
        {
            SpriteRenderer.color = Color.Lerp(startingColor, HydroManager.LiquidProperties.Color, Mathf.Clamp01(colorFadePercent));
            colorFadePercent += colorFadeRate * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        gameObject.layer = previousLayer;
    }
}