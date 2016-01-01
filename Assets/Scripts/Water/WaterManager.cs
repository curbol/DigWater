using UnityEngine;

public class WaterManager : MonoBehaviour
{
    [SerializeField]
    private bool showGizmos;

    private static WaterManager instance;
    private static WaterManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (WaterManager)FindObjectOfType(typeof(WaterManager));
                if (instance == null)
                {
                    instance = new GameObject("WaterManager").AddComponent<WaterManager>();
                }
            }

            return instance;
        }
    }

    [SerializeField]
    private int maximumNumberOfParticles;
    public static int MaximumNumberOfParticles
    {
        get
        {
            return Instance.maximumNumberOfParticles;
        }
    }

    [SerializeField]
    private Color waterColor;
    public static Color WaterColor
    {
        get
        {
            return Instance.waterColor;
        }
    }

    [SerializeField]
    private Color vaporColor;
    public static Color VaporColor
    {
        get
        {
            return Instance.vaporColor;
        }
    }

    [SerializeField]
    private Color cloudColor;
    public static Color CloudColor
    {
        get
        {
            return Instance.cloudColor;
        }
    }

    [Range(0, 10)]
    [SerializeField]
    private float deformability;
    public static float Deformability
    {
        get
        {
            return Instance.deformability;
        }
    }

    [SerializeField]
    private float heatRate;
    public static float HeatRate
    {
        get
        {
            return Instance.heatRate;
        }
    }

    [SerializeField]
    private float coolRate;
    public static float CoolRate
    {
        get
        {
            return Instance.coolRate;
        }
    }

    [SerializeField]
    private float maximumTemperature;
    public static float MaximumTemperature
    {
        get
        {
            return Instance.maximumTemperature;
        }
    }

    [SerializeField]
    private float vaporizationPoint;
    public static float VaporizationPoint
    {
        get
        {
            return Instance.vaporizationPoint;
        }
    }

    [SerializeField]
    private float vaporAcceleration;
    public static float VaporAcceleration
    {
        get
        {
            return Instance.vaporAcceleration;
        }
    }

    [SerializeField]
    private float vaporMaximumVelocity;
    public static float VaporMaximumVelocity
    {
        get
        {
            return Instance.vaporMaximumVelocity;
        }
    }

    [SerializeField]
    private float cloudLevel;
    public static float CloudLevel
    {
        get
        {
            return Instance.cloudLevel;
        }
    }

    [SerializeField]
    private float cloudLevelBuffer;
    public static float CloudLevelBuffer
    {
        get
        {
            return Instance.cloudLevelBuffer;
        }
    }

    [SerializeField]
    private float cloudLevelEquilibriumZoneSize;
    public static float CloudLevelEquilibriumZoneSize
    {
        get
        {
            return Instance.cloudLevelEquilibriumZoneSize;
        }
    }

    [Range(0, 10)]
    [SerializeField]
    private float cloudClusterMinimumCount;
    public static float CloudClusterMinimumCount
    {
        get
        {
            return Instance.cloudClusterMinimumCount;
        }
    }

    [Range(0, 10)]
    [SerializeField]
    private float cloudClusterRadius;
    public static float CloudClusterRadius
    {
        get
        {
            return Instance.cloudClusterRadius;
        }
    }

    [Range(0, 1)]
    [SerializeField]
    private float cloudClusterTransitionRate;
    public static float CloudClusterTransitionRate
    {
        get
        {
            return Instance.cloudClusterTransitionRate;
        }
    }

    [Range(0, 1)]
    [SerializeField]
    private float cloudDrag = 0.2F;
    public static float CloudDrag
    {
        get
        {
            return Instance.cloudDrag;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        float cloudLevelY = CloudLevel - MapManager.Map.Height / 2;

        Gizmos.color = new Color(0F, 0.2F, 1F, 0.8F);
        Gizmos.DrawLine(new Vector2(MapManager.Map.Width / 2F, cloudLevelY), new Vector2(-MapManager.Map.Width / 2F, cloudLevelY));
        Gizmos.DrawLine(new Vector2(MapManager.Map.Width / 2F, cloudLevelY - CloudLevelBuffer), new Vector2(-MapManager.Map.Width / 2F, cloudLevelY - CloudLevelBuffer));
        Gizmos.DrawLine(new Vector2(MapManager.Map.Width / 2F, cloudLevelY + CloudLevelBuffer), new Vector2(-MapManager.Map.Width / 2F, cloudLevelY + CloudLevelBuffer));

        Gizmos.color = new Color(0F, 0.4F, 0.8F, 0.8F);
        Gizmos.DrawLine(new Vector2(MapManager.Map.Width / 2F, cloudLevelY - CloudLevelEquilibriumZoneSize), new Vector2(-MapManager.Map.Width / 2F, cloudLevelY - CloudLevelEquilibriumZoneSize));
        Gizmos.DrawLine(new Vector2(MapManager.Map.Width / 2F, cloudLevelY + CloudLevelEquilibriumZoneSize), new Vector2(-MapManager.Map.Width / 2F, cloudLevelY + CloudLevelEquilibriumZoneSize));
    }
}