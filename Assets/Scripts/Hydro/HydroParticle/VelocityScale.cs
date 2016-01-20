using System.Collections;
using UnityEngine;

public class VelocityScale : MonoBehaviour
{
    [SerializeField]
    private Transform transformToManipulate;

    [SerializeField]
    private Rigidbody2D rigidBody;

    private void Start()
    {
        StartCoroutine(UpdateVelocityScale());
    }

    private IEnumerator UpdateVelocityScale()
    {
        while (true)
        {
            yield return null;

            if (transformToManipulate == null)
            {
                continue;
            }
            else if (rigidBody.velocity.magnitude < 0.5F)
            {
                transformToManipulate.localScale = Vector3.one;
                continue;
            }

            Vector2 scale = Vector2.one;
            float scaleModifier = Mathf.Min(Mathf.Abs(rigidBody.velocity.y) * (HydroManager.Deformability / 100), 0.5F);
            scale.x -= scaleModifier;
            scale.y += scaleModifier;

            transformToManipulate.localScale = scale;
        }
    }
}