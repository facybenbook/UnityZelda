using UnityEngine;
using System.Collections;

public class ChestController : AnimatedMechanism
{
    public Collectible content;
	public Sprite openSprite;
    public Sprite closedSprite;

    void OpenChest()
    {
		GetComponent<SpriteRenderer> ().sprite = openSprite;
        StartCoroutine(GameController.control.GetItem(content));
        GetComponent<AudioSource>().Play();

    }
}
