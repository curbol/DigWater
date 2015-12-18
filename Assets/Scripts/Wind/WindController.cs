using System.Collections;
using UnityEngine;

public class WindController : MonoBehaviour
{
    [Range(0, 50)]
    [SerializeField]
    private int emmissionHeight = 10;

    [Range(0, 2)]
    [SerializeField]
    private float force = 0.1F;

    private void Start()
    {
        StartCoroutine(EmitWind());
    }

    private IEnumerator EmitWind()
    {
        while (true)
        {
            for (int i = -emmissionHeight / 2; i <= emmissionHeight / 2; i++)
            {
                Vector2 startPosition = transform.position + Vector3.up * i;
                Vector2 direction = Vector2.left;
                RaycastHit2D[] raycastHits = Physics2D.RaycastAll(startPosition, direction, Mathf.Infinity);

                foreach (RaycastHit2D raycastHit in raycastHits)
                {
                    WaterParticle waterParticle = raycastHit.transform.GetComponent<WaterParticle>() as WaterParticle;
                    if (waterParticle != null)
                    {
                        Debug.DrawLine(startPosition, raycastHit.point, new Color(0.5F, 0.2F, 0.2F, 0.2F));
                        waterParticle.RigidBody.AddForce(direction * force);
                        break;
                    }
                }
            }

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position + Vector3.down * emmissionHeight / 2, transform.position + Vector3.up * emmissionHeight / 2, Color.red);
    }
}