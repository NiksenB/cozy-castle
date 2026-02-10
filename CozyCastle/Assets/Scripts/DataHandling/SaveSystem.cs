using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath =>
        Path.Combine(Application.persistentDataPath, "save.json");

    private static SaveFile _save;

    public static List<SlotData> LoadChest(string chestId)
    {
        LoadFile();

        var chest = _save.chests.Find(c => c.chestId == chestId);
        if (chest == null)
            return null;

        var slots = chest.slots
            .Select(s => s.ToSlotData())
            .ToList();

        // IMPORTANT: rebuild runtime-only data
        foreach (var slot in slots)
            slot.RebuildFromItemDatabase();

        return slots;
    }

    public static void SaveChest(string chestId, List<SlotData> slots)
    {
        LoadFile();

        var chest = _save.chests.Find(c => c.chestId == chestId);
        if (chest == null)
        {
            chest = new ChestSaveData { chestId = chestId };
            _save.chests.Add(chest);
        }

        chest.slots = slots
            .Select(s => new SlotDataSave(s))
            .ToList();

        WriteFile();
    }

    private static void LoadFile()
    {
        if (_save != null)
            return;

        if (!File.Exists(SavePath))
        {
            _save = new SaveFile();
            return;
        }

        _save = JsonUtility.FromJson<SaveFile>(
                    File.ReadAllText(SavePath))
                ?? new SaveFile();
    }

    private static void WriteFile()
    {
        File.WriteAllText(
            SavePath,
            JsonUtility.ToJson(_save, true));
    }
    
    public static void RebuildSlots(List<SlotData> slots)
    {
        foreach (var slot in slots)
            slot.RebuildFromItemDatabase();
    }
}