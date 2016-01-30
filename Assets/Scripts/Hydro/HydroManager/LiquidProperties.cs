using System;
using UnityEngine;

[Serializable]
public class LiquidProperties
{
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