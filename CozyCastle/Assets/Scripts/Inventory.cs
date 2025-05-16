using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Inventory
{
    public List<Slot> slots = new();
    public Slot selectedSlot = null;

    public Inventory(int numSlots)
    {
        for (int i = 0; i < numSlots; i++)
        {
            Slot slot = new();
            slots.Add(slot);
        }
    }

    [System.Serializable]
    public class Slot
    {
        public string itemName;
        public Sprite icon;
        public bool isStackable;
        public int count;
        public int max = 100;

        public Slot()
        {
            itemName = "";
            count = 0;
        }

        public bool IsEmpty()
        {
            return itemName == "" && count == 0;
        }

        public bool CanAddItem(string itemName)
        {
            return IsEmpty() || (isStackable && this.itemName == itemName && count <= max);
        }

        public void AddItem(Item item)
        {
            itemName = item.data.itemName;
            icon = item.data.icon;
            isStackable = item.data.isStackable;
            count++;
        }

        public void AddItem(string itemName, Sprite icon, bool isStackable)
        {
            this.itemName = itemName;
            this.icon = icon;
            this.isStackable = isStackable;
            count++;
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
        }
    }

    public void AddToInventoryAutomatic(Item item)
    {
        Slot targetSlot = null;

        targetSlot = slots.FirstOrDefault(slot => slot.CanAddItem(item.data.itemName));

        if (targetSlot != null)
        {
            targetSlot.AddItem(item);
            Debug.Log($"{item.data.itemName} added to inventory."); ;
        }
        else
        {
            Debug.Log($"No suitable slot found for {item.data.itemName}.");
        }
    }

    public void RemoveFromInventory(int index)
    {
        slots[index].RemoveItem();
    }

    public void RemoveFromInventory(int index, int numToRemove)
    {
        if (slots[index].count >= numToRemove)
        {
            for (int i = 0; i < numToRemove; i++)
            {
                RemoveFromInventory(index);
            }
        }
    }

    public void MoveSlot(int fromIndex, int toIndex, Inventory toInvevntory, int amountToMove = 1)
    {
        Slot fromSlot = slots[fromIndex];
        Slot toSlot = toInvevntory.slots[toIndex];

        for (int i = 0; i < amountToMove; i++)
        {
            if (toSlot.CanAddItem(fromSlot.itemName))
            {
                toSlot.AddItem(fromSlot.itemName, fromSlot.icon, fromSlot.isStackable);
                fromSlot.RemoveItem();
            }
        }
    }

    public void SelectSlot(int index)
    {
        if (slots != null && slots.Count > 0)
        { 
            selectedSlot = slots[index];
        }
    }
}
