using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {
	public int amount;
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (gameObject.tag == "Player" && coll.gameObject.tag == "Hitbox") 
		{
			print (coll.gameObject);
			GameController.control.NewHeart ();
			Destroy (this.gameObject);
		}
		if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Weapon")
		{
			if (gameObject.tag == "Rupee") {
				if (amount == 1 && GameController.control.firstOneRupee == false) {
					GameController.control.gameGUIController.DisplayMessage ("Vous avez obtenu un rubis, c'est le début de la richesse !");
					GameController.control.firstOneRupee = true;
				}
				else if (amount == 5 && GameController.control.firstFiveRupee == false) {
					GameController.control.gameGUIController.DisplayMessage ("Vous avez obtenu un rubis, c'est le début de la richesse !");
					GameController.control.firstFiveRupee = true;
				}
				GameController.control.AddRupees (amount);
				Destroy (this.gameObject);
			} 
		}
	}
}
