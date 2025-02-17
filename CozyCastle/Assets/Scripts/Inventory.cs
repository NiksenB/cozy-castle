using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Inventory
{
    public List<Slot> slots = new ();

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
        public int max;

        public Slot()
        {
            itemName = "";
            count = 0;
            max = 100;
        }

        public bool HasRoom()
        {
            return count <= max;
        }

        public void AddItem(Item item)
        { 
            itemName = item.data.itemName;
            icon = item.data.icon;
            isStackable = item.data.isStackable;
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
    
    public void AddToInventory(Item item)
    {
        Slot targetSlot = null;

        if (item.data.isStackable)
        {
            targetSlot = slots.FirstOrDefault(slot => 
                slot.itemName == item.data.itemName && slot.HasRoom());
        
            if (targetSlot != null)
                Debug.Log("Adding to existing item stack.");
            }

        targetSlot ??= slots.FirstOrDefault(slot => string.IsNullOrEmpty(slot.itemName));

        if (targetSlot != null)
        {
            targetSlot.AddItem(item);
            Debug.Log($"{item.data.itemName} added to inventory.");;
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
}
