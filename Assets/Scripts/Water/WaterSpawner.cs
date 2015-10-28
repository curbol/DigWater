using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class WaterSpawner : MonoBehaviour
{
    private static readonly System.Random random = new System.Random();
    private static readonly string waterHolderName = "Water";

    public Transform waterParticlePrefab;

    [Range(0, 2)]
    public float spawnDelay = 0.5f;

    [Range(0, 10)]
    public float spawnRadius = 2f;

    private void Awake()
    {
        GameObject waterHolder = new GameObject(waterHolderName);
        waterHolder.transform.parent = transform;
    }

	private void Start()
	{
        StartCoroutine(SpawnWater());
    }

    private IEnumerator SpawnWater()
    {
        while (waterParticlePrefab != null)
        {
            float randomAdjustmentX = spawnRadius * random.Next(-100, 100) / 100f;
            float randomAdjustmentY = spawnRadius * random.Next(-100, 100) / 100f;
            Vector2 position = new Vector2(transform.position.x + randomAdjustmentX, transform.position.y + randomAdjustmentY);

            Transform waterParticle = Instantiate(waterParticlePrefab, position, Quaternion.identity) as Transform;
            waterParticle.parent = transform;

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}