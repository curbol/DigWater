using System;
using UnityEngine;

[Serializable]
public class LiquidProperties : IHydroProperties
{
    [SerializeField]
    private PhysicsProperties physicsProperties;
    public PhysicsProperties Physics
    {
        get
        {
            return physicsProperties;
        }
    }

    [SerializeField]
    private Color color;
    public Color Color
    {
        get
        {
            return color;
        }
    }

    [SerializeField]
    [Range(0, 10)]
    private float deformability;
    public float Deformability
    {
        get
        {
            return deformability;
        }
    }
}