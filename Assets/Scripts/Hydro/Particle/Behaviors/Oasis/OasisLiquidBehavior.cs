using System.Collections;
using UnityEngine;

public class OasisLiquidBehavior : HydroBehavior
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
        Coordinate coordinate = MapManager.Map.GetCoordinateFromPosition(transform.position);
        SoilType soilType = MapManager.Map[coordinate.X, coordinate.Y];

        if (soilType == SoilType.None)
            Physics = LiquidManager.Physics;
        else
            Physics = soilType.SoilTypeMetadata().FluidPhysics;
    }

    protected override void UpdateGraphicsBehavior()
    {
        return;
    }

    private IEnumerator FadeInColor(float fadeTime)
    {
        fadeInColorRunning = true;
        float timer = 0;

        Color previousColor = LiquidManager.Color;
        previousColor.a = 0.35F;

        while (timer < fadeTime)
        {
            SpriteRenderer.color = Color.Lerp(previousColor, LiquidManager.Color, timer / fadeTime);

            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        fadeInColorRunning = false;
    }
}