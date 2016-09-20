using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public const int PlayerInventoryId = 0;

    public static Action<InventoryItem> OnPurchaseItem { get; set; }
    public static Action<AddItemToInventoryResponse> OnAddItemToInventory { get; set; }

    private Dictionary<int, InventoryItem> items;
    public static Dictionary<int, InventoryItem> Items
    {
        get
        {
            if (Instance.items == null)
                Instance.items = LoadItems();

            return Instance.items;
        }
    }

    private Dictionary<int, InventoryData> inventories;
    public static Dictionary<int, InventoryData> Inventories
    {
        get
        {
            if (Instance.inventories == null)
                Instance.inventories = LoadInventories();

            return Instance.inventories;
        }
    }

    private Dictionary<int, string> groups;
    public static Dictionary<int, string> Groups
    {
        get
        {
            if (Instance.groups == null)
                Instance.groups = LoadGroups();

            return Instance.groups;
        }
    }

    public static InventoryItem GetItem(int itemId)
    {
        return Items.FirstOrDefault(a => a.Key == itemId).Value;
    }

    public static InventoryData GetInventory(int inventoryId)
    {
        return Inventories.FirstOrDefault(a => a.Key == inventoryId).Value;
    }

    public static string GetGroupTitle(int groupId)
    {
        return Groups.FirstOrDefault(a => a.Key == groupId).Value;
    }

    public static IEnumerable<InventoryItem> GetInventoryItems(int inventoryId)
    {
        if (Inventories.ContainsKey(inventoryId))
        {
            IEnumerable<int> itemIds = Inventories[inventoryId].ItemIds;
            foreach (int itemId in itemIds)
            {
                InventoryItem item = Items.FirstOrDefault(a => a.Key == itemId).Value;

                if (item != null)
                    yield return item;
            }
        }
    }

    public static bool CanPurchaseItem(int inventoryId, int itemId)
    {
        if (Items.ContainsKey(itemId) && Inventories.ContainsKey(PlayerInventoryId))
        {
            return Inventories[inventoryId].AvailableCurrency >= Items[itemId].Value;
        }

        return false;
    }

    public static void PurchaseItem(int itemId)
    {
        PurchaseItem(itemId, PlayerInventoryId);
    }

    public static void PurchaseItem(int itemId, int inventoryId)
    {
        if (CanPurchaseItem(PlayerInventoryId, itemId) && Items.ContainsKey(itemId) && Inventories.ContainsKey(PlayerInventoryId))
        {
            Inventories[PlayerInventoryId].ItemIds.Add(itemId);
            Inventories[PlayerInventoryId].AvailableCurrency -= Items[itemId].Value;

            if (OnAddItemToInventory != null)
            {
                OnPurchaseItem(Items[itemId]);
            }
        }
    }

    public static void AddItemToInventory(int itemId, int inventoryId)
    {
        if (Items.ContainsKey(itemId) && Inventories.ContainsKey(inventoryId))
        {
            Inventories[inventoryId].ItemIds.Add(itemId);

            if (OnAddItemToInventory != null)
            {
                OnAddItemToInventory(new AddItemToInventoryResponse
                {
                    ItemId = itemId,
                    InventoryId = inventoryId
                });
            }
        }
    }

    private static Dictionary<int, InventoryData> LoadInventories()
    {
        Dictionary<int, InventoryData> loadedInventories = new Dictionary<int, InventoryData>();

        JsonData inventoriesData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Inventories.json"));
        for (int i = 0; i < inventoriesData.Count; i++)
        {
            int inventoryId = (int)inventoriesData[i]["inventoryId"];

            loadedInventories[inventoryId] = new InventoryData
            {
                InventoryId = inventoryId,
                AvailableCurrency = (int)inventoriesData[i]["availableCurrency"],
                ItemIds = GetIntArrayFromJsonData(inventoriesData[i]["itemIds"]).ToList()
            };
        }

        return loadedInventories;
    }

    private static Dictionary<int, string> LoadGroups()
    {
        Dictionary<int, string> loadedGroups = new Dictionary<int, string>();

        JsonData groupsData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Groups.json"));
        for (int i = 0; i < groupsData.Count; i++)
        {
            int groupId = (int)groupsData[i]["groupId"];
            string title = (string)groupsData[i]["title"];

            loadedGroups[groupId] = title;
        }

        return loadedGroups;
    }

    private static Dictionary<int, InventoryItem> LoadItems()
    {
        Dictionary<int, InventoryItem> loadedItems = new Dictionary<int, InventoryItem>();

        JsonData itemsData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        for (int i = 0; i < itemsData.Count; i++)
        {
            int itemId = (int)itemsData[i]["id"];
            loadedItems[itemId] = new InventoryItem
            {
                Id = (int)itemsData[i]["id"],
                Group = (int)itemsData[i]["group"],
                Value = (int)itemsData[i]["value"],
                Name = (string)itemsData[i]["name"],
                Description = (string)itemsData[i]["description"],
                Slug = (string)itemsData[i]["slug"]
            };
        }

        return loadedItems;
    }

    private static IEnumerable<int> GetIntArrayFromJsonData(JsonData jsonData)
    {
        for (int i = 0; i < jsonData.Count; i++)
        {
            yield return (int)jsonData[i];
        }
    }

    private static void SaveInventories(IEnumerable<InventoryItem> items)
    {
    }

    private static void SaveItems(IEnumerable<InventoryItem> items)
    {
    }

    private void Awake()
    {
        LoadItems();
    }
}