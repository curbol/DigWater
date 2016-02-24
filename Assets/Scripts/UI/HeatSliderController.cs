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
        Slider.minValue = HydroManager.Heat.MinimumAmbientTemperatureChange;
        Slider.maxValue = HydroManager.Heat.MaximumAmbientTemperatureChange;
        Slider.value = HydroManager.Heat.CurrentAmbientTemperatureChange;
    }
}