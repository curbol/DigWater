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
    private Transform ratioPositionLocator;

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

        while (true)
        {
            yield return new WaitForEndOfFrame();

            Vector2 relativeWorldPosition = ratioPositionLocator.position - soilMixImage.transform.position;
            if (relativeWorldPosition == previousPosition)
                continue;

            previousPosition = relativeWorldPosition;
            Vector2 relativePercent = new Vector2(Mathf.Clamp01(relativeWorldPosition.x / SoilMixImageWorldDimensions.x + 0.5F), Mathf.Clamp01(relativeWorldPosition.y / SoilMixImageWorldDimensions.y + 0.5F));
            Vector2 imagePixelPosition = new Vector2((int)(SoilMixImageTexture.width * relativePercent.x), (int)(SoilMixImageTexture.height * relativePercent.y));
            Color pixelColor = SoilMixImageTexture.GetPixel((int)imagePixelPosition.x, (int)imagePixelPosition.y);
            var soilMix = soilMixes.FirstOrDefault(a => pixelColor.r < a.Key);

            Debug.Log(soilMix);

            soilMixText.text = soilMix.Value;
        }
    }
}