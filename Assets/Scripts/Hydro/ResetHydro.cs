using UnityEngine;

public class ResetHydro : MonoBehaviour
{
    [SerializeField]
    private Transform spawnerPrefab;

    [SerializeField]
    private string spawnerHolderName = "Hydro Spawner";

    private void Start()
    {
        //Reset();
    }

    public void Reset()
    {
        Reset(0);
    }

    public void Reset(float value)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Transform spawner = Instantiate(spawnerPrefab, transform.position, Quaternion.identity) as Transform;
        spawner.parent = transform;
    }
}