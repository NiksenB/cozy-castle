using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public Dictionary<string, Inventory_UI> inventoryUIByName = new();
    public GameObject inventoryPanel;
    public List<Inventory_UI> inventoryUIs;
    public Manabar_UI manabarUI;
    public static UI_Manager instance { get; private set; }
    public static Slot_UI draggedSlot;
    public static Image draggedIcon;
    public static int draggedQuantity;
    public static bool dragSingle;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Ensure draggedSlot and draggedIcon are initialized.
        draggedSlot = null;
        draggedIcon = null;
        draggedQuantity = 0;
        dragSingle = false;

        Initialize();
        ToggleInventoryUI(); 
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventoryUI();
        }
        
        dragSingle = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetMouseButton(1);   
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

    public void RefreshAll()
    {
        foreach (KeyValuePair<string, Inventory_UI> keyValuePair in inventoryUIByName)
        {
            keyValuePair.Value.Refresh();
        }
        manabarUI.Refresh();
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

    private void Initialize()
    {
        foreach (Inventory_UI ui in inventoryUIs)
        {
            if (!inventoryUIByName.ContainsKey(ui.inventoryName))
            {
                inventoryUIByName.Add(ui.inventoryName, ui);
            }
        }
        return;
    }
}