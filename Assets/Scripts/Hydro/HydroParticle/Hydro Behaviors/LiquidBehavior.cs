using System.Collections;
using UnityEngine;

public class LiquidBehavior : HydroBehavior
{
    private static readonly System.Random random = new System.Random();

    private const float colorFadeRate = 0.2F;

    private float colorFadePercent;

    public override void InitializeState()
    {
        gameObject.layer = LayerMask.NameToLayer("Metaball");
        Rigidbody.gravityScale = HydroManager.Liquid.Physics.GravityScale;
        Rigidbody.angularDrag = HydroManager.Liquid.Physics.AngularDrag;
        Rigidbody.mass = HydroManager.Liquid.Physics.Mass;
        HeatableObject.HeatPenetration = HydroManager.Liquid.HeatPenetration;
        MoleculeVibration.EnergyLevel = HydroManager.Heat.MinimumEnergyLevel;

        colorFadePercent = 0;
        StopCoroutine("SetUnheatable");
        StopCoroutine("FadeInColor");

        StartCoroutine("SetUnheatable");
        StartCoroutine("FadeInColor");
    }

    protected override void UpdatePhysicsBehavior()
    {
        if (Rigidbody == null)
            return;

        if (Rigidbody.velocity.magnitude > HydroManager.Liquid.Physics.MaximumVelocity)
            Rigidbody.velocity = Rigidbody.velocity.normalized * HydroManager.Liquid.Physics.MaximumVelocity;
    }

    protected override void UpdateGraphicsBehavior()
    {
        if (SpriteRenderer == null || colorFadePercent < 1)
            return;

        SpriteRenderer.color = HydroManager.Liquid.Color;
    }

    protected override void UpdateTemperatureBehavior()
    {
        HeatableObject.AddHeat(HydroManager.Heat.CurrentAmbientTemperatureChange * Time.deltaTime);
    }

    private IEnumerator SetUnheatable()
    {
        float temperature = HydroManager.Heat.EvaporationPoint;
        float newTemperature = random.Next((int)(temperature * 0.5F), (int)(temperature * 0.9F)); ;

        while (colorFadePercent < 1 && Rigidbody.velocity.magnitude > 0.01)
        {
            HeatableObject.Temperature = newTemperature;

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator FadeInColor()
    {
        int previousLayer = gameObject.layer;

        if (transform.InCloudRegion())
            gameObject.layer = LayerMask.NameToLayer("Rain");

        Color startingColor = HydroManager.Liquid.Color;
        startingColor.a = 0.35F;

        while (colorFadePercent < 1)
        {
            SpriteRenderer.color = Color.Lerp(startingColor, HydroManager.Liquid.Color, Mathf.Clamp01(colorFadePercent));
            colorFadePercent += colorFadeRate * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        gameObject.layer = previousLayer;
    }
}