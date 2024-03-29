﻿using System.Collections;
using UnityEngine;

public class AtmosphereLiquidBehavior : HydroBehavior
{
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
        return;
    }

    private IEnumerator FadeInColor(float fadeTime)
    {
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
    }
}