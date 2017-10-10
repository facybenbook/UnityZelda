using UnityEngine;
using System.Collections.Generic;

// defines the 
public enum DestructibleType {small, medium, big}
public class Destructible : MonoBehaviour {
    //affects the interaction with the destructors
    public DestructibleType type;
	public bool loot = true;
	public GameObject[] lootList;
	public int[] lootChances = new int[10];

	public void DestructionPhase ()
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
            gameObject.GetComponent<Animator>().SetBool("Destroyed", true);
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
				return lootList [i];
			}
		}
		return null;
	}
}