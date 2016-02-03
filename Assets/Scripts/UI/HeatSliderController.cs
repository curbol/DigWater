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

    private void Start()
    {
        float startingValue = PlayerPrefs.GetFloat("HeatLevel", HydroManager.HeatProperties.AmbientTemperatureChange);
        Slider.value = startingValue;
    }
}