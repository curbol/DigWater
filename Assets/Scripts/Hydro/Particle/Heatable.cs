using UnityEngine;

public class Heatable : MonoBehaviour
{
    private static readonly System.Random random = new System.Random();

    [SerializeField]
    private float temperature;

    public bool IsHeatable { get; internal set; }

    public float Temperature
    {
        get
        {
            return temperature;
        }

        private set
        {
            temperature = Mathf.Clamp(value, 0, HeatManager.MaximumTemperature);
        }
    }

    public void AddHeat(float value)
    {
        if (IsHeatable)
            Temperature += value;
    }

    public void ReduceHeat(float value)
    {
        if (IsHeatable)
            Temperature -= value;
    }

    public void SetToMaximumTemperature()
    {
        Temperature = HeatManager.MaximumTemperature;
    }

    public void SetToMinimumTemperature()
    {
        Temperature = 0;
    }

    public void SetRandomTemperature(float min, float max)
    {
        Temperature = Mathf.Lerp(min, max, (float)random.NextDouble());
    }

    private void Awake()
    {
        IsHeatable = true;
    }
}