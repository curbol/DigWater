using System.Collections;
using UnityEngine;

public class Vibration : MonoBehaviour
{
    private static readonly System.Random random = new System.Random();
    private readonly float vibrationAngle = random.Next(180);
    private readonly float energyLevelAdjustment = Mathf.Lerp(-HydroManager.EnergyLevelDeviation, HydroManager.EnergyLevelDeviation, (float)random.NextDouble());

    private float PercentToMaximumTemperature
    {
        get
        {
            return Mathf.Clamp(HeatableObject.Temperature / HydroManager.MaximumTemperature, 0, 1);
        }
    }

    private float EnergyLevel
    {
        get
        {
            return Mathf.Lerp(HydroManager.MinimumEnergyLevel, HydroManager.MaximumEnergyLevel, PercentToMaximumTemperature) + energyLevelAdjustment;
        }
    }

    [SerializeField]
    private Heatable heatableObject;
    private Heatable HeatableObject
    {
        get
        {
            if (heatableObject == null)
                heatableObject = gameObject.GetSafeComponent<Heatable>();

            return heatableObject;
        }
    }

    [SerializeField]
    private Transform transformToManipulate;

    [SerializeField]
    [Range(0, 0.5F)]
    private float vibrationTravelDistance = 0.1F;

    private void Start()
    {
        StartCoroutine(Vibrate());
    }

    private IEnumerator Vibrate()
    {
        while (true)
        {
            Vector2 vibrationDirection = Vector2Extentions.Vector2FromAngle(vibrationAngle);
            Vector2 minPoint = (Vector2)transformToManipulate.parent.position - vibrationDirection * vibrationTravelDistance;
            Vector2 maxPoint = (Vector2)transformToManipulate.parent.position + vibrationDirection * vibrationTravelDistance;
            transformToManipulate.position = Vector2.Lerp(minPoint, maxPoint, (Mathf.Sin(Time.time * EnergyLevel) + 1) / 2);

            yield return null;
        }
    }
}