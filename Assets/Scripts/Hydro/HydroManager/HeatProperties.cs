using System;
using UnityEngine;

[Serializable]
public class HeatProperties
{
    [SerializeField]
    [Range(-10, 10)]
    private float ambientTemperatureChange;
    public float AmbientTemperatureChange
    {
        get
        {
            return ambientTemperatureChange;
        }

        set
        {
            ambientTemperatureChange = value;
        }
    }

    [SerializeField]
    [Range(-10, 10)]
    private float sunHeat;
    public float SunHeat
    {
        get
        {
            return sunHeat;
        }

        set
        {
            sunHeat = value;
        }
    }

    [SerializeField]
    private float maximumTemperature;
    public float MaximumTemperature
    {
        get
        {
            return maximumTemperature;
        }
    }

    [SerializeField]
    private float vaporizationPoint;
    public float VaporizationPoint
    {
        get
        {
            return vaporizationPoint;
        }
    }

    [SerializeField]
    private float minimumEnergyLevel;
    public float MinimumEnergyLevel
    {
        get
        {
            return minimumEnergyLevel;
        }
    }

    [SerializeField]
    private float maximumEnergyLevel;
    public float MaximumEnergyLevel
    {
        get
        {
            return maximumEnergyLevel;
        }
    }

    [SerializeField]
    private float energyLevelDeviation;
    public float EnergyLevelDeviation
    {
        get
        {
            return energyLevelDeviation;
        }
    }
}