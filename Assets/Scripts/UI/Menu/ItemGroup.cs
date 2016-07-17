using UnityEngine;
using UnityEngine.UI;

public class ItemGroup : MonoBehaviour
{
    [SerializeField]
    private Text titleText;
    public string Title
    {
        get
        {
            return titleText.text;
        }

        set
        {
            titleText.text = value;
        }
    }

    [SerializeField]
    private Transform itemSlotContainer;
    public Transform ItemSlotContainer
    {
        get
        {
            return itemSlotContainer;
        }
    }
}