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
        Slider.minValue = 0;
        Slider.maxValue = 1;
        Slider.value = HeatManager.HeatSlider;
    }
}