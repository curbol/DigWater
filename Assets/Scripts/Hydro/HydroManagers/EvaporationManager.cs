using System;
using UnityEngine;

[Serializable]
public class EvaporationManager : Singleton<EvaporationManager>
{
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

    [SerializeField]
    private float heatPenetration;
    public static float HeatPenetration
    {
        get
        {
            return Instance.heatPenetration;
        }
    }
}