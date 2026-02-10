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
        Debug.LogWarning($"[PIM] Awake id={GetInstanceID()} at frame={Time.frameCount}, scene={gameObject.scene.name}");
        RegisterInventory("backpack", backpackTemplate);
        RegisterInventory("toolbar", toolbarTemplate);
        RegisterInventory("chest", chestTemplate);
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

        Debug.LogWarning($"[PIM] RegisterInventory('{inventoryName}') id={GetInstanceID()} " +
                         $"source={(loaded != null ? "SAVE" : "TEMPLATE")} nonEmptySlots={slots.FindAll(s => !s.IsEmpty()).Count}");

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
    public void MoveSlot(int sourceIndex, int targetIndex, string sourceInventoryName, string targetInventoryName, int amountToMove = 1)
    {
        List<SlotData> sourceSlots = GetInventoryByName(sourceInventoryName);
        List<SlotData> targetSlots = GetInventoryByName(targetInventoryName);
        
        Debug.LogWarning($"[PIM id={GetInstanceID()}] MoveSlot using sourceList hash={sourceSlots.GetHashCode()} targetList hash={targetSlots.GetHashCode()}");
        
        SlotData sourceSlotData = sourceSlots[sourceIndex];
        SlotData targetSlotData = targetSlots[targetIndex];

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
                targetSlotData.AddItem(sourceSlotData.itemName);
                sourceSlotData.RemoveItem();
            }
        }
        OnInventoryChanged?.Invoke(); // notify UI
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
    
    public void RaiseInventoriesReady()
    {
        OnInventoriesReady?.Invoke();
    }
    
    private void InitializeSlots(InventoryData invData, int? slotCount)
    {
        var slots = invData.slots;
        var defaultSlotCount = invData.defaultSlotCount;
        
        slots.Clear();

        if (slotCount == null || slotCount <= 0)
        {
            slotCount = defaultSlotCount;
        }
        
        for (int i = 0; i < slotCount; i++)
        {
            slots.Add(new SlotData());
        }
    }

    private void RemoveFromInventory(List<SlotData> inv, int index)
    {
        inv[index].RemoveItem();
        if (inv[index].count <= 0)
        {
            inv[index].SetEmpty();
        }
    }

    private void RemoveFromInventory(List<SlotData> inv, int index, int numToRemove)
    {
        if (inv[index].count >= numToRemove)
        {
            for (int i = 0; i < numToRemove; i++)
            {
                RemoveFromInventory(inv, index);
            }
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
            Debug.Log($"[PIM] Explicit SaveAll: saving '{k}'");
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
