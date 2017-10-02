﻿using UnityEngine;
using System.Collections;

public class Chuchu_IA : CharacterMovement
{
	bool hidden;
	// Use this for initialization
	void Start ()
	{
		rbody = gameObject.GetComponent<Rigidbody2D> ();
		hidden = true;
		inputVector = Vector2.zero;
	}
	
	// Update is called once per frame

	protected override void Action() {
			rbody.velocity = Vector3.zero;
			Move (inputVector, 1);
	}
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player" && hidden == true) 
		{
			GetComponent<Animator> ().SetBool ("is_coming", true);
			GetComponent<CircleCollider2D>().radius = 6;
			hidden = false;
		}
	}
	void OnTriggerStay2D(Collider2D coll)
	{
		//verify if not hurt and collision is the player
		if (GetComponent<Animator>().GetBool("is_hurt") == false && coll.gameObject.tag == "Player")
		{
			//move toward the player
			inputVector.x = (coll.gameObject.transform.position.x - rbody.position.x);
			inputVector.y = coll.gameObject.transform.position.y - rbody.position.y;
			inputVector.Normalize ();
		}
	}
	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player")
		{
			//when the player leaves the sight zone
			inputVector = Vector2.zero;
		}
	}
}
