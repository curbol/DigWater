using System.Collections;
using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    [SerializeField]
    private bool showGizmos;

    [SerializeField]
    private bool alwaysShowCursor;

    [SerializeField]
    private Location2D showCursorLocation;

    private void OnEnable()
    {
        StartCoroutine("UpdateCursor");
    }

    private void OnDisable()
    {
        StopCoroutine("UpdateCursor");
    }

    private IEnumerator UpdateCursor()
    {
        while (true)
        {
            Vector2 screenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bool inShowCursorRegion = showCursorLocation.Contains(screenPosition);
            Cursor.visible = inShowCursorRegion || alwaysShowCursor;
            bool showGameObject = !inShowCursorRegion;

            transform.GetChild(0).gameObject.SetActive(showGameObject);

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawLine(new Vector2(showCursorLocation.BottomLeft.x, showCursorLocation.BottomLeft.y), new Vector2(showCursorLocation.TopRight.x, showCursorLocation.BottomLeft.y));
        Gizmos.DrawLine(new Vector2(showCursorLocation.BottomLeft.x, showCursorLocation.TopRight.y), new Vector2(showCursorLocation.TopRight.x, showCursorLocation.TopRight.y));
        Gizmos.DrawLine(new Vector2(showCursorLocation.BottomLeft.x, showCursorLocation.BottomLeft.y), new Vector2(showCursorLocation.BottomLeft.x, showCursorLocation.TopRight.y));
        Gizmos.DrawLine(new Vector2(showCursorLocation.TopRight.x, showCursorLocation.BottomLeft.y), new Vector2(showCursorLocation.TopRight.x, showCursorLocation.TopRight.y));
    }
}