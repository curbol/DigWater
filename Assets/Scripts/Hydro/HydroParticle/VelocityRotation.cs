using System.Collections;
using UnityEngine;

public class VelocityRotation : MonoBehaviour
{
    [SerializeField]
    private Transform transformToManipulate;

    [SerializeField]
    private Rigidbody2D rigidBody;

    private void Start()
    {
        StartCoroutine(UpdateRotation());
    }

    private IEnumerator UpdateRotation()
    {
        while (true)
        {
            yield return null;

            if (transformToManipulate == null || rigidBody.velocity == Vector2.zero)
            {
                continue;
            }

            transformToManipulate.rotation = Quaternion.LookRotation(Vector3.forward, rigidBody.velocity);
        }
    }
}