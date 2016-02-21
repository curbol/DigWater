﻿using System.Collections;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Follow());
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
}