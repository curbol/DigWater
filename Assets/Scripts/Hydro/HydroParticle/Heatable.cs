using UnityEngine;

public class Heatable : MonoBehaviour
{
    [SerializeField]
    private float maximumTemperature;
    public float MaximumTemperature
    {
        get
        {
            return maximumTemperature;
        }

        set
        {
            maximumTemperature = value;
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

        set
        {
            heatPenetration = value;
        }
    }

    private float temperature;
    public float Temperature
    {
        get
        {
            return temperature;
        }

        set
        {
            temperature = Mathf.Clamp(value, 0, MaximumTemperature);
        }
    }

    public void AddHeat(float value)
    {
        Temperature += value;
    }
}