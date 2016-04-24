using System.Collections;
using UnityEngine;

public class AtmosphereLiquidBehavior : HydroBehavior
{
    private bool fadeInColorRunning;

    public override void InitializeState()
    {
        gameObject.layer = LayerMask.NameToLayer("Liquid");
        SpriteRenderer.color = LiquidManager.Color;
        Physics = LiquidManager.Physics;

        OnStartBehavior += () =>
        {
            StartCoroutine("FadeInColor", 2);
        };

        OnStopBehavior += () =>
        {
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

    private IEnumerator FadeInColor(float fadeTime)
    {
        fadeInColorRunning = true;
        float timer = 0;

        Color previousColor = LiquidManager.Color;
        previousColor.a = 0.35F;

        if (transform.InCloudZone())
            gameObject.layer = LayerMask.NameToLayer("Percipitation");

        while (timer < fadeTime)
        {
            SpriteRenderer.color = Color.Lerp(previousColor, LiquidManager.Color, timer / fadeTime);

            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        gameObject.layer = LayerMask.NameToLayer("Liquid");
        fadeInColorRunning = false;
    }
}