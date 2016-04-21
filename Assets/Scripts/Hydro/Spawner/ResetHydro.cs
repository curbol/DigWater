using System.Collections;
using UnityEngine;

public class ResetHydro : MonoBehaviour
{
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
    }
}