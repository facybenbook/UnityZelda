using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour {
    public enum WhichButton {A, B, L, R};
    public WhichButton whichButton;
    public Image sprite;
    protected InventorySlot currentItem;

    void Start()
    {
        if (whichButton == WhichButton.A)
            currentItem = GameController.control.playerStats.slotA;
        else if (whichButton == WhichButton.B)
            currentItem = GameController.control.playerStats.slotB;
        UpdateSprite(currentItem);
    }
    // Update is called once per frame
    void Update ()
    {
        //Which button is the script attached to ?
        switch (whichButton)
        {
            case WhichButton.A:
                if (GameController.control.playerStats.slotA.item.name != currentItem.item.name)
                {
                    currentItem = GameController.control.playerStats.slotA;
                    UpdateSprite(currentItem);
                }
                break;
            case WhichButton.B:
                if (GameController.control.playerStats.slotB.item.name != currentItem.item.name)
                {
                    currentItem = GameController.control.playerStats.slotB;
                    UpdateSprite(currentItem);
                }
                break;
        }
    }

    void UpdateSprite(InventorySlot slot)
    {
        sprite.sprite = slot.item.icon;
    }
}
