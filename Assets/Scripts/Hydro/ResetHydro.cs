using System.Collections;
using UnityEngine;

public class ResetHydro : MonoBehaviour
{
    private static readonly System.Random random = new System.Random();

    [SerializeField]
    private Transform spawnerPrefab;

    [SerializeField]
    private float resetDelaySeconds = 0.1F;

    private float lastAttemptedResetTime;

    public void Reset()
    {
        Reset(0);
    }

    public void Reset(float value)
    {
        StopCoroutine("ResetHydroMoleculesWithDelay");

        if (Time.time - lastAttemptedResetTime <= resetDelaySeconds)
        {
            StartCoroutine("ResetHydroMoleculesWithDelay");
        }
        else
        {
            ResetHydroMolecules();
        }

        lastAttemptedResetTime = Time.time;
    }

    private IEnumerator ResetHydroMoleculesWithDelay()
    {
        yield return new WaitForSeconds(resetDelaySeconds);

        ResetHydroMolecules();
    }

    private void ResetHydroMolecules()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Transform spawner = Instantiate(spawnerPrefab, transform.position, Quaternion.identity) as Transform;
        spawner.parent = transform;

        SetRandomHeat(spawner.GetComponentsInChildren<Heatable>());
    }

    private static void SetRandomHeat(Heatable[] heatableObjects)
    {
        foreach (Heatable heatableObject in heatableObjects)
        {
            float temperature = HydroManager.Heat.EvaporationPoint;
            heatableObject.Temperature = random.Next((int)(temperature * 0.5F), (int)(temperature * 0.9F));
        }
    }
}