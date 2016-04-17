﻿using System.Collections;
using UnityEngine;

public class EvaporationBehavior : HydroBehavior
{
    private bool fadeInColorRunning;

    public override void InitializeState()
    {
        gameObject.layer = LayerMask.NameToLayer("Vapor");
        SpriteRenderer.color = EvaporationManager.Color;
        Physics = EvaporationManager.Physics;
        Rigidbody.velocity = Rigidbody.velocity.SetX(0);

        OnStartBehavior += () =>
        {
            StartCoroutine("FadeInColor", 1);
        };

        OnStopBehavior += () =>
        {
            StopCoroutine("FadeInColor");
        };
    }

    protected override void UpdatePhysicsBehavior()
    {
        if (Rigidbody == null)
            return;

        if (Rigidbody.transform.MapY() < CondensationManager.CloudLevelLowerBound)
        {
            float percentToCloudLevel = Mathf.Clamp(Rigidbody.transform.MapY() / CondensationManager.CloudLevel, 0, 1);
            float minSpeedY = EvaporationManager.Physics.VerticalMaxVelocity;
            float maxSpeedY = CondensationManager.Physics.VerticalMaxVelocity;
            float currentSpeedY = Mathf.Lerp(minSpeedY, maxSpeedY, percentToCloudLevel);
            float directionY = Rigidbody.transform.MapY() < CondensationManager.CloudLevel ? 1 : -1;

            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, directionY * currentSpeedY);
        }
        else if (Rigidbody.transform.MapY() < CondensationManager.EquilibriumZoneLowerBound)
        {
            Rigidbody.gravityScale = -CondensationManager.Physics.GravityScale;
        }
        else if (Rigidbody.transform.MapY() > CondensationManager.CloudLevelUpperBound)
        {
            Rigidbody.gravityScale = CondensationManager.Physics.GravityScale;
        }
        else
        {
            Rigidbody.gravityScale = 0;
        }
    }

    protected override void UpdateGraphicsBehavior()
    {
        if (SpriteRenderer == null || fadeInColorRunning)
            return;

        SpriteRenderer.color = EvaporationManager.Color;
    }

    private IEnumerator FadeInColor(float fadeTime)
    {
        fadeInColorRunning = true;
        float timer = 0;

        Color previousColor = SpriteRenderer.color;

        while (timer < fadeTime)
        {
            SpriteRenderer.color = Color.Lerp(previousColor, EvaporationManager.Color, timer / fadeTime);
            timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        fadeInColorRunning = false;
    }
}