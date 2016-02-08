using System.Collections;
using UnityEngine;

public class ResetHydro : MonoBehaviour
{
    [SerializeField]
    private Transform spawnerPrefab;

    [SerializeField]
    private string spawnerHolderName = "Hydro Spawner";

    [SerializeField]
    private float resetDelaySeconds = 0.1F;

    private void Start()
    {
    }

    public void Reset()
    {
        Reset(0);
    }

    public void Reset(float value)
    {
        StopCoroutine("ResetWithDelay");
        StartCoroutine("ResetWithDelay");
    }

    public IEnumerator ResetWithDelay()
    {
        yield return new WaitForSeconds(resetDelaySeconds);

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Transform spawner = Instantiate(spawnerPrefab, transform.position, Quaternion.identity) as Transform;
        spawner.parent = transform;
    }
}