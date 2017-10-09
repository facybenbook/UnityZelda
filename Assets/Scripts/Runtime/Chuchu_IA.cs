using UnityEngine;
using System.Collections;

public class Chuchu_IA : CharactersController
{
	bool hidden;

	protected override void Start ()
	{
		rbody = gameObject.GetComponent<Rigidbody2D> ();
		hidden = true;
		movementDirection = Vector2.zero;
	}
	
	protected override void Action() {
			rbody.velocity = Vector3.zero;
			Move (1);
	}

    //if the chuchu sees the player (visual radius), appears
    void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player" && hidden == true) 
		{
			GetComponent<Animator> ().SetBool ("is_coming", true);
			GetComponent<CircleCollider2D>().radius = 6;
			hidden = false;
		}
	}

    //while the player is in the visual radius of the chuchu, chase the player
    void OnTriggerStay2D(Collider2D coll)
	{
		//verify if the object in the rdius is the player
		if (coll.gameObject.tag == "Player")
		{
			//move toward the player
			movementDirection.x = (coll.gameObject.transform.position.x - rbody.position.x);
			movementDirection.y = coll.gameObject.transform.position.y - rbody.position.y;
			movementDirection.Normalize ();
		}
	}
	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player")
		{
			//when the player leaves the sight zone
			movementDirection = Vector2.zero;
		}
	}
}
