using System;
using UnityEngine;

[Serializable]
public class Heat
{
    [SerializeField]
    [Range(-10, 10)]
    private float minimumAmbientTemperatureChange;
    public float MinimumAmbientTemperatureChange
    {
        get
        {
            return minimumAmbientTemperatureChange;
        }

        set
        {
            minimumAmbientTemperatureChange = value;
        }
    }

    [SerializeField]
    [Range(-10, 10)]
    private float maximumAmbientTemperatureChange;
    public float MaximumAmbientTemperatureChange
    {
        get
        {
            return maximumAmbientTemperatureChange;
        }

        set
        {
            maximumAmbientTemperatureChange = value;
        }
    }

    [SerializeField]
    [Range(-10, 10)]
    private float currentAmbientTemperatureChange;
    public float CurrentAmbientTemperatureChange
    {
        get
        {
            return currentAmbientTemperatureChange;
        }

        set
        {
            currentAmbientTemperatureChange = value;
        }
    }

    public float AmbientTemperatureChangePercentage
    {
        get
        {
            return (CurrentAmbientTemperatureChange - MinimumAmbientTemperatureChange) / (MaximumAmbientTemperatureChange - MinimumAmbientTemperatureChange);
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
    public float EvaporationPoint
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