using System;
using UnityEngine;

[Serializable]
public class CondensationManager : Singleton<CondensationManager>
{
    [SerializeField]
    private bool showGizmos;

    [SerializeField]
    private PhysicsProperties physicsProperties;
    public static PhysicsProperties Physics
    {
        get
        {
            return Instance.physicsProperties;
        }
    }

    [SerializeField]
    private Color color;
    public static Color Color
    {
        get
        {
            return Instance.color;
        }
    }

    [Range(0, 1)]
    [SerializeField]
    private float fadeRate;
    public static float FadeRate
    {
        get
        {
            return Instance.fadeRate;
        }
    }

    [SerializeField]
    private int cloudLevel;
    public static int CloudLevel
    {
        get
        {
            return Instance.cloudLevel;
        }
    }

    [SerializeField]
    private int cloudLevelThickness;
    public static int CloudLevelThickness
    {
        get
        {
            return Instance.cloudLevelThickness;
        }
    }

    public static float CloudLevelUpperBound
    {
        get
        {
            return CloudLevel + CloudLevelThickness / 2;
        }
    }

    public static float CloudLevelLowerBound
    {
        get
        {
            return CloudLevel - CloudLevelThickness / 2;
        }
    }

    [Range(0, 1)]
    [SerializeField]
    private float equilibriumZonePercent;
    public static float EquilibriumZonePercent
    {
        get
        {
            return Instance.equilibriumZonePercent;
        }
    }

    public static float EquilibriumZoneThickness
    {
        get
        {
            return EquilibriumZonePercent * CloudLevelThickness;
        }
    }

    public static float EquilibriumZoneUpperBound
    {
        get
        {
            return CloudLevel + EquilibriumZoneThickness / 2;
        }
    }

    public static float EquilibriumZoneLowerBound
    {
        get
        {
            return CloudLevel - EquilibriumZoneThickness / 2;
        }
    }

    [Range(0, 10)]
    [SerializeField]
    private float minimumNeighborCountForCluster;
    public static float MinimumNeighborCount
    {
        get
        {
            return Instance.minimumNeighborCountForCluster;
        }
    }

    [Range(0, 10)]
    [SerializeField]
    private float neighborSearchRadiusForCluster;
    public static float NeighborSearchRadius
    {
        get
        {
            return Instance.neighborSearchRadiusForCluster;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        float mapAdjustmentY = MapManager.Map.Height / 2;
        float mapAdjustmentX = MapManager.Map.Width / 2;

        Gizmos.color = new Color(0F, 0.2F, 1F, 0.8F);
        Gizmos.DrawLine(new Vector2(mapAdjustmentX, CloudLevelUpperBound - mapAdjustmentY), new Vector2(-mapAdjustmentX, CloudLevelUpperBound - mapAdjustmentY));
        Gizmos.DrawLine(new Vector2(mapAdjustmentX, CloudLevelLowerBound - mapAdjustmentY), new Vector2(-mapAdjustmentX, CloudLevelLowerBound - mapAdjustmentY));

        Gizmos.color = new Color(0F, 0.4F, 0.8F, 0.8F);
        Gizmos.DrawLine(new Vector2(mapAdjustmentX, EquilibriumZoneUpperBound - mapAdjustmentY), new Vector2(-mapAdjustmentX, EquilibriumZoneUpperBound - mapAdjustmentY));
        Gizmos.DrawLine(new Vector2(mapAdjustmentX, EquilibriumZoneLowerBound - mapAdjustmentY), new Vector2(-mapAdjustmentX, EquilibriumZoneLowerBound - mapAdjustmentY));
    }
}