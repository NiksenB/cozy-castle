using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public string chestId;

    private List<SlotData> slots;

    private void Start()
    {
        const int expectedSlotCount = 27; // match the UI
        
        slots = SaveSystem.LoadChest(chestId);
        
        if (slots == null || slots.Count != expectedSlotCount)
        {
            if (slots != null)
                Debug.LogWarning($"[Chest:{chestId}] Saved slot count ({slots.Count}) doesn't match expected ({expectedSlotCount}). Resetting to empty.");
            
            slots = new List<SlotData>();
            for (int i = 0; i < 27; i++)
                slots.Add(new SlotData());
        }
        else
        {
            SaveSystem.RebuildSlots(slots);
        }

        // Live-save: persist the moment any slot in this chest changes.
        foreach (var slot in slots)
            slot.OnSlotChanged += HandleSlotChanged;
    }

    private void HandleSlotChanged()
    {
        SaveState();
    }

    public void SaveState()
    {
        if (slots == null) return;
        Debug.Log($"[Chest:{chestId}] Live-saving, non-empty slots: " +
                  slots.FindAll(s => !s.IsEmpty()).Count);
        SaveSystem.SaveChest(chestId, slots);
    }

    public void Interact(GameObject player)
    {
        UI_Manager uiManager = UI_Manager.instance;
        if (uiManager == null) { Debug.Log("UI Manager is null"); return; }

        SaveSystem.RebuildSlots(slots);

        // Activate the panel FIRST so the slot GameObjects are live in the
        // hierarchy before we bind — SetItem/SetEmpty no-op on inactive objects,
        // which previously left stale placeholder sprites on empty slots.
        uiManager.ToggleChestInventory();

        Inventory_UI chestUI = uiManager.GetInventoryUI("chest");
        chestUI.Bind(slots);
    }
    
    private void OnDestroy()
    {
        foreach (var slot in slots)
            slot.OnSlotChanged -= HandleSlotChanged;

        SaveState(); // safety net
    }
}