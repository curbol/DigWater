using System.Collections;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    [SerializeField]
    private bool showCursor;

    private void Start()
    {
        StartCoroutine(Follow());
        StartCoroutine(SetMouseVisibility());
    }

    private IEnumerator Follow()
    {
        while (true)
        {
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y);

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator SetMouseVisibility()
    {
        while (true)
        {
            Cursor.visible = showCursor;

            yield return new WaitForEndOfFrame();
        }
    }
}