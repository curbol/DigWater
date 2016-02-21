using System.Collections;
using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    [SerializeField]
    private bool showGizmos;

    [SerializeField]
    private bool alwaysShowCursor;

    [SerializeField]
    private Vector2 maxShowCursorPosition;

    [SerializeField]
    private Vector2 minShowCursorPosition;

    private void Start()
    {
        StartCoroutine(UpdateCursor());
    }

    private IEnumerator UpdateCursor()
    {
        while (true)
        {
            Vector2 screenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            bool inShowCursorRegion =
                screenPosition.x <= maxShowCursorPosition.x &&
                screenPosition.y <= maxShowCursorPosition.y &&
                screenPosition.x >= minShowCursorPosition.x &&
                screenPosition.y >= minShowCursorPosition.y;

            Cursor.visible = inShowCursorRegion || alwaysShowCursor;
            bool showGameObject = alwaysShowCursor || !Cursor.visible;
            transform.GetChild(0).gameObject.SetActive(showGameObject);

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawLine(new Vector2(minShowCursorPosition.x, minShowCursorPosition.y), new Vector2(maxShowCursorPosition.x, minShowCursorPosition.y));
        Gizmos.DrawLine(new Vector2(minShowCursorPosition.x, maxShowCursorPosition.y), new Vector2(maxShowCursorPosition.x, maxShowCursorPosition.y));
        Gizmos.DrawLine(new Vector2(minShowCursorPosition.x, minShowCursorPosition.y), new Vector2(minShowCursorPosition.x, maxShowCursorPosition.y));
        Gizmos.DrawLine(new Vector2(maxShowCursorPosition.x, minShowCursorPosition.y), new Vector2(maxShowCursorPosition.x, maxShowCursorPosition.y));
    }
}