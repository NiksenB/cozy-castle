using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour 
{
    public InventoryData backpack;
    public InventoryData toolbar;
    public Dictionary<string, InventoryData> inventoryByName = new();

    public void Awake()
    {
        inventoryByName.Clear();

        // try to find scriptable object in resources 
        if (backpack == null)
            backpack = Resources.Load<InventoryData>("Inventory/Backpack");

        if (toolbar == null)
            toolbar = Resources.Load<InventoryData>("Inventory/Toolbar");
            
        if (backpack != null) inventoryByName.Add("backpack", backpack);
        if (toolbar != null) inventoryByName.Add("toolbar", toolbar);    
    }

    public InventoryData GetInventoryByName(string inventoryName)
    {
        inventoryByName.TryGetValue(inventoryName, out var inventory);
        return inventory;
    }

    public void AddItem(string inventoryName, ItemData item)
    {
        var inventory = GetInventoryByName(inventoryName);
        if (inventory != null)
        {
            inventory.AddToSlot(item); 
        }   
    }
}
