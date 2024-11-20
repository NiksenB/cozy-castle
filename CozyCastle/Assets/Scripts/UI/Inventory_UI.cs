using System.Collections.Generic;
using UnityEngine;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Player player;
    public List<Slot_UI> slots = new ();
    
    private void Awake()
    {
        ToggleInventory();
    } 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        
        if (!inventoryPanel.activeSelf) 
        {
            inventoryPanel.SetActive(true);
            Setup();
        } 
        else
        {
            inventoryPanel.SetActive(false);
        }
    }

    void Setup()
    {
        if (slots.Count >= player.inventory.slots.Count)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (player.inventory.slots[i].type != CollectableType.NONE)
                {
                    slots[i].SetItem(player.inventory.slots[i]);
                }
                else
                {
                    slots[i].SetEmpty();
                }
            }
        } 
        else 
        {
            Debug.Log("Error! Failed to populate inventory: Player has too many inventory slots to show in the UI.");
        }
    }
}
