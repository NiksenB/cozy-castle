using UnityEngine;
using System;

[Serializable]
public class SlotData 
{

    public string itemName;
    public int count;
    [NonSerialized] public bool IsStackable;
    [NonSerialized] public Sprite Icon;
    [NonSerialized] public int MaxStackSize;
    public event Action OnSlotChanged;

    public SlotData()
    {
        itemName = "";
        Icon = null;
        IsStackable = true;
        count = 0;
        MaxStackSize = 100;
    }
    
    public SlotData(string itemName, Sprite icon, bool isStackable, int count, int maxStackSize)
    {
        this.itemName = itemName;
        Icon = icon;
        IsStackable = isStackable;
        this.count = count;
        MaxStackSize = maxStackSize;
    }
    
    public bool IsEmpty()
    {
        return itemName == "" && count == 0;
    }

    public bool CanAddItem(string itemName)
    {
        return IsEmpty() || (IsStackable && this.itemName == itemName && count < MaxStackSize);
    }

    public void AddItem(ItemData item)
    {
        itemName = item.itemName;
        Icon = item.icon;
        IsStackable = item.isStackable;
        MaxStackSize = item.maxStackSize;
        count++;
        OnSlotChanged?.Invoke();
    }

    public void AddItem(string name)
    {
        if (ItemManager.instance.itemDict.TryGetValue(name, out var item))
        {
            AddItem(item.data);
        }
        else { Debug.LogError($"Item {name} not found."); }
    }

    public void RemoveItem()
    {
        if (count > 0)
        {
            count--;

            if (count == 0)
            {
                itemName = "";
                Icon = null;
                IsStackable = false;
            }
        }
        OnSlotChanged?.Invoke();
    }
    
    public void SetEmpty()
    {
        itemName = "";
        Icon = null;
        IsStackable = false;
        count = 0;
        OnSlotChanged?.Invoke();
    }
    
    public void RebuildFromItemDatabase()
    {
        if (string.IsNullOrEmpty(itemName))
            return;

        var item = ItemManager.instance.itemDict[itemName];

        Debug.Assert(item != null, "Item " + itemName + " not found in item dictionary!");
        
        Icon = item.data.icon;
        IsStackable = item.data.isStackable;
        MaxStackSize = item.data.maxStackSize;

        OnSlotChanged?.Invoke();
    }

}