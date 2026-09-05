using System.Collections.Generic;
using UnityEngine;

public class Inventory_UI : MonoBehaviour
{    
    public string inventoryName;
    public List<Slot_UI> slotUIs = new ();
    [SerializeField] private Canvas canvas;
    private List<SlotData> slots;

    private void Awake()
    {
        canvas = FindFirstObjectByType<Canvas>();
    }

    private void Start() { } // nothing needed here
    
    private void OnEnable()
    {
        PlayerInventoryManager.OnInventoriesReady += TryBind;
        GameManager.OnPlayerRegistered += OnPlayerRegistered;
        TryBind();
    }

    private void OnDisable()
    {
        PlayerInventoryManager.OnInventoriesReady -= TryBind;
        GameManager.OnPlayerRegistered -= OnPlayerRegistered;
    }

    private void OnPlayerRegistered(Player player)
    {
        TryBind();
    }
    
    private void TryBind()
    {
        if (inventoryName == "chest")
           return; // chest binds manually via Chest.Interact();

        if (GameManager.gameInstance == null)
            return;

        var player = GameManager.gameInstance.Player;

        if (player == null)
            return;

        var inv = player.playerInventoryManager;

        if (inv == null)
            return;

        Debug.LogWarning($"[UI:{inventoryName}] TryBind resolved Player id={player.GetInstanceID()}, PIM id={inv.GetInstanceID()}");
        
        var slotDatas = inv.GetInventoryByName(inventoryName);

        if (slotDatas == null)
        {
            Debug.LogWarning(
                $"'{inventoryName}': inventory not found."
            );
            return;
        }

        Bind(slotDatas);
    }

    
    public void Unbind()
    {
        slots = null;
    }
    
    private void SetupSlots()
    {
        for (int i = 0; i < slotUIs.Count; i++)
        {
            var data = slots.Count > i ? slots[i] : new SlotData();
            
            slotUIs[i].slotID = i;
            slotUIs[i].SetSlotData(data);
            slotUIs[i].parentInventoryUI = this;
        }
    }
    
    public void Refresh()
    {
        if (slots == null)
        {
            TryBind(); // self-heal: attempt to (re)bind before giving up
        }

        if (slots == null)
        {
            Debug.LogWarning($"Inventory_UI '{inventoryName}' has not been bound.");
            return;
        }

        Debug.Log($"Refreshing inventory UI: {inventoryName}");
        
        if (slotUIs.Count == slots.Count) 
        {
            for (int i = 0; i < slotUIs.Count; i++)
            {
                if (!string.IsNullOrEmpty(slots[i].itemName))
                {
                    if (slots[i].Icon == null)
                        slots[i].RebuildFromItemDatabase(); // safety net
                    slotUIs[i].SetItem(slots[i]);
                }
                else
                {
                    slotUIs[i].SetEmpty();
                }
            }
        }
        else
        {
            Debug.LogWarning($"Inventory '{inventoryName}' has {slots.Count} slots, but Slot_UI has {slotUIs.Count} slots defined.");
        }
    }

    public void Remove(int slotID, bool single)
    {
        var slot = slots[slotID];
        Item itemToDrop = ItemManager.instance.GetItemByName(slot.itemName);

        if (itemToDrop == null)
        {
            Debug.LogWarning("Item to drop not found.");
            return;
        }

        int numToRemove = single ? 1 : slot.count;

        for (int i = 0; i < numToRemove; i++)
            slot.RemoveItem();

        GameManager.gameInstance.Player.DropItem(itemToDrop, numToRemove);
        Refresh();
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
        List<SlotData> sourceSlots = sourceSlotUI.parentInventoryUI.slots;
        List<SlotData> targetSlots = targetSlotUI.parentInventoryUI.slots;
        
        PlayerInventoryManager.MoveItem
        (
            sourceSlots,
            sourceSlotUI.slotID,
            targetSlots,
            targetSlotUI.slotID,
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
    
    public void Bind(List<SlotData> newSlots)
    {
        Debug.LogWarning($"[UI:{inventoryName}] Bind() using list hash={newSlots.GetHashCode()} count={newSlots.Count}");
        slots = newSlots;
        SetupSlots();
        Refresh();
    }
}
