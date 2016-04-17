using UnityEngine;

public class Heatable : MonoBehaviour
{
    private static readonly System.Random random = new System.Random();

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
            temperature = Mathf.Clamp(value, 0, HeatManager.MaximumTemperature);
        }
    }

    public void AddHeat(float value)
    {
        Temperature += value;
    }

    public void SetRandomTemperature(float min, float max)
    {
        Temperature = Mathf.Lerp(min, max, (float)random.NextDouble());
    }
}