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
        Slider.minValue = HydroManager.GetProperties<HeatProperties>().MinimumAmbientTemperatureChange;
        Slider.maxValue = HydroManager.GetProperties<HeatProperties>().MaximumAmbientTemperatureChange;
        Slider.value = HydroManager.GetProperties<HeatProperties>().CurrentAmbientTemperatureChange;
    }
}