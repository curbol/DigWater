using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private Sprite defaultSprite;

    [SerializeField]
    private Image itemImage;
    private Image ItemImage
    {
        get
        {
            if (itemImage == null)
                itemImage = GetComponentInChildren<Image>();

            return itemImage;
        }
    }

    private InventoryItem item;
    public InventoryItem Item
    {
        get
        {
            return item;
        }

        set
        {
            item = value;

            ItemImage.sprite = item != null ? item.Sprite : defaultSprite;
            name = "Slot" + (item != null ? " - " + item.Description : "");
        }
    }
}