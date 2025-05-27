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


    private void OnEnable()
    {
        if (slotData != null)
            slotData.OnSlotChanged += UpdateUI;
    }

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

    public void SetItem(SlotData slotData)
    {
        if (slotData != null)
        {
            itemIcon.sprite = slotData.icon;
            itemIcon.color = new Color(1, 1, 1, 1);
            quantityText.text = slotData.count.ToString();
            this.slotData = slotData;
        }
    }

    public void SetEmpty()
    {
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
            SetItem(slotData);
        }
        else
        {
            SetEmpty();
        }
        parentInventoryUI?.Refresh();
    }

}
