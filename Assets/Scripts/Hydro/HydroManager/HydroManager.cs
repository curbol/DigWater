using UnityEngine;

public class HydroManager : Singleton<HydroManager>
{
    [SerializeField]
    private bool showGizmos;

    [SerializeField]
    private Heat heatProperties;
    public static Heat Heat
    {
        get
        {
            return Instance.heatProperties;
        }
    }

    [SerializeField]
    private Liquid liquidProperties;
    public static Liquid Liquid
    {
        get
        {
            return Instance.liquidProperties;
        }
    }

    [SerializeField]
    private Vapor vaporProperties;
    public static Vapor Vapor
    {
        get
        {
            return Instance.vaporProperties;
        }
    }

    [SerializeField]
    private Cloud cloudProperties;
    public static Cloud Cloud
    {
        get
        {
            return Instance.cloudProperties;
        }
    }

    public void SetHeat(float value)
    {
        if (Heat.CurrentAmbientTemperatureChange == value)
            return;

        Heat.CurrentAmbientTemperatureChange = value;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        float mapAdjustmentY = MapManager.Map.Height / 2;
        float mapAdjustmentX = MapManager.Map.Width / 2;

        Gizmos.color = new Color(0F, 0.2F, 1F, 0.8F);
        Gizmos.DrawLine(new Vector2(mapAdjustmentX, Cloud.CloudLevelUpperBound - mapAdjustmentY), new Vector2(-mapAdjustmentX, Cloud.CloudLevelUpperBound - mapAdjustmentY));
        Gizmos.DrawLine(new Vector2(mapAdjustmentX, Cloud.CloudLevelLowerBound - mapAdjustmentY), new Vector2(-mapAdjustmentX, Cloud.CloudLevelLowerBound - mapAdjustmentY));

        Gizmos.color = new Color(0F, 0.4F, 0.8F, 0.8F);
        Gizmos.DrawLine(new Vector2(mapAdjustmentX, Cloud.EquilibriumZoneUpperBound - mapAdjustmentY), new Vector2(-mapAdjustmentX, Cloud.EquilibriumZoneUpperBound - mapAdjustmentY));
        Gizmos.DrawLine(new Vector2(mapAdjustmentX, Cloud.EquilibriumZoneLowerBound - mapAdjustmentY), new Vector2(-mapAdjustmentX, Cloud.EquilibriumZoneLowerBound - mapAdjustmentY));
    }
}