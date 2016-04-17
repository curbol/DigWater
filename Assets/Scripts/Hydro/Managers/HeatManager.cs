using System;
using UnityEngine;

[Serializable]
public class HeatManager : Singleton<HeatManager>
{
    [SerializeField]
    [Range(0, 1)]
    private float heatSlider;
    public static float HeatSlider
    {
        get
        {
            return Instance.heatSlider;
        }

        set
        {
            Instance.heatSlider = Mathf.Clamp01(value);
        }
    }

    public static Quandrant SliderQuadrant
    {
        get
        {
            if (HeatSlider <= 0.25F)
                return Quandrant.First;
            else if (HeatSlider > 0.25F && HeatSlider <= 0.5F)
                return Quandrant.Second;
            else if (HeatSlider > 0.5F && HeatSlider <= 0.75F)
                return Quandrant.Third;
            else
                return Quandrant.Forth;
        }
    }

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
    [Range(0, 1)]
    private float sunHeatPenetration;
    public static float SunHeatPenetration
    {
        get
        {
            return Instance.sunHeatPenetration;
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
    public void SetHeatSlider(float value)
    {
        HeatSlider = value;
    }
}