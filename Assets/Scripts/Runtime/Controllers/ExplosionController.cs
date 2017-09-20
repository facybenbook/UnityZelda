using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.name == "HitBox" && coll.gameObject.GetComponent<LifeController>()) {
			//Hurt the player with damages, and gives the hurter's position for the character to escape
			coll.gameObject.GetComponent<LifeController> ().Hurt (2, this.transform.position);
		}
		else if (coll.gameObject.GetComponent<Destructible>()) {
			coll.gameObject.GetComponent<Destructible> ().DestroyObject ();
		}
	}
}
