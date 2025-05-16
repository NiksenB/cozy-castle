using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{    
    public string inventoryName;
    public List<Slot_UI> slots = new ();
    [SerializeField] private Canvas canvas;
    private Inventory inventory;
    
    private void Awake()
    {
        canvas = FindFirstObjectByType<Canvas>();
    }

    private void Start()
    {
        inventory = GameManager.gameInstance.player.inventory.GetInventoryByName(inventoryName);
        SetupSlots();
        Refresh();
    }

    public void Refresh()
    {
        if (slots.Count == inventory.slots.Count)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (inventory.slots[i].itemName != "")
                {
                    slots[i].SetItem(inventory.slots[i]);
                }
                else
                {
                    slots[i].SetEmpty();
                }
            }
        } 
        else 
        {
            Debug.Log("Error! Failed to populate inventory: Player has invalid number of inventory slots.");
        }
    }

    public void Remove()
    {
        Debug.Log("Trying to remove item " + UI_Manager.draggedSlot.slotID);
        Item itemToDrop = GameManager.gameInstance.itemManager.GetItemByName(inventory.slots[UI_Manager.draggedSlot.slotID].itemName);
        if (itemToDrop != null)
        {
            int numToRemove = UI_Manager.dragSingle ? 1 : inventory.slots[UI_Manager.draggedSlot.slotID].count;

            inventory.RemoveFromInventory(UI_Manager.draggedSlot.slotID, numToRemove);
            GameManager.gameInstance.player.DropItem(itemToDrop, numToRemove);
            Refresh();
        }

        UI_Manager.draggedSlot = null;
    }

    public void SlotBeginDrag(Slot_UI slot)
    {
        UI_Manager.draggedSlot = slot;

        //Spawn in world
        UI_Manager.draggedIcon = Instantiate(slot.itemIcon);
        UI_Manager.draggedIcon.transform.SetParent(canvas.transform, false);
        UI_Manager.draggedIcon.raycastTarget = false;
        UI_Manager.draggedIcon.rectTransform.localScale = Vector3.one * 0.1f;

        if (UI_Manager.dragSingle)
        {
            UI_Manager.draggedQuantity = 1;
        }
        else if (int.TryParse(UI_Manager.draggedSlot.quantityText.text, out int initialQuantity))
        {
            {
                UI_Manager.draggedQuantity = initialQuantity;
            }
        }

        MoveToMousePosition(UI_Manager.draggedIcon.gameObject);
    }

    public void SlotDrag()
    {
        MoveToMousePosition(UI_Manager.draggedIcon.gameObject);
    }

    public void SlotEndDrag()
    {
        Destroy(UI_Manager.draggedIcon.gameObject);
        UI_Manager.draggedIcon = null;
    }

    public void SlotDrop(Slot_UI slot)
    {
        UI_Manager.draggedSlot.inventory.MoveSlot(UI_Manager.draggedSlot.slotID, slot.slotID, slot.inventory, UI_Manager.draggedQuantity);
        Debug.Log("Dropped " + UI_Manager.draggedSlot.name + " on " + slot.name);

        GameManager.gameInstance.uiManager.RefreshAll();
    }

    private void MoveToMousePosition(GameObject toMove)
    {
        if(canvas != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out Vector2 position);
            toMove.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    private void SetupSlots()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].slotID = i;
            slots[i].inventory = inventory;
        }
    }
}
