using UnityEngine;

public class Heatable : MonoBehaviour
{
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

    [SerializeField]
    private float temperature;
    public float Temperature
    {
        get
        {
            return temperature;
        }

        set
        {
            temperature = Mathf.Clamp(value, 0, HydroManager.Heat.MaximumTemperature);
        }
    }

    public void AddHeat(float value)
    {
        Temperature += value;
    }
}