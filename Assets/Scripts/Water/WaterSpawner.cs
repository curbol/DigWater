using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class WaterSpawner : MonoBehaviour
{
    private static readonly System.Random random = new System.Random();
    private static readonly string waterHolderName = "Water";
    private Queue<WaterParticle> waterParticleQueue;

    public WaterParticle waterParticlePrefab;

    [Range(0, 2)]
    public float spawnDelay = 0.5f;

    [Range(0, 10)]
    public float spawnRadius = 2f;

    private void Awake()
    {
        GameObject waterHolder = new GameObject(waterHolderName);
        waterHolder.transform.parent = transform;
        waterParticleQueue = new Queue<WaterParticle>();
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

            WaterParticle waterParticle;

            if (waterParticleQueue.Count > 0)
            {
                waterParticle = waterParticleQueue.Dequeue();
                waterParticle.Initialize();
                waterParticle.transform.position = position;
                waterParticle.transform.localScale = waterParticlePrefab.transform.localScale;
            }
            else
            {
                waterParticle = Instantiate(waterParticlePrefab, position, Quaternion.identity) as WaterParticle;
            }

            waterParticle.OnDeath += () => waterParticleQueue.Enqueue(waterParticle);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}