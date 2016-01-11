using System.Collections;
using UnityEngine;

public class MagnifyingGlassController : MonoBehaviour
{
    [SerializeField]
    private bool showCursor;

    private void Start()
    {
        StartCoroutine(FollowMouse());
    }

    private IEnumerator FollowMouse()
    {
        while (true)
        {
            Cursor.visible = showCursor;
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y);

            yield return null;
        }
    }
}