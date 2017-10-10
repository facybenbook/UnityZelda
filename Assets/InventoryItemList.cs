using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemList : ScriptableObject {
    public List<InventoryItem> itemList;

    public InventoryItem Find(Equipments itemName)
    {
        return (itemList.Find(x => x.itemName == itemName));
    }
}
