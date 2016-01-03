using System;
using UnityEngine;

[Serializable]
public class VaporProperties
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
    private float baseAcceleration;
    public float BaseAcceleration
    {
        get
        {
            return baseAcceleration;
        }
    }

    [SerializeField]
    private float maximumVelocity;
    public float MaximumVelocity
    {
        get
        {
            return maximumVelocity;
        }
    }
}