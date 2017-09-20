using UnityEngine;
using System.Collections;

public class Hurt_touch : MonoBehaviour {

	public int damages = 1;
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.name == "Player")
		{
			//Hurt the player with damages, and gives the hurter's position for the character to escape
			coll.gameObject.GetComponent<LifeController>().Hurt(damages, this.transform.position);
		}
	}
}
