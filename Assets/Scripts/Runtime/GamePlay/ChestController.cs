using UnityEngine;
using System.Collections;

public class ChestController : Activable
{
	public GameObject content;
	bool open;

	void Start()
	{
		state = false;
		open = false;
		if (conditions.Count > 0) {
			GetComponent<SpriteRenderer> ().sprite = null;
			GetComponent<BoxCollider2D> ().enabled = false;
		}
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (state == true && open == false) {
			
			if (coll.gameObject.tag == "Player") {
				print (coll.gameObject.GetComponent<PlayerMovement> ().direction.y);
				if (coll.gameObject.GetComponent<PlayerMovement> ().direction.y == 1 && Input.GetKeyDown ("u")) {
					open = true;
					GameObject.Instantiate (content);
					GetComponent<AudioSource> ().Play ();
				}
			}
		}
	}

}
