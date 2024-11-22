using System.Collections.Generic;
using UnityEngine;

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
        public CollectableType type;
        public Sprite icon;
        public bool isStackable;
        public int count;
        public int max;

        public Slot()
        {
            type = CollectableType.NONE;
            count = 0;
            max = 100;
        }

        public bool HasRoom()
        {
            return count <= max;
        }

        public void AddItem(Collectable item)
        { 
            type = item.type;
            icon = item.icon;
            isStackable = item.isStackable;
            count++;
        }

        public void RemoveItem()
        { 
            if (count > 0)
            {
                count--;

                if (count == 0)
                {
                    type = CollectableType.NONE;
                    icon = null;
                    isStackable = false;
                }
            }
        }
    }
    
    public void AddToInventory(Collectable item)
    {
        Slot s = null;
        if (item.isStackable)
        {
            Debug.Log("Attempt add to item stack.");
            foreach (Slot slot in slots)
            {   
                if (slot.type == item.type && slot.HasRoom())
                {
                    s = slot;
                    break;
                }
            }
        }

        if (s is null)
        {
            foreach (Slot slot in slots)
            {
                if (slot.type == CollectableType.NONE)
                {
                    s = slot;
                    break;
                }
            }
        }

        if (s is not null)
        {
            Debug.Log(item.type + " added to inventory.");
            s.AddItem(item);
        }
        return;
    }

    public void RemoveFromInventory(int index) 
    { 
        slots[index].RemoveItem();
    }
}
