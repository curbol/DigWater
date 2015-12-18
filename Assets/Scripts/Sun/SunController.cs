using System.Collections;
using UnityEngine;

public class SunController : MonoBehaviour
{
    [Range(-90, 0)]
    [SerializeField]
    private int startRayAngle = -90;

    [Range(0, 90)]
    [SerializeField]
    private int endRayAngle = 90;

    [Range(0, 1)]
    [SerializeField]
    private float obstaclePercentHeatDecrease = 0.1F;

    private void Start()
    {
        StartCoroutine(EmitRays());
    }

    private IEnumerator EmitRays()
    {
        while (true)
        {
            for (int i = startRayAngle; i <= endRayAngle; i++)
            {
                transform.rotation = Quaternion.Euler(0, 0, i);
                Vector2 direction = transform.TransformDirection(Vector2.down);
                RaycastHit2D[] raycastHits = Physics2D.RaycastAll(transform.position, direction, Mathf.Infinity);
                float heatPercent = 1;

                foreach (RaycastHit2D raycastHit in raycastHits)
                {
                    WaterParticle waterParticle = raycastHit.transform.GetComponent<WaterParticle>() as WaterParticle;
                    if (waterParticle == null)
                        break;

                    Debug.DrawLine(transform.position, raycastHit.point, new Color(0.5F, 0.5F, 0.2F, 1F));
                    waterParticle.Heat(heatPercent);
                    heatPercent -= obstaclePercentHeatDecrease;

                    if (waterParticle.State == WaterState.Water || heatPercent <= 0)
                        break;
                }
            }

            yield return null;
        }
    }
}