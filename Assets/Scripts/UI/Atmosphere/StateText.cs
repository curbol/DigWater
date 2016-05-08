using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StateText : MonoBehaviour
{
    [SerializeField]
    private DetectHydroStates detectHydroStates;

    [SerializeField]
    private Text text;
    private Text Text
    {
        get
        {
            if (text == null)
                text = gameObject.GetSafeComponent<Text>();

            return text;
        }
    }

    private void Start()
    {
        StartCoroutine(UpdateStateText());
    }

    private IEnumerator UpdateStateText()
    {
        while (true)
        {
            if (detectHydroStates == null)
                break;

            string stateText = "";

            if (detectHydroStates.FoundStates.Contains(LayerMask.NameToLayer("Evaporation")))
                stateText += "Evaporation   ";

            if (detectHydroStates.FoundStates.Contains(LayerMask.NameToLayer("Condensation")))
                stateText += "Condensation   ";

            if (detectHydroStates.FoundStates.Contains(LayerMask.NameToLayer("Percipitation")))
                stateText += "Precipitation";

            Text.text = stateText;

            yield return new WaitForEndOfFrame();
        }
    }
}