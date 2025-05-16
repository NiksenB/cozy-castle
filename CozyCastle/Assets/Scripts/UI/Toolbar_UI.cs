using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolbar_UI : MonoBehaviour
{
    public List<Slot_UI> toolbarSlots;
    private Slot_UI selectedSlot;
    private int selectedSlotIndex;

    private void Start ()
    {
        SelectSlot(0);
    }
    private void Update()
    {
        checkAlphaNumericKeys();
        checkScroll();
    }
    public void SelectSlot(int index)
    {
        if (toolbarSlots.Count == 9 && index >= 0 && index <= 8)
        {
            if(selectedSlot != null)
            {
                selectedSlot.SetHighlight(false);
            }
            selectedSlot = toolbarSlots[index];
            selectedSlot.SetHighlight(true);
            selectedSlotIndex = index;
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
        {
            SelectSlot(selectedSlotIndex + 1);
        }
        else if  (Input.mouseScrollDelta.y > 0)
        {
            SelectSlot(selectedSlotIndex - 1);
        }
    }
}

