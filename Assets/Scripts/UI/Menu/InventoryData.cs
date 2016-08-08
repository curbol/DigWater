using System.Collections.Generic;

public class InventoryData
{
    public int InventoryId { get; set; }
    public int AvailableCurrency { get; set; }
    public List<int> ItemIds { get; set; }
}