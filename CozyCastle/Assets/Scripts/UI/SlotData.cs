using UnityEngine;
using System;

public class SlotData 
{

    public string itemName;
    public Sprite icon;
    public bool isStackable;
    public int count;
    public int maxStackSize;
    public event Action OnSlotChanged;

    public SlotData()
    {
        itemName = "";
        icon = null;
        isStackable = true;
        count = 0;
        maxStackSize = 100;
    }
    
    public bool IsEmpty()
    {
        return itemName == "" && count == 0;
    }

    public bool CanAddItem(string itemName)
    {
        return IsEmpty() || (isStackable && this.itemName == itemName && count <= maxStackSize);
    }

    public void AddItem(ItemData item)
    {
        itemName = item.itemName;
        icon = item.icon;
        isStackable = item.isStackable;
        count++;
        OnSlotChanged?.Invoke();
    }

    public void AddItem(string itemName, Sprite icon, bool isStackable)
    {
        this.itemName = itemName;
        this.icon = icon;
        this.isStackable = isStackable;
        count++;
        OnSlotChanged?.Invoke();
    }

    public void RemoveItem()
    {
        if (count > 0)
        {
            count--;

            if (count == 0)
            {
                itemName = "";
                icon = null;
                isStackable = false;
            }
        }
        OnSlotChanged?.Invoke();
    }
    
    public void SetEmpty()
    {
        itemName = "";
        icon = null;
        isStackable = false;
        count = 0;
        OnSlotChanged?.Invoke();
    }
}