using UnityEngine;

public class HydroManager : Singleton<HydroManager>
{
    [SerializeField]
    private bool showGizmos;

    [SerializeField]
    private HeatProperties heatProperties;
    public static HeatProperties HeatProperties
    {
        get
        {
            return Instance.heatProperties;
        }
    }

    [SerializeField]
    private LiquidProperties liquidProperties;
    public static LiquidProperties LiquidProperties
    {
        get
        {
            return Instance.liquidProperties;
        }
    }

    [SerializeField]
    private VaporProperties vaporProperties;
    public static VaporProperties VaporProperties
    {
        get
        {
            return Instance.vaporProperties;
        }
    }

    [SerializeField]
    private CloudProperties cloudProperties;
    public static CloudProperties CloudProperties
    {
        get
        {
            return Instance.cloudProperties;
        }
    }

    public void SetHeat(float value)
    {
        if (HeatProperties.AmbientTemperatureChange == value)
            return;

        HeatProperties.AmbientTemperatureChange = value;
        //PlayerPrefs.SetFloat("HeatLevel", HeatProperties.AmbientTemperatureChange);
        //SceneManager.LoadScene("DigWater");
    }

    private void Awake()
    {
        //HeatProperties.AmbientTemperatureChange = PlayerPrefs.GetFloat("HeatLevel", HeatProperties.AmbientTemperatureChange);
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        float mapAdjustmentY = MapManager.Map.Height / 2;
        float mapAdjustmentX = MapManager.Map.Width / 2;

        Gizmos.color = new Color(0F, 0.2F, 1F, 0.8F);
        Gizmos.DrawLine(new Vector2(mapAdjustmentX, CloudProperties.CloudLevelUpperBound - mapAdjustmentY), new Vector2(-mapAdjustmentX, CloudProperties.CloudLevelUpperBound - mapAdjustmentY));
        Gizmos.DrawLine(new Vector2(mapAdjustmentX, CloudProperties.CloudLevelLowerBound - mapAdjustmentY), new Vector2(-mapAdjustmentX, CloudProperties.CloudLevelLowerBound - mapAdjustmentY));

        Gizmos.color = new Color(0F, 0.4F, 0.8F, 0.8F);
        Gizmos.DrawLine(new Vector2(mapAdjustmentX, CloudProperties.EquilibriumZoneUpperBound - mapAdjustmentY), new Vector2(-mapAdjustmentX, CloudProperties.EquilibriumZoneUpperBound - mapAdjustmentY));
        Gizmos.DrawLine(new Vector2(mapAdjustmentX, CloudProperties.EquilibriumZoneLowerBound - mapAdjustmentY), new Vector2(-mapAdjustmentX, CloudProperties.EquilibriumZoneLowerBound - mapAdjustmentY));
    }
}