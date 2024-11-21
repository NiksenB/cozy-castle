using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Collectable[] collectableItems;

    public Dictionary<CollectableType, Collectable> collectables = new();

    private void Awake()
    {
        foreach (Collectable item in collectableItems)
        {
            AddItem(item);
        }
    }

    private void AddItem(Collectable item)
    {
        if (!collectables.ContainsKey(item.type))
        {
            collectables.Add(item.type, item);
        }
    }

    public Collectable GetItemByType(CollectableType type)
    {
        return collectables.GetValueOrDefault(type);
    }
}
