using UnityEngine;

public class WaterBehavior : HydroStateBehavior
{
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
    }

    public override void RunPhysicsBehavior()
    {
        return;
    }

    public override void RunGraphicsBehavior()
    {
        SpriteRenderer.color = Color.Lerp(HydroManager.VaporProperties.Color, HydroManager.LiquidProperties.Color, (1 - PercentToVaporizationPoint) + 0.6F);
    }

    public override void RunTemperatureBehavior()
    {
        HeatableObject.AddHeat(HydroManager.AmbientTemperatureChange * Time.deltaTime);
    }
}