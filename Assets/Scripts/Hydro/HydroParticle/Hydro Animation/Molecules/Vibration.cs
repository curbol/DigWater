using System.Collections;
using UnityEngine;

public class Vibration : MonoBehaviour
{
    private static readonly System.Random random = new System.Random();
    private readonly float vibrationAngle = random.Next(180);

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
            energyLevel = value;
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
            Vector2 vibrationDirection = vibrationAngle.DegreeToVector();
            Vector2 minPoint = (Vector2)transformToManipulate.parent.position - vibrationDirection * vibrationTravelDistance;
            Vector2 maxPoint = (Vector2)transformToManipulate.parent.position + vibrationDirection * vibrationTravelDistance;
            transformToManipulate.position = Vector2.Lerp(minPoint, maxPoint, (Mathf.Sin(Time.time * EnergyLevel) + 1) / 2);

            yield return null;
        }
    }
}