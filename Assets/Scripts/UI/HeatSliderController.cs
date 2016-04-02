using UnityEngine;
using UnityEngine.UI;

public class HeatSliderController : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    public Slider Slider
    {
        get
        {
            if (slider == null)
            {
                slider = gameObject.GetSafeComponent<Slider>();
            }

            return slider;
        }
    }

    private void Awake()
    {
        Slider.minValue = -1;
        Slider.maxValue = 0;
        Slider.value = HeatManager.AmbientHeatRate;
    }
}