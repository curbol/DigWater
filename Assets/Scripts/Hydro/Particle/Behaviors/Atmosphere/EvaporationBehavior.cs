using System.Collections;
using UnityEngine;

public class EvaporationBehavior : HydroBehavior
{
    private bool fadeInColorRunning;

    [SerializeField]
    private Heatable heatable;
    private Heatable Heatable
    {
        get
        {
            if (heatable == null)
                heatable = gameObject.GetSafeComponent<Heatable>();

            return heatable;
        }
    }

    public override void InitializeState()
    {
        gameObject.layer = LayerMask.NameToLayer("Evaporation");
        SpriteRenderer.color = EvaporationManager.Color;
        Physics = EvaporationManager.Physics;
        Rigidbody.velocity = Rigidbody.velocity.SetX(0);
        Heatable.IsHeatable = false;

        OnStartBehavior += () =>
        {
            StartCoroutine("FadeInColor", 1);
        };

        OnStopBehavior += () =>
        {
            StopAllCoroutines();
        };
    }

    protected override void UpdatePhysicsBehavior()
    {
        if (Rigidbody == null)
            return;

        if (Rigidbody.transform.MapY() < CondensationManager.CloudLevelLowerBound)
        {
            float percentToCloudLevel = Mathf.Clamp(Rigidbody.transform.MapY() / CondensationManager.CloudLevel, 0, 1);
            float minSpeedY = Mathf.Min(EvaporationManager.Physics.MaxVelocityY, CondensationManager.Physics.MaxVelocityY);
            float maxSpeedY = Mathf.Max(EvaporationManager.Physics.MaxVelocityY, CondensationManager.Physics.MaxVelocityY);
            float currentSpeedY = Mathf.Lerp(minSpeedY, maxSpeedY, percentToCloudLevel);
            float directionY = Rigidbody.transform.MapY() < CondensationManager.CloudLevel ? 1 : -1;

            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, directionY * currentSpeedY);
        }
        else if (Rigidbody.transform.MapY() < CondensationManager.EquilibriumZoneLowerBound)
        {
            Rigidbody.gravityScale = -EvaporationManager.Physics.GravityScale;
        }
        else if (Rigidbody.transform.MapY() > CondensationManager.EquilibriumZoneUpperBound)
        {
            Rigidbody.gravityScale = EvaporationManager.Physics.GravityScale;
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