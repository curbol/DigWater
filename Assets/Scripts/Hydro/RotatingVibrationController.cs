using System.Collections;
using UnityEngine;

public class RotatingVibrationController : MonoBehaviour
{
    private static readonly System.Random random = new System.Random();

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

    private float vibrationAngle;
    private bool rotateClockwise;
    private float rotationDirectionChangeProbability;
    private float travelDistance;

    private void Start()
    {
        transform.Rotate(Vector3.forward, random.Next(360));
        vibrationAngle = random.Next(180);
        rotateClockwise = random.NextDouble() > 0.5F;
        rotationDirectionChangeProbability = 0.1F;
        travelDistance = 0.1F;

        StartCoroutine(Vibrate());
    }

    private IEnumerator Vibrate()
    {
        while (true)
        {
            Vector2 vector = Vector2Extentions.Vector2FromAngle(vibrationAngle);
            transform.position = Vector2.Lerp((Vector2)transform.parent.position - vector * travelDistance, (Vector2)transform.parent.position + vector * travelDistance, (Mathf.Sin(Time.time * EnergyLevel) + 1) / 2);

            int increment = rotateClockwise ? 1 : -1;
            vibrationAngle += increment;
            transform.Rotate(Vector3.forward, increment);

            if (random.NextDouble() < rotationDirectionChangeProbability)
            {
                rotateClockwise = !rotateClockwise;
            }

            yield return null;
        }
    }
}