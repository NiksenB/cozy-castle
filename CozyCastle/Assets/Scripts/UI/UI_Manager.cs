using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public Dictionary<string, Inventory_UI> inventoryUIByName = new();
    public GameObject inventoryPanel;
    public List<Inventory_UI> inventoryUIs;
    public Manabar_UI manabarUI;
    public GameObject swapInventoryPanel;
    
    public static UI_Manager instance { get; private set; }
    public static Slot_UI draggedSlot;
    public static Image draggedIcon;
    public static int draggedQuantity;
    public static bool dragSingle;
    public Chest currentChest;
    
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        // Ensure draggedSlot and draggedIcon are initialized.
        draggedSlot = null;
        draggedIcon = null;
        draggedQuantity = 0;
        dragSingle = false;

        Initialize();
    }

    private void Start()
    {
        ToggleInventoryUI(); 
        swapInventoryPanel.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryUI();
        }
        
        dragSingle = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetMouseButton(1);   
    }

    private void OnEnable()
    {
        PlayerInventoryManager.OnInventoryChanged += RefreshAll;
    }

    private void OnDisable()
    {
        PlayerInventoryManager.OnInventoryChanged -= RefreshAll;
    }
    
    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public void ToggleInventoryUI()
    {   
        // Ensures only main inventory can be toggled.
        if (inventoryPanel != null)
        {
            if (!inventoryPanel.activeSelf) 
            {
                inventoryPanel.SetActive(true);
                RefreshInventoryUI("backpack");
                Time.timeScale = 0;
            } 
            else
            {
                inventoryPanel.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void RefreshInventoryUI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
        {
                inventoryUIByName[inventoryName].Refresh();
        }
    }
    
    public void ToggleChestInventory()
    {
        if (inventoryUIByName.ContainsKey("chest"))
        {
            bool opening = !swapInventoryPanel.activeSelf;
            if (!opening)
            {
                inventoryUIByName["chest"].Unbind();
                currentChest?.SaveState(); // persist immediately on manual close too
            }

            swapInventoryPanel.SetActive(opening);
            Time.timeScale = opening ? 0 : 1;
        }
    }
    
    public void RefreshAll()
    {
        foreach (KeyValuePair<string, Inventory_UI> keyValuePair in inventoryUIByName)
        {
            keyValuePair.Value.Refresh();
        }
        manabarUI?.Refresh();
    }

    public Inventory_UI GetInventoryUI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
        {
            return inventoryUIByName[inventoryName];
        } 
        Debug.LogWarning("There is no UI associated with inventoryName: " + inventoryName);
        return null;
    }

    public void RemoveDragged()
    { 
        if (draggedSlot != null)
        {
            draggedSlot.parentInventoryUI.Remove(draggedSlot.slotID, dragSingle);
            draggedSlot = null;
        }
    }

    public void UnbindChestIfBound()
    {
        if (inventoryUIByName.TryGetValue("chest", out var chestUI))
            chestUI.Unbind();
    }
    
    private void Initialize()
    {
        foreach (Inventory_UI ui in inventoryUIs)
        {
            if (!inventoryUIByName.ContainsKey(ui.inventoryName))
            {
                inventoryUIByName.Add(ui.inventoryName, ui);
            }
        }
    }
}