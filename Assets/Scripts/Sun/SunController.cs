using System.Collections;
using System.Linq;
using UnityEngine;

public class SunController : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(EmitRays());
    }

    private IEnumerator EmitRays()
    {
        while (true)
        {
            for (int i = -90; i <= 90; i++)
            {
                transform.rotation = Quaternion.Euler(0, 0, i);
                Vector2 direction = transform.TransformDirection(Vector2.down);
                RaycastHit2D[] raycastHits = Physics2D.RaycastAll(transform.position, direction, Mathf.Infinity);

                if (raycastHits.Length > 0)
                {
                    WaterParticle waterParticle = raycastHits.First().transform.GetComponent<WaterParticle>() as WaterParticle;
                    if (waterParticle != null)
                    {
                        Debug.DrawLine(transform.position, raycastHits.First().point, new Color(0.5F, 0.5F, 0.2F, 0.2F));
                        waterParticle.Heat();
                    }
                }
            }

            yield return null;
        }
    }
}