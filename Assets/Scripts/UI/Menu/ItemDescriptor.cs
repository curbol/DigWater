using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptor : MonoBehaviour
{
    public Action OnPurchase { get; set; }

    [SerializeField]
    private Text titleText;

    [SerializeField]
    private Text descriptionText;

    [SerializeField]
    private Text valueText;

    [SerializeField]
    private Image itemImage;
    public Image ItemImage
    {
        get
        {
            return itemImage;
        }

        set
        {
            itemImage = value;

            float alpha = itemImage != null ? 1 : 0;
            itemImage.color = new Color(itemImage.color.r, itemImage.color.g, itemImage.color.b, alpha);
        }
    }

    [SerializeField]
    private Button purchaseButton;
    private Button PurchaseButton
    {
        get
        {
            if (purchaseButton == null)
                purchaseButton = FindObjectOfType<Button>();

            return purchaseButton;
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

            titleText.text = item != null ? item.Name : null;
            descriptionText.text = item != null ? item.Description : null;
            valueText.text = "Price: " + (item != null ? "$" + item.Value : null);
            ItemImage.sprite = item != null ? item.Sprite : null;
            itemImage.color = new Color(itemImage.color.r, itemImage.color.g, itemImage.color.b, item != null && item.Sprite != null ? 1 : 0);
        }
    }

    private void Awake()
    {
        Item = null;
    }

    private void Start()
    {
        PurchaseButton.onClick.AddListener(() => Purchase());
    }

    public void Purchase()
    {
        if (OnPurchase != null)
            OnPurchase();
    }
}