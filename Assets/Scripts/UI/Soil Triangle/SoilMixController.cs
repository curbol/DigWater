using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SoilMixController : MonoBehaviour
{
    [SerializeField]
    private Text soilMixText;

    [SerializeField]
    private RawImage soilMixImage;

    [SerializeField]
    private Transform ratioLocator;

    [SerializeField]
    private Vector2 ratioLocatorReachableArea;

    private readonly Dictionary<float, string> soilMixes = new Dictionary<float, string>
    {
        { 0.07F, "Sandy Clay" },
        { 0.14F, "Clay" },
        { 0.21F, "Silty Clay" },
        { 0.27F, "Sandy Clay Loam" },
        { 0.34F, "Clay Loam" },
        { 0.41F, "Silt Clay Loam" },
        { 0.47F, "Sand" },
        { 0.54F, "Loamy Sand" },
        { 0.61F, "Sandy Loam" },
        { 0.67F, "Loam" },
        { 0.74F, "Silt Loam" },
        { 0.81F, "Silt" }
    };

    private Texture2D SoilMixImageTexture
    {
        get
        {
            return (Texture2D)soilMixImage.texture;
        }
    }

    private Vector2 soilMixImageWorldDimensions;
    private Vector2 SoilMixImageWorldDimensions
    {
        get
        {
            if (soilMixImageWorldDimensions == Vector2.zero)
            {
                var fourCornersArray = new Vector3[4];
                soilMixImage.GetComponent<RectTransform>().GetWorldCorners(fourCornersArray);
                soilMixImageWorldDimensions = new Vector2(fourCornersArray[3].x - fourCornersArray[0].x, fourCornersArray[1].y - fourCornersArray[0].y);
            }

            return soilMixImageWorldDimensions;
        }
    }

    private void Start()
    {
        StartCoroutine(GetSoulMix());
    }

    public IEnumerator GetSoulMix()
    {
        Vector2 previousPosition = Vector2.zero;

        Vector2 bottomLeft = new Vector2(transform.position.x - ratioLocatorReachableArea.x / 2, transform.position.y - ratioLocatorReachableArea.y / 2);
        Vector2 topCenter = bottomLeft + new Vector2(ratioLocatorReachableArea.x / 2, ratioLocatorReachableArea.y);
        Vector2 bottomRight = bottomLeft + new Vector2(ratioLocatorReachableArea.x, 0);

        while (true)
        {
            yield return new WaitForEndOfFrame();

            Vector2 relativeWorldPosition = ratioLocator.position - soilMixImage.transform.position;
            if (relativeWorldPosition == previousPosition)
                continue;
            else
                previousPosition = relativeWorldPosition;

            Vector2 relativePercent = new Vector2(Mathf.Clamp01(relativeWorldPosition.x / SoilMixImageWorldDimensions.x + 0.5F), Mathf.Clamp01(relativeWorldPosition.y / SoilMixImageWorldDimensions.y + 0.5F));
            Vector2 imagePixelPosition = new Vector2((int)(SoilMixImageTexture.width * relativePercent.x), (int)(SoilMixImageTexture.height * relativePercent.y));
            Color pixelColor = SoilMixImageTexture.GetPixel((int)imagePixelPosition.x, (int)imagePixelPosition.y);
            var soilMix = soilMixes.FirstOrDefault(a => pixelColor.r < a.Key);

            float distanceFromSand = Vector2.Distance(bottomLeft, ratioLocator.position);
            float distanceFromClay = Vector2.Distance(topCenter, ratioLocator.position);
            float distanceFromSilt = Vector2.Distance(bottomRight, ratioLocator.position);
            float quantitySand = -distanceFromSand + distanceFromClay + distanceFromSilt;
            float quantityClay = distanceFromSand - distanceFromClay + distanceFromSilt;
            float quantitySilt = distanceFromSand + distanceFromClay - distanceFromSilt;
            float quantityTotal = quantitySand + quantityClay + quantitySilt;
            int percentSand = Mathf.RoundToInt(quantitySand * 100 / quantityTotal);
            int percentClay = Mathf.RoundToInt(quantityClay * 100 / quantityTotal);
            int percentSilt = 100 - percentSand - percentClay;

            //Debug.Log(soilMix + "Sand:" + percentSand + " Clay:" + percentClay + " Silt:" + percentSilt);

            soilMixText.text = soilMix.Value
                + "\n%Sand: " + percentSand
                + "\n%Clay: " + percentClay
                + "\n%Silt: " + percentSilt;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector2 bottomLeft = new Vector2(transform.position.x - ratioLocatorReachableArea.x / 2, transform.position.y - ratioLocatorReachableArea.y / 2);
        Vector2 topCenter = bottomLeft + new Vector2(ratioLocatorReachableArea.x / 2, ratioLocatorReachableArea.y);
        Vector2 bottomRight = bottomLeft + new Vector2(ratioLocatorReachableArea.x, 0);
        Vector2 topLeft = bottomLeft + new Vector2(0, ratioLocatorReachableArea.y);
        Vector2 topRight = bottomLeft + ratioLocatorReachableArea;

        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topCenter);
        Gizmos.DrawLine(bottomRight, topCenter);
    }
}