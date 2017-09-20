using UnityEngine;
using System.Collections.Generic;

public class Destructible : MonoBehaviour {
	public bool loot = true;
	public GameObject[] lootList;
	public int[] lootChances = new int[10];

	public void DestroyObject ()
	{
		//if it's something that drops objects
		if (loot)
		{
			
			GameObject objet = RandomItem();
			if (objet != null) {
				objet = Instantiate (objet);
				objet.transform.position = gameObject.transform.localPosition;
				//objet.GetComponentInChildren<Animator> ().SetBool ("is_bouncing", true);
			}
			Destroy (this.gameObject);
		}
	}
	GameObject RandomItem() {
		int range = 0;
		for (int i = 0; i < lootChances.Length; i++) {
			range += lootChances [i];
		}
		int rand = Random.Range (0, range);
		int top = 0;
		for (int i = 0; i < lootChances.Length; i++) {
			top += lootChances [i];
			if (rand < top) {
				print ("looted");
				return lootList [i];
			}
		}
		return null;
	}
}