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
        Rigidbody.gravityScale = LiquidManager.Physics.GravityScale;
        Rigidbody.angularDrag = LiquidManager.Physics.AngularDrag;
        Rigidbody.mass = LiquidManager.Physics.Mass;
        HeatableObject.HeatPenetration = 0.5F;
        MoleculeVibration.EnergyLevel = 0;

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

        if (Rigidbody.velocity.magnitude > LiquidManager.Physics.HorizontalMaxVelocity)
            Rigidbody.velocity = Rigidbody.velocity.normalized * LiquidManager.Physics.HorizontalMaxVelocity;
    }

    protected override void UpdateGraphicsBehavior()
    {
        if (SpriteRenderer == null || colorFadePercent < 1)
            return;

        SpriteRenderer.color = LiquidManager.Color;
    }

    protected override void UpdateTemperatureBehavior()
    {
        HeatableObject.AddHeat(HeatManager.AmbientHeatRate * Time.deltaTime);
    }

    private IEnumerator SetUnheatable()
    {
        float temperature = HeatManager.EvaporationPoint;
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

        if (transform.InCloudZone())
            gameObject.layer = LayerMask.NameToLayer("Rain");

        Color startingColor = LiquidManager.Color;
        startingColor.a = 0.35F;

        while (colorFadePercent < 1)
        {
            SpriteRenderer.color = Color.Lerp(startingColor, LiquidManager.Color, Mathf.Clamp01(colorFadePercent));
            colorFadePercent += colorFadeRate * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        gameObject.layer = previousLayer;
    }
}