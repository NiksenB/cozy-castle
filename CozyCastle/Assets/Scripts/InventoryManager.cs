using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class InventoryManager : MonoBehaviour 
{
    public Dictionary<string, Inventory> inventoryByName = new();

    [Header("Backpack")]
    public Inventory backpack;
    public int backpackSlotsCount;
    [Header("Toolbar")]
    public Inventory toolbar;
    public int toolbarSlotsCount;

    public void Awake()
    {
        backpack = new Inventory(backpackSlotsCount);
        toolbar = new Inventory(toolbarSlotsCount);

        inventoryByName.Add("backpack", backpack);
        inventoryByName.Add("toolbar", toolbar);
    }

    public Inventory GetInventoryByName(string inventoryName)
    {
        if (inventoryByName.ContainsKey(inventoryName))
        {
            return inventoryByName[inventoryName];
        }        
        return null;
    }

    public void Add(string inventoryName, Item item)
    {
        if (inventoryByName.ContainsKey(inventoryName))
        {
            inventoryByName[inventoryName].AddToInventoryAutomatic(item);
        }        
    }
}
