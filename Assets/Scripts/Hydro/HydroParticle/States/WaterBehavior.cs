using System.Collections;
using UnityEngine;

public class WaterBehavior : HydroStateBehavior
{
    private static readonly System.Random random = new System.Random();

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
        Rigidbody.mass = 1;
        HeatableObject.HeatPenetration = HydroManager.LiquidProperties.HeatPenetration;
        MoleculeVibration.EnergyLevel = HydroManager.HeatProperties.MinimumEnergyLevel;

        colorFadePercent = 0;
        StopCoroutine("SetUnheatable");
        StopCoroutine("FadeInColor");

        StartCoroutine("SetUnheatable");
        StartCoroutine("FadeInColor");
    }

    public override void RunPhysicsBehavior()
    {
        if (Rigidbody == null)
            return;

        if (Rigidbody.velocity.magnitude > HydroManager.LiquidProperties.MaximumVelocity)
            Rigidbody.velocity = Rigidbody.velocity.normalized * HydroManager.LiquidProperties.MaximumVelocity;
    }

    public override void RunGraphicsBehavior()
    {
        if (SpriteRenderer == null || colorFadePercent < 1)
            return;

        SpriteRenderer.color = HydroManager.LiquidProperties.Color;
    }

    public override void RunTemperatureBehavior()
    {
        HeatableObject.AddHeat(HydroManager.HeatProperties.CurrentAmbientTemperatureChange * Time.deltaTime);
    }

    private IEnumerator SetUnheatable()
    {
        float temperature = HydroManager.HeatProperties.VaporizationPoint;
        float newTemperature = random.Next((int)(temperature * 0.5F), (int)(temperature * 0.9F)); ;

        while (colorFadePercent < 1 && Rigidbody.velocity.magnitude > 0.01)
        {
            HeatableObject.Temperature = newTemperature;

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