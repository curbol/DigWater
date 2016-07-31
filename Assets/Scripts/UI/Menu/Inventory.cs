using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private ItemDescriptor itemDescriptor;
    private ItemDescriptor ItemDescriptor
    {
        get
        {
            if (itemDescriptor == null)
                itemDescriptor = FindObjectOfType<ItemDescriptor>();

            return itemDescriptor;
        }
    }

    [SerializeField]
    private int inventoryID;

    [SerializeField]
    private ItemSlot itemSlotPrefab;

    [SerializeField]
    private ItemGroup itemGroupPrefab;

    [SerializeField]
    private Transform itemGroupsPanel;
    private Transform ItemGroupsPanel
    {
        get
        {
            if (itemGroupsPanel == null)
                itemGroupsPanel = transform.FindChild("Item Groups");

            return itemGroupsPanel;
        }
    }

    private Dictionary<int, ItemGroup> ItemGroups { get; set; }

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

            if (selectedItemSlot != null && ItemDescriptor != null)
                ItemDescriptor.Item = selectedItemSlot.Item;
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

    public void AddItem(int itemId)
    {
        AddItem(ItemManager.GetItem(itemId));
    }

    //public bool AddItem(int itemId)
    //{
    //    return AddItem(ItemManager.GetItem(itemId));
    //}

    public bool AddItem(InventoryItem item)
    {
        if (item == null)
            return false;

        if (!ItemGroups.ContainsKey(item.Group))
        {
            string groupTitle = ItemManager.GetGroupTitle(item.Group);
            ItemGroups[item.Group] = BuildNewItemGroup(ItemGroupsPanel, groupTitle);
        }

        ItemSlot itemSlot = BuildNewItemSlot(ItemGroups[item.Group].ItemSlotContainer, item);
        ItemSlots.Add(itemSlot);

        return true;
    }

    public bool RemoveItem(int itemId)
    {
        return RemoveItem(ItemManager.GetItem(itemId));
    }

    public bool RemoveItem(InventoryItem item)
    {
        ItemSlot itemSlot = ItemSlots.FirstOrDefault(a => a.Item.Id == item.Id);
        if (itemSlot == null)
            return false;

        ItemSlots.Remove(itemSlot);
        Destroy(itemSlot);

        if (!ItemSlots.Any(a => a.Item.Group == item.Group))
        {
            ItemGroup itemGroupToRemove = ItemGroups[item.Group];
            ItemGroups.Remove(item.Group);
            Destroy(itemGroupToRemove);
        }

        return true;
    }

    private void Awake()
    {
        ItemGroups = new Dictionary<int, ItemGroup>();
        ItemSlots = new List<ItemSlot>();
    }

    private void Start()
    {
        foreach (InventoryItem item in ItemManager.GetInventoryItems(inventoryID))
            AddItem(item);

        SelectedItemSlot = ItemSlots.FirstOrDefault();

        ItemDescriptor.OnPurchase += () => AddItem(ItemDescriptor.Item);
    }

    private ItemGroup BuildNewItemGroup(Transform parent, string title)
    {
        ItemGroup itemGroup = parent.InstantiateChild(itemGroupPrefab);
        itemGroup.Title = title;

        return itemGroup;
    }

    private ItemSlot BuildNewItemSlot(Transform parent)
    {
        ItemSlot itemSlot = parent.InstantiateChild(itemSlotPrefab);
        itemSlot.GetComponent<Button>().onClick.AddListener(() => SelectedItemSlot = itemSlot);

        return itemSlot;
    }

    private ItemSlot BuildNewItemSlot(Transform parent, InventoryItem item)
    {
        ItemSlot itemSlot = BuildNewItemSlot(parent);
        itemSlot.Item = item;

        return itemSlot;
    }
}