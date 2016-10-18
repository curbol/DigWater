using System.Collections.Generic;
using UnityEngine;

public class UniqueActiveObjectController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> uniqueSceneObjects = new List<GameObject>();

    public void SelectTool(int toolIndex)
    {
        if (toolIndex < 0 || toolIndex >= uniqueSceneObjects.Count)
            return;

        for (int i = 0; i < uniqueSceneObjects.Count; i++)
            uniqueSceneObjects[i].SetActive(i == toolIndex);
    }
}