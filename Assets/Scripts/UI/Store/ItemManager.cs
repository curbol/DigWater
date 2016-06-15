using LitJson;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    private IEnumerable<InventoryItem> items;
    private static IEnumerable<InventoryItem> Items
    {
        get
        {
            if (Instance.items == null)
                Instance.items = LoadItems();

            return Instance.items;
        }
    }

    private Dictionary<int, IEnumerable<int>> inventories;
    public static Dictionary<int, IEnumerable<int>> Inventories
    {
        get
        {
            if (Instance.inventories == null)
                Instance.inventories = LoadInventories();

            return Instance.inventories;
        }
    }

    public static InventoryItem GetItem(int itemId)
    {
        return Items.FirstOrDefault(a => a.Id == itemId);
    }

    public static IEnumerable<InventoryItem> GetInventoryItems(int inventoryId)
    {
        if (Inventories.ContainsKey(inventoryId))
        {
            IEnumerable<int> itemIds = Inventories[inventoryId];
            foreach (int itemId in itemIds)
            {
                InventoryItem item = Items.FirstOrDefault(a => a.Id == itemId);

                if (item != null)
                    yield return item;
            }
        }
    }

    private static Dictionary<int, IEnumerable<int>> LoadInventories()
    {
        Dictionary<int, IEnumerable<int>> loadedInventories = new Dictionary<int, IEnumerable<int>>();

        JsonData inventoriesData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Inventories.json"));
        for (int i = 0; i < inventoriesData.Count; i++)
        {
            int inventoryId = (int)inventoriesData[i]["inventoryId"];
            IEnumerable<int> itemIds = GetIntArrayFromJsonData(inventoriesData[i]["itemIds"]);

            loadedInventories[inventoryId] = itemIds;
        }

        return loadedInventories;
    }

    private static IEnumerable<InventoryItem> LoadItems()
    {
        List<InventoryItem> loadedItems = new List<InventoryItem>();

        JsonData itemsData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        for (int i = 0; i < itemsData.Count; i++)
        {
            loadedItems.Add(new InventoryItem
            {
                Id = (int)itemsData[i]["id"],
                Value = (int)itemsData[i]["value"],
                Name = (string)itemsData[i]["name"],
                Description = (string)itemsData[i]["description"],
                Slug = (string)itemsData[i]["slug"]
            });
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