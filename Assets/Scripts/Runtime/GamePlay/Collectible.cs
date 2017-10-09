using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {
	public enum CollectibleType{Rupee, Heart, Container, ContainerPiece, RupeeStash, Other};
	public CollectibleType type = CollectibleType.Other;
	public int amount;
    public bool BigItem;

	void OnTriggerEnter2D(Collider2D coll)
	{
		StartCoroutine (DoAction(coll));
	}

	IEnumerator DoAction(Collider2D coll)
	{
		switch (type) {
		case CollectibleType.Rupee:
			{
				if (coll.gameObject.tag == "Player" || coll.gameObject.name == "Sword") {
					yield return GameController.control.AddRupees (amount, false);
					Destroy (this.gameObject);
				}
				break;
			}
		case CollectibleType.Heart:
			{
				if (coll.gameObject.tag == "Player" || coll.gameObject.name == "Sword") {
					GameController.control.Heal (4);
					Destroy (this.gameObject);
				}
				break;
			}
		case CollectibleType.Container:
			{
				if (coll.gameObject.name == "PlayerShadow") {
					yield return StartCoroutine (GameController.control.NewHeart ());
					Destroy (this.gameObject);
				}
				break;
			}
		case CollectibleType.ContainerPiece:
			{
				if (coll.gameObject.name == "PlayerShadow") {
					yield return StartCoroutine (GameController.control.NewHeartPiece ());
					Destroy (this.gameObject);
				}
				break;
			}
		case CollectibleType.Other:
			break;
		}
	}
}
