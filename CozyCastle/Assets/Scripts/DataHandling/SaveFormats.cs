using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChestSaveData
{
    public string chestId;
    public List<SlotDataSave> slots = new();
}

[Serializable]
public class SlotDataSave
{
    public string itemName;
    public int count;

    public SlotDataSave(SlotData slot)
    {
        itemName = slot.itemName;
        count = slot.count;
    }

    public SlotData ToSlotData()
    {
        var slot = new SlotData
        {
            itemName = itemName,
            count = count
        };

        return slot;
    }
}

[Serializable]
public class SaveFile
{
    public List<ChestSaveData> chests = new();
}