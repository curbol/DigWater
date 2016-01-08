using System.Collections;
using UnityEngine;

public class MagnifyingGlassController : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
        StartCoroutine(FollowMouse());
    }

    private IEnumerator FollowMouse()
    {
        while (true)
        {
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, -0.5F);

            yield return null;
        }
    }
}