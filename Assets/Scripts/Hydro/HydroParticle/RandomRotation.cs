using System.Collections;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    private static readonly System.Random random = new System.Random();

    private bool rotateClockwise;

    [SerializeField]
    private Transform transformToManipulate;

    [SerializeField]
    [Range(0, 0.1F)]
    private float rotationDirectionChangeProbability = 0.01F;

    private void Start()
    {
        transformToManipulate.Rotate(Vector3.forward, random.Next(360));
        rotateClockwise = random.NextDouble() > 0.5F;

        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            int rotationIncrement = rotateClockwise ? 1 : -1;
            transformToManipulate.Rotate(Vector3.forward, rotationIncrement);

            if (random.NextDouble() < rotationDirectionChangeProbability)
            {
                rotateClockwise = !rotateClockwise;
            }

            yield return null;
        }
    }
}