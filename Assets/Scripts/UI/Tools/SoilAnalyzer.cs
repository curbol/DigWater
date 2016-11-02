using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoilAnalyzer : MonoBehaviour
{
    public Action<Vector2> AnalyzeSoilEvent { get; set; }

    private void Update()
    {
        if (Input.GetMouseButton((int)MouseButton.Left) && AnalyzeSoilEvent != null)
        {
            var selectedObject = EventSystem.current.currentSelectedGameObject;
            if (selectedObject == null || selectedObject.gameObject.layer != LayerMask.NameToLayer("UI"))
            {
                Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                AnalyzeSoilEvent(mouseScreenPosition);
            }
        }
    }
}