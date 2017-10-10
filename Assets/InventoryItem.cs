using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public enum Equipments { None, Sword, Bow, MoleClaws, Bottle, Bomb, RemoteBomb };
[System.Serializable]
public class InventoryItem {
    public Equipments itemName;         //the name to be used to find the item
    public Sprite itemIcon;             //icon that is used by the GUI
    public bool usesAmmo = false;       //if it's a bomb or bow or else
}