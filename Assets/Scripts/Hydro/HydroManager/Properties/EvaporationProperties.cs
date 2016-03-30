using System;
using UnityEngine;

[Serializable]
public class EvaporationProperties : IHydroProperties
{
    [SerializeField]
    private PhysicsProperties physics;
    public PhysicsProperties Physics
    {
        get
        {
            return physics;
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
    private float heatPenetration;
    public float HeatPenetration
    {
        get
        {
            return heatPenetration;
        }
    }
}