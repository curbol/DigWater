using System;
using UnityEngine;

[Serializable]
public class HeatManager : Singleton<HeatManager>
{
    [SerializeField]
    [Range(0, 1)]
    private float ambientCoolRate;
    public static float AmbientCoolRate
    {
        get
        {
            return Instance.ambientCoolRate;
        }

        set
        {
            Instance.ambientCoolRate = Mathf.Clamp01(value);
        }
    }

    public static float AmbientHeatRate
    {
        get
        {
            return -AmbientCoolRate;
        }
    }

    [SerializeField]
    [Range(0, 1)]
    private float sunHeat;
    public static float SunHeat
    {
        get
        {
            return Instance.sunHeat;
        }
    }

    [SerializeField]
    private float maximumTemperature;
    public static float MaximumTemperature
    {
        get
        {
            return Instance.maximumTemperature;
        }
    }

    [SerializeField]
    private float evaporationPoint;
    public static float EvaporationPoint
    {
        get
        {
            return Instance.evaporationPoint;
        }
    }

    // used for slider event
    public void SetCoolRate(float value)
    {
        AmbientCoolRate = value;
    }

    public void SetHeatRate(float value)
    {
        AmbientCoolRate = -value;
    }
}