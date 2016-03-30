using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HydroManager : Singleton<HydroManager>
{
    [SerializeField]
    private bool showGizmos;

    [SerializeField]
    private List<IHydroProperties> hydroProperties;

    public static T GetProperties<T>() where T : IHydroProperties
    {
        if (Instance.hydroProperties == null)
            Instance.hydroProperties = new List<IHydroProperties>();

        T properties = Instance.hydroProperties.OfType<T>().FirstOrDefault();

        return properties;
    }

    public void SetHeat(float value)
    {
        HeatProperties heatProperties = GetProperties<HeatProperties>();
        if (heatProperties == null || heatProperties.CurrentAmbientTemperatureChange == value)
            return;

        heatProperties.CurrentAmbientTemperatureChange = value;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        CondensationProperties condensationProperties = GetProperties<CondensationProperties>();
        if (condensationProperties == null)
            return;

        float mapAdjustmentY = MapManager.Map.Height / 2;
        float mapAdjustmentX = MapManager.Map.Width / 2;

        Gizmos.color = new Color(0F, 0.2F, 1F, 0.8F);
        Gizmos.DrawLine(new Vector2(mapAdjustmentX, condensationProperties.CloudLevelUpperBound - mapAdjustmentY), new Vector2(-mapAdjustmentX, condensationProperties.CloudLevelUpperBound - mapAdjustmentY));
        Gizmos.DrawLine(new Vector2(mapAdjustmentX, condensationProperties.CloudLevelLowerBound - mapAdjustmentY), new Vector2(-mapAdjustmentX, condensationProperties.CloudLevelLowerBound - mapAdjustmentY));

        Gizmos.color = new Color(0F, 0.4F, 0.8F, 0.8F);
        Gizmos.DrawLine(new Vector2(mapAdjustmentX, condensationProperties.EquilibriumZoneUpperBound - mapAdjustmentY), new Vector2(-mapAdjustmentX, condensationProperties.EquilibriumZoneUpperBound - mapAdjustmentY));
        Gizmos.DrawLine(new Vector2(mapAdjustmentX, condensationProperties.EquilibriumZoneLowerBound - mapAdjustmentY), new Vector2(-mapAdjustmentX, condensationProperties.EquilibriumZoneLowerBound - mapAdjustmentY));
    }
}