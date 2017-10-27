using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonScript : MonoBehaviour {
    public enum WhichButton {A, B, L, R};
    public WhichButton whichButton;
    public InventoryItemList itemInfos;
    protected InventoryItem currentItem;

    void Start()
    {
        currentItem = itemInfos.Find(Equipments.None);
    }
    // Update is called once per frame
    void Update ()
    {
        //Which button is the script attached to ?
        switch (whichButton)
        {
            case WhichButton.A:
                if (GameController.control.playerStats.slotA != currentItem.itemName)
                {
                    currentItem = itemInfos.Find(GameController.control.playerStats.slotA);
                    UpdateButtonSprite(currentItem.itemIcon);
                }
                break;
            case WhichButton.B:
                if (GameController.control.playerStats.slotB != currentItem.itemName)
                {
                    currentItem = itemInfos.Find(GameController.control.playerStats.slotB);
                    UpdateButtonSprite(currentItem.itemIcon);
                }
                break;
        }
    }

    void UpdateButtonSprite(Sprite newSprite)
    {
        transform.Find("ItemGUI").GetComponent<Image>().sprite = newSprite;
    }
}
