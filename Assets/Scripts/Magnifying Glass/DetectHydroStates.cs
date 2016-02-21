using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DetectHydroStates : MonoBehaviour
{
    [SerializeField]
    private bool showGizmos = true;

    [SerializeField]
    private float detectionRadius = 2;

    [SerializeField]
    private float detectionCountThreshold = 10;

    private Dictionary<int, int> layerDetectionCounts;

    public List<int> FoundStates { get; private set; }

    public void Reset()
    {
        Reset(0);
    }

    public void Reset(float value)
    {
        layerDetectionCounts = new Dictionary<int, int>();
        layerDetectionCounts[LayerMask.NameToLayer("Vapor")] = 0;
        layerDetectionCounts[LayerMask.NameToLayer("Cloud")] = 0;
        layerDetectionCounts[LayerMask.NameToLayer("Rain")] = 0;

        FoundStates = new List<int>();
    }

    private void Awake()
    {
        Reset();
    }

    private void Start()
    {
        StartCoroutine(DetectStates());
    }

    private IEnumerator DetectStates()
    {
        while (true)
        {
            Collider2D[] neighbors = transform.position.GetNeighbors(detectionRadius);

            IEnumerable<int> neighborLayers = neighbors.Select(a => a.gameObject.layer).Distinct().Where(a => layerDetectionCounts.Keys.Contains(a));

            foreach (int keyLayer in layerDetectionCounts.Keys.OfType<int>().ToList())
            {
                if (neighborLayers.Contains(keyLayer) && layerDetectionCounts[keyLayer] < detectionCountThreshold)
                {
                    layerDetectionCounts[keyLayer]++;
                }
                else if (layerDetectionCounts[keyLayer] > 0)
                {
                    layerDetectionCounts[keyLayer]--;
                }

                if (!FoundStates.Contains(keyLayer) && layerDetectionCounts[keyLayer] >= detectionCountThreshold)
                {
                    Debug.Log("Detected " + LayerMask.LayerToName(keyLayer));
                    FoundStates.Add(keyLayer);
                }
            }

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Gizmos.color = new Color(0, 0, 1, 0.5F);
        Gizmos.DrawSphere(transform.position, detectionRadius);
    }
}