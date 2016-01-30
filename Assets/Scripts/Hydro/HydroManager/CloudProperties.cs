using System;
using UnityEngine;

[Serializable]
public class CloudProperties
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

    [Range(0, 1)]
    [SerializeField]
    private float fadeRate;
    public float FadeRate
    {
        get
        {
            return fadeRate;
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

    [Range(0, 1)]
    [SerializeField]
    private float drag;
    public float Drag
    {
        get
        {
            return drag;
        }
    }

    [SerializeField]
    private float cloudLevel;
    public float CloudLevel
    {
        get
        {
            return cloudLevel;
        }
    }

    [SerializeField]
    private float cloudLevelThickness;
    public float CloudLevelThickness
    {
        get
        {
            return cloudLevelThickness;
        }
    }

    public float CloudLevelUpperBound
    {
        get
        {
            return CloudLevel + CloudLevelThickness / 2;
        }
    }

    public float CloudLevelLowerBound
    {
        get
        {
            return CloudLevel - CloudLevelThickness / 2;
        }
    }

    [Range(0, 1)]
    [SerializeField]
    private float equilibriumZonePercent;
    public float EquilibriumZonePercent
    {
        get
        {
            return equilibriumZonePercent;
        }
    }

    public float EquilibriumZoneThickness
    {
        get
        {
            return EquilibriumZonePercent * CloudLevelThickness;
        }
    }

    public float EquilibriumZoneUpperBound
    {
        get
        {
            return CloudLevel + EquilibriumZoneThickness / 2;
        }
    }

    public float EquilibriumZoneLowerBound
    {
        get
        {
            return CloudLevel - EquilibriumZoneThickness / 2;
        }
    }

    [Range(0, 10)]
    [SerializeField]
    private float minimumNeighborCount;
    public float MinimumNeighborCount
    {
        get
        {
            return minimumNeighborCount;
        }
    }

    [Range(0, 10)]
    [SerializeField]
    private float neighborSearchRadius;
    public float NeighborSearchRadius
    {
        get
        {
            return neighborSearchRadius;
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