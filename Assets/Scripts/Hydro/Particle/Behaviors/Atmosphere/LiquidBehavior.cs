using System.Collections;
using UnityEngine;

public class LiquidBehavior : HydroBehavior
{
    private bool fadeInColorRunning;

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

    public override void InitializeState()
    {
        gameObject.layer = LayerMask.NameToLayer("Metaball");
        SpriteRenderer.color = LiquidManager.Color;
        Physics = LiquidManager.Physics;

        OnStartBehavior += () =>
        {
            StartCoroutine("FadeInColor", 2);
            StartCoroutine("RunTemperatureBehavior");
        };

        OnStopBehavior += () =>
        {
            StopCoroutine("RunTemperatureBehavior");
            StopCoroutine("FadeInColor");
        };
    }

    protected override void UpdatePhysicsBehavior()
    {
        return;
    }

    protected override void UpdateGraphicsBehavior()
    {
        if (SpriteRenderer == null || fadeInColorRunning)
            return;

        SpriteRenderer.color = LiquidManager.Color;
    }

    private IEnumerator RunTemperatureBehavior()
    {
        while (true)
        {
            HeatableObject.AddHeat(HeatManager.AmbientHeatRate * Time.deltaTime);

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator FadeInColor(float fadeTime)
    {
        fadeInColorRunning = true;
        float timer = 0;

        Color previousColor = LiquidManager.Color;
        previousColor.a = 0.35F;

        if (transform.InCloudZone())
            gameObject.layer = LayerMask.NameToLayer("Rain");

        while (timer < fadeTime)
        {
            HeatableObject.Temperature = 0;
            SpriteRenderer.color = Color.Lerp(previousColor, LiquidManager.Color, timer / fadeTime);

            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        HeatableObject.SetRandomTemperature(HeatManager.EvaporationPoint * 0.5F, HeatManager.EvaporationPoint * 0.9F);

        gameObject.layer = LayerMask.NameToLayer("Metaball");
        fadeInColorRunning = false;
    }
}