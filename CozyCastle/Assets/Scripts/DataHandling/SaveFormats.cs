using System;
using System.Collections.Generic;

[Serializable]
public class ChestSaveData
{
    public string chestId;
    public List<SlotData> slots = new();
}

[Serializable]
public class SaveFile
{
    public List<ChestSaveData> chests = new();
}

// call sometime after load
// public void RebuildFromItemDatabase()
// {
//     if (string.IsNullOrEmpty(itemName))
//         return;
//
//     var item = ItemManager.instance.itemDict.GetValueOrDefault(itemName);
//     icon = item.data.icon;
//     isStackable = item.data.isStackable;
// }
