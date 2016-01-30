using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectHydroStates : MonoBehaviour
{
    [SerializeField]
    private float detectionRadius = 2;

    public List<int> FoundStates { get; private set; }

    private void Awake()
    {
        FoundStates = new List<int>();
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
            foreach (Collider2D neighbor in neighbors)
            {
                bool isHydroState = neighbor.gameObject.layer == LayerMask.NameToLayer("Metaball")
                                 || neighbor.gameObject.layer == LayerMask.NameToLayer("Vapor")
                                 || neighbor.gameObject.layer == LayerMask.NameToLayer("Cloud")
                                 || neighbor.gameObject.layer == LayerMask.NameToLayer("Rain");

                if (isHydroState && !FoundStates.Contains(neighbor.gameObject.layer))
                {
                    FoundStates.Add(neighbor.gameObject.layer);
                }
            }

            yield return null;
        }
    }
}