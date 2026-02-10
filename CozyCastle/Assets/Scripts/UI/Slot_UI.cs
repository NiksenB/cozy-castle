using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot_UI : MonoBehaviour
{
    public int slotID;
    public SlotData slotData;
    public Inventory_UI parentInventoryUI;
    public UnityEngine.UI.Image itemIcon;
    public TextMeshProUGUI quantityText;

    [SerializeField] private GameObject highlight;

    private void OnDisable()
    {
        if (slotData != null)
            slotData.OnSlotChanged -= UpdateUI;
    }

    public void SetSlotData(SlotData data)
    {
        if (slotData != null)
            slotData.OnSlotChanged -= UpdateUI;
        
        slotData = data;

        if (slotData != null)
            slotData.OnSlotChanged += UpdateUI;

        UpdateUI();
    }

    public void SetItem(SlotData data)
    {
        if (this == null || !gameObject.activeInHierarchy) return;
        if (data != null)
        {
            itemIcon.sprite = data.Icon;
            itemIcon.color = new Color(1, 1, 1, 1);
            quantityText.text = data.count.ToString();
            slotData = data;
        }
    }

    public void SetEmpty()
    {
        if (this == null || !gameObject.activeInHierarchy) return;
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0); // make empty image invisible
        quantityText.text = "";
        slotData = null;
    }

    public void SetHighlight(bool isOn)
    {
        highlight.SetActive(isOn);
    }

    private void UpdateUI()
    {
        if (slotData != null)
        {
            if (slotData.Icon == null && !string.IsNullOrEmpty(slotData.itemName))
                slotData.RebuildFromItemDatabase();

            SetItem(slotData);
        }
        else
        {
            SetEmpty();
        }

        // TODO remove this call
        //parentInventoryUI?.Refresh();
    }

    private void OnDestroy()
    {
        if (slotData != null)
            slotData.OnSlotChanged -= UpdateUI;
    }
}
