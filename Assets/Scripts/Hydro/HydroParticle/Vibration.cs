using System.Collections;
using UnityEngine;

public class Vibration : MonoBehaviour
{
    private static readonly System.Random random = new System.Random();

    private float vibrationAngle;
    private float energyLevelDeviation;

    [SerializeField]
    private Transform transformToManipulate;

    [SerializeField]
    private float energyLevel;
    public float EnergyLevel
    {
        get
        {
            return energyLevel;
        }

        set
        {
            energyLevel = Mathf.Max(0, value);
        }
    }

    [SerializeField]
    [Range(0, 0.5F)]
    private float vibrationTravelDistance = 0.1F;

    private void Start()
    {
        vibrationAngle = random.Next(180);

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