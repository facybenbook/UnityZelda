using UnityEngine;
using System.Collections;

public class ChestController : Conditionable
{
    public bool startsVisible = true;
    public Collectible content;
	public Sprite openSprite;

    protected override void Start()
    {
        base.Start();
        if (startsVisible == false)
        {
            ToggleVisible();
            action = ToggleVisible;
        }
        else
            action = OpenChest;
    }

    void OpenChest()
    { 
		state = true;
		GetComponent<SpriteRenderer> ().sprite = openSprite;
        StartCoroutine(GameController.control.GetItem(content));
        GetComponent<AudioSource>().Play();
    }

    public override void OnCheckConditions()
    {
        if (startsVisible == false)
        {
            base.OnCheckConditions();
        }
    }

    void ToggleVisible()
    {
        GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
        GetComponent<BoxCollider2D>().enabled = !GetComponent<BoxCollider2D>().enabled;
        action = OpenChest;
    }
}
