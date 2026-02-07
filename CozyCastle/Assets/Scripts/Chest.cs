using System;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public string chestId;

    private List<SlotData> slots;

    
    private void Awake()
    {
        slots = SaveSystem.LoadChest(chestId)
                ?? Resources.Load<InventoryData>("Chests/" + chestId).slots;
    }
    
    public void Interact(GameObject player)
    {
        UI_Manager uiManager  = GameManager.gameInstance.GetComponent<UI_Manager>();
        
        InventoryData inventoryUI =
            GameManager.gameInstance.player.playerInventoryManager.GetInventoryByName("chest");
        
        inventoryUI.PopulateInventory(slots);
        
        uiManager.ToggleChestInventory();
        SaveSystem.SaveChest(chestId, slots);
    }

    public void OnDestroy()
    {
        SaveSystem.SaveChest(chestId, slots);
    }
}