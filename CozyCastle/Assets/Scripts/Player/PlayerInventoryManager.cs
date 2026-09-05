using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    [Header("Templates")]
    [SerializeField] private InventoryData backpackTemplate;
    [SerializeField] private InventoryData toolbarTemplate;
    [SerializeField] private InventoryData chestTemplate;
    
    public static event Action OnInventoriesReady;
    public static event Action OnInventoryChanged;

    private readonly Dictionary<string, List<SlotData>> _inventoryByName = new();
    public SlotData selectedSlot;

    private void Awake()
    {
        RegisterInventory("backpack", backpackTemplate);
        RegisterInventory("toolbar", toolbarTemplate);
    }

    private void Start()
    {
        // Build runtime inventories AFTER ItemManager is ready
        OnInventoriesReady?.Invoke();
    }

    public void RegisterInventory(string inventoryName, InventoryData template)
    {
        var loaded = SaveSystem.LoadChest(inventoryName);
        List<SlotData> slots = loaded ?? CloneTemplateSlots(template);
        
        Debug.Assert(slots is not null, "Failing to register inventory, " + inventoryName + " has neither template nor save data.");
        SaveSystem.RebuildSlots(slots);
        _inventoryByName[inventoryName] = slots;
    }

    private List<SlotData> CloneTemplateSlots(InventoryData template)
    {
        if (template == null)
        {
            Debug.LogError("Template is null");
            return null;
        }

        var list = new List<SlotData>();

        for (int i = 0; i < template.defaultSlotCount; i++)
            list.Add(new SlotData());

        return list;
    }

    public List<SlotData> GetInventoryByName(string inventoryName)
    {
        _inventoryByName.TryGetValue(inventoryName, out var slots);
        return slots;
    }

    public static void MoveItem(List<SlotData> sourceSlots, int sourceIndex, List<SlotData> targetSlots, int targetIndex, int amountToMove = 1)
    {
        if (sourceSlots == null || targetSlots == null)
        {
            Debug.LogWarning("MoveItem: source or target inventory is unbound. Cannot move item.");
            return;
        }
        if (sourceIndex < 0 || sourceIndex >= sourceSlots.Count || targetIndex < 0 || targetIndex >= targetSlots.Count)
        {
            Debug.LogWarning($"MoveItem: index out of range (source {sourceIndex}/{sourceSlots.Count}, target {targetIndex}/{targetSlots.Count}).");
            return;
        }

        SlotData sourceSlotData = sourceSlots[sourceIndex];
        SlotData targetSlotData = targetSlots[targetIndex];

        for (int i = 0; i < amountToMove; i++)
        {
            if (sourceSlotData.IsEmpty())
            {
                Debug.LogWarning($"Source slot at index {sourceIndex} is empty. Cannot move item.");
                return;
            }
            if (targetSlotData.CanAddItem(sourceSlotData.itemName))
            {
                targetSlotData.AddItem(sourceSlotData.itemName);
                sourceSlotData.RemoveItem();
            }
        }

        OnInventoryChanged?.Invoke();
    }
    
    public void AddItem(string inventoryName, ItemData item)
    {
        var inventory = GetInventoryByName(inventoryName);
        if (inventory != null)
        {
            AddToSlot(inventory, item); 
        }   
    }
    
    public void AddToSlot(List<SlotData> slots, ItemData item)
    {
        SlotData targetSlot = null;

        // Check if there's an existing slot that can add this item   
        targetSlot = slots.FirstOrDefault(slot => slot.CanAddItem(item.itemName) && slot.itemName == item.itemName);
        if (targetSlot != null)
        {
            targetSlot.AddItem(item);
            Debug.Log($"{item.itemName} added to existing stack in inventory.");
            OnInventoryChanged?.Invoke(); // notify UI
            return;
        }

        // Else, find an empty slot 
        targetSlot = slots.FirstOrDefault(slot => slot.CanAddItem(item.itemName));
        if (targetSlot != null)
        {
            targetSlot.AddItem(item);
            Debug.Log($"{item.itemName} added to inventory."); ;
            OnInventoryChanged?.Invoke(); // notify UI
        }
        else
        {
            Debug.Log($"No suitable slot found for {item.itemName}.");
        }
    }
    
    public void SelectSlot(List<SlotData> inv,  int index)
    {
        if (inv != null && inv.Count > 0)
        {
            selectedSlot = inv[index];
        }
    }
    
    public void SaveAll()
    {
        foreach (var (k, v) in _inventoryByName)
        {
            SaveSystem.SaveChest(k, v);
        }
    }

    public void OnDestroy()
    {
        // Safety net for any destroy path that didn't go through an explicit
        // transition (e.g. quitting, being destroyed directly). The normal
        // transition flow now calls SaveAll() proactively before this fires.
        SaveAll();
    }
}
