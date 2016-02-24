using System;
using UnityEngine;

[Serializable]
public class Vapor
{
    [SerializeField]
    private Physics physics;
    public Physics Physics
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