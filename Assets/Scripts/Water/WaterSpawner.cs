using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class WaterSpawner : MonoBehaviour
{
    [Range(0, 2)]
    public float spawnDelay = 0.5F;

    [Range(0, 10)]
    public float spawnRadius = 2F;

    public Transform waterParticlePrefab;
    private static readonly System.Random random = new System.Random();
    private static readonly string waterHolderName = "Water";

    private void Awake()
    {
        GameObject waterHolder = new GameObject(waterHolderName);
        waterHolder.transform.parent = transform;
    }

    private IEnumerator SpawnWater()
    {
        while (waterParticlePrefab != null)
        {
            float randomAdjustmentX = spawnRadius * random.Next(-100, 100) / 100F;
            float randomAdjustmentY = spawnRadius * random.Next(-100, 100) / 100F;
            Vector2 position = new Vector2(transform.position.x + randomAdjustmentX, transform.position.y + randomAdjustmentY);

            Transform waterParticle = Instantiate(waterParticlePrefab, position, Quaternion.identity) as Transform;
            waterParticle.parent = transform;

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnWater());
    }
}