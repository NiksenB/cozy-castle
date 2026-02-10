using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Toolbar_UI : MonoBehaviour
{
    public List<Slot_UI> toolbarSlots;
    private Slot_UI selectedSlot;
    private int selectedSlotIndex;

    private void OnEnable()
    {
        PlayerInventoryManager.OnInventoriesReady += OnInventoriesReady;
        GameManager.OnPlayerRegistered += OnPlayerRegistered;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        PlayerInventoryManager.OnInventoriesReady -= OnInventoriesReady;
        GameManager.OnPlayerRegistered -= OnPlayerRegistered;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Canvas" || scene.name == "Bootstrap")
            return;

        SelectSlot(selectedSlotIndex);
    }
    
    private void OnPlayerRegistered(Player player)
    {
        // A new Player just registered (e.g. after a scene transition).
        // Re-resolve the toolbar selection against the live player/inventory
        // instead of waiting on OnEnable/OnInventoriesReady timing to line up.
        SelectSlot(selectedSlotIndex);
    }

    private void OnInventoriesReady()
    {
        SelectSlot(0);
    }
    private void Update()
    {
        checkAlphaNumericKeys();
        checkScroll();
    }

    public void SelectSlot(Slot_UI slot)
    { 
        SelectSlot(slot.slotID);
    }

    public void SelectSlot(int index)
    {
        if (GameManager.gameInstance == null) return;
        var player = GameManager.gameInstance.Player;
        if (player == null) return;
        var inv = player.playerInventoryManager;
        if (inv == null) return;

        if (toolbarSlots.Count == 9 && index >= 0 && index <= 8)
        {
            if (selectedSlot != null)
                selectedSlot.SetHighlight(false);

            selectedSlot = toolbarSlots[index];
            selectedSlot.SetHighlight(true);
            selectedSlotIndex = index;

            List<SlotData> toolbar = inv.GetInventoryByName("toolbar");
            inv.SelectSlot(toolbar, index);
        }
    }

    private void checkAlphaNumericKeys()
    {
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectSlot(i);
            }
        }
    }

    private void checkScroll()
    {
        if (Input.mouseScrollDelta.y < 0)
            SelectSlot((selectedSlotIndex + 1) % 9);
        else if (Input.mouseScrollDelta.y > 0)
            SelectSlot((selectedSlotIndex + 8) % 9); 
    }
}

