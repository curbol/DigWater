using System.Collections;
using UnityEngine;

public class WaterBehavior : HydroStateBehavior
{
    private float unheatableDuration;

    public float PercentToVaporizationPoint
    {
        get
        {
            return Mathf.Clamp(HeatableObject.Temperature / HydroManager.VaporizationPoint, 0, 1);
        }
    }

    public override void InitializeState()
    {
        gameObject.layer = LayerMask.NameToLayer("Metaball");
        Rigidbody.gravityScale = 1;
        Rigidbody.angularDrag = 0;
        HeatableObject.HeatPenetration = 0.5F;

        unheatableDuration = 1;
        StartCoroutine(SetUnheatable());
    }

    public override void RunPhysicsBehavior()
    {
        return;
    }

    public override void RunGraphicsBehavior()
    {
        if (SpriteRenderer == null)
            return;

        SpriteRenderer.color = HydroManager.LiquidProperties.Color;

        if (unheatableDuration > 0)
        {
            SpriteRenderer.color = Color.red;
        }
    }

    public override void RunTemperatureBehavior()
    {
        HeatableObject.AddHeat(HydroManager.AmbientTemperatureChange * Time.deltaTime);
    }

    private IEnumerator SetUnheatable()
    {
        while (unheatableDuration > 0)
        {
            HeatableObject.Temperature = 0;
            unheatableDuration -= 0.1F * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}