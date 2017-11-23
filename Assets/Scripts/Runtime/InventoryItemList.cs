using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItemList : ScriptableObject {
    [SerializeField]
    protected List<InventoryItem> itemList;

    public void Initialize()
    {
        itemList = new List<InventoryItem>();
    }

    public int Count()
    {
        return (itemList.Count);
    }

    public InventoryItem GetItem(int index)
    {
        return itemList[index];
    }

    public void AddItem(InventoryItem newItem)
    {
        itemList.Add(newItem);
    }

    public void RemoveItem(int index)
    {
        itemList.RemoveAt(index);
    }

    public string[] GetNameList()
    {
        List<string> nameList = new List<string>();
        foreach (InventoryItem item in itemList)
        {
            nameList.Add(item.name);
        }
        return (nameList.ToArray());
    }

    public InventoryItem GetItem(string itemName)
    {
        return itemList.Find(x => x.name == itemName);
    }
}
