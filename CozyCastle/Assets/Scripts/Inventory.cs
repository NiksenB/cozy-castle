using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory;

[System.Serializable]
public class Inventory
{
    public List<Slot> slots = new List<Slot>();

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
        public int count;
        public int max;

        public Slot()
        {
            type = CollectableType.NONE;
            count = 0;
            max = 3;
        }

        public bool HasRoom()
        {
            return count >= max;
        }

        public void AddItem(CollectableType type)
        { 
            this.type = type;
            count++;
        }
    }
    
    public void Add(CollectableType type, bool isStackable)
    {
        Slot s = null;
        if (isStackable)
        {
            foreach (Slot slot in slots)
            {
                if (slot.type == type && slot.HasRoom())
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
            Debug.Log(type + " added to inventory.");
            s.AddItem(type);
        }
        return;
    }
}
