using System.Collections.Generic;
using UnityEngine;

public class Inventory_UI : MonoBehaviour
{    
    public string inventoryName;
    public List<Slot_UI> slotUIs = new ();
    [SerializeField] private Canvas canvas;
    private InventoryData inventoryData;

    private void Awake()
    {
        canvas = FindFirstObjectByType<Canvas>();
    }

    private void Start()
    {
        inventoryData = GameManager.gameInstance.player.playerInventoryManager.GetInventoryByName(inventoryName);

        Debug.Assert(inventoryData != null, $"Inventory '{inventoryName}' not found in PlayerInventoryManager.");

        if (inventoryData.slots == null || inventoryData.slots.Count == 0)
        {
            inventoryData.InitializeSlots(inventoryData.defaultSlotCount);
        }

        SetupSlots();
        Refresh();
    }

    public void Refresh()
    {
        if (slotUIs.Count == inventoryData.slots.Count)
        {
            for (int i = 0; i < slotUIs.Count; i++)
            {
                if (inventoryData.slots[i].itemName != "")
                {
                    slotUIs[i].SetItem(inventoryData.slots[i]);
                }
                else
                {
                    slotUIs[i].SetEmpty();
                }
            }
        }
        else
        {
            Debug.Log("Error! Failed to populate inventory: Player has invalid number of inventory slots.");
            Debug.Log($"Inventory '{inventoryName}' has {inventoryData.slots.Count} slots, but Slot_UI has {slotUIs.Count} slots defined.");
        }
    }

    public void Remove()
    {
        Debug.Log("Trying to remove item " + UI_Manager.draggedSlot.slotID);
        Item itemToDrop = ItemManager.instance.GetItemByName(inventoryData.slots[UI_Manager.draggedSlot.slotID].itemName);
        if (itemToDrop != null)
        {
            int numToRemove = UI_Manager.dragSingle ? 1 : inventoryData.slots[UI_Manager.draggedSlot.slotID].count;

            inventoryData.RemoveFromInventory(UI_Manager.draggedSlot.slotID, numToRemove);
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

    public void SlotDrop(Slot_UI targetSlotUI)
    {
        if (UI_Manager.draggedSlot == null || targetSlotUI == null)
        {
            Debug.LogWarning("Dragged slot or target slot is null. Cannot perform drop.");
            return;
        }

        Slot_UI sourceSlotUI = UI_Manager.draggedSlot;
        InventoryData sourceInventory = sourceSlotUI.parentInventoryUI.inventoryData;
        InventoryData targetInventory = targetSlotUI.parentInventoryUI.inventoryData;

        sourceInventory.MoveSlot(
            sourceSlotUI.slotID,
            targetSlotUI.slotID,
            targetInventory,
            UI_Manager.draggedQuantity
        );
        Debug.Log("Dropped " + sourceSlotUI.name + " on " + targetSlotUI.name);

        UI_Manager.instance.RefreshAll();
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
        Debug.Assert(slotUIs.Count == inventoryData.slots.Count, 
            $"Inventory_UI '{inventoryName}' has {inventoryData.slots.Count} slots, but Slot_UI has {slotUIs.Count} slots defined.");

        for (int i = 0; i < slotUIs.Count; i++)
        {
            slotUIs[i].slotID = i;
            slotUIs[i].SetSlotData(inventoryData.slots[i]);
            slotUIs[i].parentInventoryUI = this;
        }
    }
}
