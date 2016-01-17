using System.Collections;
using UnityEngine;

public class DefomationController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Rigidbody2D rigidBody;

    private void Start()
    {
        StartCoroutine(UpdateRotation());
        StartCoroutine(UpdateVelocityScale());
    }

    private IEnumerator UpdateRotation()
    {
        while (true)
        {
            if (spriteRenderer != null && rigidBody.velocity != Vector2.zero)
            {
                spriteRenderer.transform.rotation = Quaternion.LookRotation(Vector3.forward, rigidBody.velocity);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator UpdateVelocityScale()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (spriteRenderer != null && rigidBody.velocity.magnitude < 0.5F)
            {
                spriteRenderer.transform.localScale = Vector3.one;
                continue;
            }

            Vector2 scale = Vector2.one;
            float scaleModifier = Mathf.Min(Mathf.Abs(rigidBody.velocity.y) * (HydroManager.Deformability / 100), 0.5F);
            scale.x -= scaleModifier;
            scale.y += scaleModifier;

            if (spriteRenderer != null)
                spriteRenderer.transform.localScale = scale;
        }
    }
}