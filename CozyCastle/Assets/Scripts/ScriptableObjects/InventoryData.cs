

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
#endif

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory")]
public class InventoryData : ScriptableObject
{
    public List<SlotData> slots = new();
    public int defaultSlotCount;
    public SlotData selectedSlot = null;

    public void InitializeSlots(int? slotCount)
    {
        slots.Clear();

        if (slotCount == null || slotCount <= 0)
        {
            Debug.LogWarning("Invalid slot count provided. Using default slot count.");
            slotCount = defaultSlotCount;
        }
        
        for (int i = 0; i < slotCount; i++)
        {
            slots.Add(new SlotData());
        }
    }

    public void AddToSlot(ItemData item)
    {
        SlotData targetSlot = null;

        // Check if there's an existing slot that can add this item   
        targetSlot = slots.FirstOrDefault(slot => slot.CanAddItem(item.itemName) && slot.itemName == item.itemName);
        if (targetSlot != null)
        {
            targetSlot.AddItem(item);
            Debug.Log($"{item.itemName} added to existing stack in inventory.");
            return;
        }

        targetSlot = slots.FirstOrDefault(slot => slot.CanAddItem(item.itemName));
        if (targetSlot != null)
        {
            targetSlot.AddItem(item);
            Debug.Log($"{item.itemName} added to inventory."); ;
        }
        else
        {
            Debug.Log($"No suitable slot found for {item.itemName}.");
        }
    }

    public void RemoveFromInventory(int index)
    {
        slots[index].RemoveItem();
        if (slots[index].count <= 0)
        {
            slots[index].SetEmpty();
            Debug.Log($"Removed item from slot {index}. Slot is now empty.");
        }
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

    public void MoveSlot(int sourceIndex, int targetIndex, InventoryData targetInventory, int amountToMove = 1)
    {
        SlotData sourceSlotData = slots[sourceIndex];
        SlotData targetSlotData = targetInventory.slots[targetIndex];

        Debug.Assert(sourceSlotData != null, $"Source slot at index {sourceIndex} is null.");
        Debug.Assert(targetSlotData != null, $"Target slot at index {targetIndex} is null.");

        for (int i = 0; i < amountToMove; i++)
        {
            if (sourceSlotData.IsEmpty())
            {
                Debug.LogWarning($"Source slot at index {sourceIndex} is empty. Cannot move item.");
                return;
            }
            if (targetSlotData.CanAddItem(sourceSlotData.itemName))
            {
                targetSlotData.AddItem(sourceSlotData.itemName, sourceSlotData.icon, sourceSlotData.isStackable);
                sourceSlotData.RemoveItem();
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

    #if UNITY_EDITOR
    [ContextMenu("Initialize Slots")]
    private void InitializeDefaultSlots() => InitializeSlots(defaultSlotCount);
    #endif
}

