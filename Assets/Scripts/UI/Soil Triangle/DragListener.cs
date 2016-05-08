using System.Collections;
using UnityEngine;

public class DragListener : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 0.3F;

    private Vector2 MouseVelocity
    {
        get
        {
            return Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")), maxSpeed);
        }
    }

    private bool previousCursorVisibility;
    private CursorLockMode previousCursorLockState;

    public void OnBeginDrag()
    {
        previousCursorVisibility = Cursor.visible;
        Cursor.visible = false;
        previousCursorLockState = Cursor.lockState;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine("MoveTowardMouseDirection");
    }

    public void OnDrag()
    {
        return;
    }

    public void OnEndDrag()
    {
        Cursor.visible = previousCursorVisibility;
        Cursor.lockState = previousCursorLockState;
        StopAllCoroutines();
    }

    private IEnumerator MoveTowardMouseDirection()
    {
        while (true)
        {
            transform.position += (Vector3)MouseVelocity;
            yield return new WaitForEndOfFrame();
        }
    }
}