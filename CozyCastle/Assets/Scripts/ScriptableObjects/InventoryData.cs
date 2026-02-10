

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
#endif

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory")]
public class InventoryData : ScriptableObject
{
    public List<SlotData> slots = new();
    public int defaultSlotCount;
}