using UnityEngine;
using UnityEngine.UI;

public class PlayerAvailableCurrencyTextUpdater : MonoBehaviour
{
    [SerializeField]
    private Text playerAvailableCurrencyText;
    private Text PlayerAvailableCurrencyText
    {
        get
        {
            if (playerAvailableCurrencyText == null)
                playerAvailableCurrencyText = GetComponent<Text>();

            return playerAvailableCurrencyText;
        }
    }

    private void Start()
    {
        ItemManager.OnPurchaseItem += (InventoryItem item) => UpdatePlayerAvailableCurrency();

        UpdatePlayerAvailableCurrency();
    }

    private void UpdatePlayerAvailableCurrency()
    {
        int availableCurrency = ItemManager.GetInventory(ItemManager.PlayerInventoryId).AvailableCurrency;
        PlayerAvailableCurrencyText.text = "$" + availableCurrency;
    }
}