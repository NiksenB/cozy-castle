using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public string chestId;

    private List<SlotData> slots;

    private void Start()
    {
        slots = SaveSystem.LoadChest(chestId);
        if (slots == null)
        {
            slots = new List<SlotData>();
            for (int i = 0; i < 27; i++)
                slots.Add(new SlotData());
        }
        else
        {
            SaveSystem.RebuildSlots(slots);
        }

        // Live-save: persist the moment any slot in this chest changes,
        // instead of relying on OnDestroy timing.
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

        Inventory_UI chestUI = uiManager.GetInventoryUI("chest");
        chestUI.Bind(slots);
        uiManager.currentChest = this; // track which chest is open, for save-on-close

        uiManager.ToggleChestInventory();
    }
    
    private void OnDestroy()
    {
        foreach (var slot in slots)
            slot.OnSlotChanged -= HandleSlotChanged;

        SaveState(); // safety net, now redundant in the common case but harmless
    }
}