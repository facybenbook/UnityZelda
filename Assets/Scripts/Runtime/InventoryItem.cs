using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum ItemType {Other, Weapon, Consummable, Quest}
[System.Serializable]
public class InventoryItem {

    public string name;                 //the name to be used to find the item
    public Sprite icon;                 //icon that is used by the GUI

    public ItemType itemType;        

    public int afterConsumed;     //becomes another item after being consumed ?
    public bool usesAmmo = false;           //if it's a bomb or bow or else
    public bool stackable;
    public int maxStack;
    public bool unique;
}