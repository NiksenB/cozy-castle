using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item[] items;

    public Dictionary<string, Item> itemDict = new();
    public static ItemManager instance;

    private void Awake()
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
        
        foreach (Item item in items)
        {
            AddItem(item);
        }
    }

    private void AddItem(Item item)
    {
        if (!itemDict.ContainsKey(item.data.itemName))
        {
            itemDict.Add(item.data.itemName, item);
        }
    }

    public Item GetItemByName(string key)
    {
        return itemDict.GetValueOrDefault(key);
    }
}
