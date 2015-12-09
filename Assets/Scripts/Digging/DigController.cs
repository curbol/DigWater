using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SoilMapController))]
public class DigController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem digEffect;

    [SerializeField]
    private float digRadius;

    private SoilMapController soilMapController;

    public ParticleSystem DigEffect
    {
        get
        {
            return digEffect;
        }

        set
        {
            digEffect = value;
        }
    }

    public float DigRadius
    {
        get
        {
            return digRadius;
        }

        set
        {
            digRadius = value;
        }
    }

    private SoilMap SoilMap
    {
        get
        {
            return SoilMapController != null ? SoilMapController.SoilMap : null;
        }
    }

    private SoilMapController SoilMapController
    {
        get
        {
            if (soilMapController == null)
            {
                soilMapController = GetComponent<SoilMapController>();
            }

            return soilMapController;
        }
    }

    private void PlayDigEffect(Vector2 screenPosition)
    {
        if (DigEffect == null)
            return;

        DigEffect.Stop();
        DigEffect.transform.position = screenPosition;
        DigEffect.GetComponent<ParticleSystem>().Play();
    }

    private void Start()
    {
        DigEffect = Instantiate(DigEffect, Vector2.zero, Quaternion.identity) as ParticleSystem;
        DigEffect.transform.parent = transform;
    }

    private void Update()
    {
        bool leftMouseClicked = Input.GetMouseButton((int)MouseButton.Left);
        bool rightMouseClicked = Input.GetMouseButton((int)MouseButton.Right);

        if (SoilMap != null && (leftMouseClicked || rightMouseClicked))
        {
            SoilType newSoilType = leftMouseClicked ? SoilType.None : SoilType.Dirt;
            Vector2 screenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Coordinate coordinateToDig = SoilMap.GetCoordinateFromPosition(screenPosition);

            if (SoilMap[coordinateToDig.X, coordinateToDig.Y] != newSoilType)
            {
                if (leftMouseClicked)
                {
                    PlayDigEffect(screenPosition);
                }

                SoilMap.Draw(coordinateToDig, newSoilType, DigRadius);

                SoilMapController.RedrawSoilMesh();
            }
        }
    }
}