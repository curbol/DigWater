using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private ItemDescriptor itemDescriptor;

    [SerializeField]
    private int inventoryID;

    [SerializeField]
    private Transform slotPanel;
    private Transform SlotPanel
    {
        get
        {
            if (slotPanel == null)
                slotPanel = transform.FindChild("Slot Panel");

            return slotPanel;
        }
    }

    [SerializeField]
    private ItemSlot itemSlotPrefab;

    [SerializeField]
    private int slotCount = 36;

    private List<ItemSlot> ItemSlots { get; set; }

    private ItemSlot selectedItemSlot;
    public ItemSlot SelectedItemSlot
    {
        get
        {
            return selectedItemSlot;
        }

        private set
        {
            selectedItemSlot = value;

            if (selectedItemSlot != null && itemDescriptor != null)
                itemDescriptor.Item = selectedItemSlot.Item;
        }
    }

    private float availableCurrency;
    public float AvailableCurrency
    {
        get
        {
            return availableCurrency;
        }

        set
        {
            availableCurrency = Mathf.Max(value, 0);
        }
    }

    public bool CanBuyItem(InventoryItem item)
    {
        return item.Value <= AvailableCurrency;
    }

    public bool BuyItem(InventoryItem item)
    {
        if (!CanBuyItem(item))
            return false;

        AvailableCurrency -= item.Value;
        AddItem(item);

        return true;
    }

    public bool AddItem(int itemId)
    {
        return AddItem(ItemManager.GetItem(itemId));
    }

    public bool AddItem(InventoryItem item)
    {
        ItemSlot emptyItemSlot = ItemSlots.FirstOrDefault(a => a.Item == null);
        if (emptyItemSlot == null)
            return false;

        emptyItemSlot.Item = item;
        return true;
    }

    private void Start()
    {
        ItemSlots = BuildInventorySlots(SlotPanel, itemSlotPrefab, slotCount);
        IEnumerable<InventoryItem> items = ItemManager.GetInventoryItems(inventoryID);

        AddItemsToSlots(ItemSlots, items);
        SelectedItemSlot = ItemSlots.FirstOrDefault();
    }

    private List<ItemSlot> BuildInventorySlots(Transform slotPanel, ItemSlot itemSlotPrefab, int slotCount)
    {
        List<ItemSlot> itemSlots = new List<ItemSlot>();

        for (int i = 0; i < slotCount; i++)
        {
            ItemSlot itemSlot = (ItemSlot)Instantiate(itemSlotPrefab, slotPanel.position, slotPanel.rotation);
            itemSlot.transform.SetParent(slotPanel);
            itemSlot.transform.localScale = Vector3.one;
            itemSlot.GetComponent<Button>().onClick.AddListener(() => SelectedItemSlot = itemSlot);

            itemSlots.Add(itemSlot);
        }

        return itemSlots;
    }

    private static void AddItemsToSlots(IEnumerable<ItemSlot> itemSlots, IEnumerable<InventoryItem> items)
    {
        int i = 0;
        foreach (ItemSlot itemSlot in itemSlots)
        {
            InventoryItem item = items.ElementAtOrDefault(i);
            if (item == null)
                break;

            itemSlot.Item = item;
            i++;
        }
    }
}