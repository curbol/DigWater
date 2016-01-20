using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class Spawner : MonoBehaviour
{
    private static readonly System.Random random = new System.Random();
    private static readonly string objectHolderName = "Spawned Objects";

    private int objectCount;

    [SerializeField]
    private int maximumNumberOfParticles;

    [SerializeField]
    private bool showGizmos;

    [Range(0, 2)]
    [SerializeField]
    private float spawnDelay = 0.5F;

    [Range(0, 10)]
    [SerializeField]
    private float spawnRadius = 2F;

    [SerializeField]
    private RecyclableObject recyclableObjectPrefab;

    private void Awake()
    {
        GameObject waterHolder = new GameObject(objectHolderName);
        waterHolder.transform.parent = transform;
    }

    private IEnumerator SpawnWater()
    {
        while (recyclableObjectPrefab != null)
        {
            if (objectCount < maximumNumberOfParticles)
            {
                float randomAdjustmentX = spawnRadius * random.Next(-100, 100) / 100F;
                float randomAdjustmentY = spawnRadius * random.Next(-100, 100) / 100F;
                Vector2 position = new Vector2(transform.position.x + randomAdjustmentX, transform.position.y + randomAdjustmentY);

                RecyclableObject recyclableObject = Instantiate(recyclableObjectPrefab, position, Quaternion.identity) as RecyclableObject;
                recyclableObject.transform.parent = transform;
                recyclableObject.OnDeath += ObjectDeath;
                objectCount++;
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnWater());
    }

    private void ObjectDeath()
    {
        objectCount--;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Gizmos.color = new Color(0, 0, 1, 0.8F);
        foreach (RecyclableObject recyclableObject in GetComponentsInChildren<RecyclableObject>())
        {
            Gizmos.DrawSphere(recyclableObject.transform.position, 0.2F);
        }
    }
}