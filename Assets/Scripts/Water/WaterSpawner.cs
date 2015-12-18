using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class WaterSpawner : MonoBehaviour
{
    private static readonly System.Random random = new System.Random();
    private static readonly string waterHolderName = "Water";

    private int waterParticleCount;

    [Range(0, 2)]
    public float spawnDelay = 0.5F;

    [Range(0, 10)]
    public float spawnRadius = 2F;

    public WaterParticle waterParticlePrefab;

    private void Awake()
    {
        GameObject waterHolder = new GameObject(waterHolderName);
        waterHolder.transform.parent = transform;
    }

    private IEnumerator SpawnWater()
    {
        while (waterParticlePrefab != null)
        {
            if (waterParticleCount < WaterManager.MaximumNumberOfParticles)
            {
                float randomAdjustmentX = spawnRadius * random.Next(-100, 100) / 100F;
                float randomAdjustmentY = spawnRadius * random.Next(-100, 100) / 100F;
                Vector2 position = new Vector2(transform.position.x + randomAdjustmentX, transform.position.y + randomAdjustmentY);

                WaterParticle waterParticle = Instantiate(waterParticlePrefab, position, Quaternion.identity) as WaterParticle;
                waterParticle.transform.parent = transform;
                waterParticle.OnDeath += WaterParticleDeath;
                waterParticleCount++;
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnWater());
    }

    private void WaterParticleDeath()
    {
        waterParticleCount--;
    }
}