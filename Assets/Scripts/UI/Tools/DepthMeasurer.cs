using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DepthMeasurer : MonoBehaviour
{
    public Action<Vector2> MeasureDepthEvent { get; set; }

    private void Update()
    {
        if (Input.GetMouseButton((int)MouseButton.Left) && MeasureDepthEvent != null)
        {
            var selectedObject = EventSystem.current.currentSelectedGameObject;
            if (selectedObject == null || selectedObject.gameObject.layer != LayerMask.NameToLayer("UI"))
            {
                Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                MeasureDepthEvent(mouseScreenPosition);
            }
        }
    }
}