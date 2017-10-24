using UnityEngine;
using System.Collections;

public class ChestController : Conditionable
{
	public Collectible content;
	public Sprite openSprite;

    private void Start()
    {
        state = false;
        if (visible)
        {
            action = OpenChest;
        }
        else
        {
            ToggleVisible();
            action = ToggleVisible;
        }
    }

    void OpenChest()
    { 
		state = true;
		GetComponent<SpriteRenderer> ().sprite = openSprite;
        StartCoroutine(GameController.control.GetItem(content));
        GetComponent<AudioSource>().Play();
    }
    
    void ToggleVisible()
    {
        GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
        GetComponent<BoxCollider2D>().enabled = !GetComponent<BoxCollider2D>().enabled;
        action = OpenChest;
    }



}
