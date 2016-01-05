using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class WaterSpawner : MonoBehaviour
{
    private static readonly System.Random random = new System.Random();
    private static readonly string waterHolderName = "Water";

    private int hydroParticleCount;

    [SerializeField]
    private bool showGizmos;

    [Range(0, 2)]
    [SerializeField]
    private float spawnDelay = 0.5F;

    [Range(0, 10)]
    [SerializeField]
    private float spawnRadius = 2F;

    [SerializeField]
    private HydroParticle hydroParticlePrefab;

    private void Awake()
    {
        GameObject waterHolder = new GameObject(waterHolderName);
        waterHolder.transform.parent = transform;
    }

    private IEnumerator SpawnWater()
    {
        while (hydroParticlePrefab != null)
        {
            if (hydroParticleCount < HydroManager.MaximumNumberOfParticles)
            {
                float randomAdjustmentX = spawnRadius * random.Next(-100, 100) / 100F;
                float randomAdjustmentY = spawnRadius * random.Next(-100, 100) / 100F;
                Vector2 position = new Vector2(transform.position.x + randomAdjustmentX, transform.position.y + randomAdjustmentY);

                HydroParticle hydroParticle = Instantiate(hydroParticlePrefab, position, Quaternion.identity) as HydroParticle;
                hydroParticle.transform.parent = transform;
                hydroParticle.OnDeath += HydroParticleDeath;
                hydroParticleCount++;
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnWater());
    }

    private void HydroParticleDeath()
    {
        hydroParticleCount--;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Gizmos.color = new Color(0, 0, 1, 0.8F);
        foreach (HydroParticle hydroParticle in GetComponentsInChildren<HydroParticle>())
        {
            Gizmos.DrawSphere(hydroParticle.transform.position, 0.2F);
        }
    }
}