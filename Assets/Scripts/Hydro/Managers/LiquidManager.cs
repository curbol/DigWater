using System;
using UnityEngine;

[Serializable]
public class LiquidManager : Singleton<LiquidManager>
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
    [Range(0, 10)]
    private float deformability;
    public static float Deformability
    {
        get
        {
            return Instance.deformability;
        }
    }
}