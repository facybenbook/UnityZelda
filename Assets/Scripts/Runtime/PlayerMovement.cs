using UnityEngine;
using System.Collections;

public class PlayerMovement : CharacterMovement
{
	void Update ()
	{
		if (!GameController.control.gamePaused) {
			GetComponent<Animator> ().speed = 1;
			if (!GameController.control.gameOverState)
				Action ();
		}
	}

	protected override void Action ()
	{
		rbody.velocity = Vector3.zero;
		//is busy if a blocking animation is playing
		if (!anim.GetBool ("is_busy") && !GameController.control.gamePaused) {
			movement_vector = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
			//Inout direction processing
			if (movement_vector.x != lastMovement.x || movement_vector.y != lastMovement.y) {
				//take the different axis direction as new direction
				if (movement_vector != Vector2.zero) {
					//double key
					if (movement_vector.x != 0 && movement_vector.y != 0) {
						if (lastMovement.x != movement_vector.x) {
							direction.x = movement_vector.x;
							direction.y = 0;
						} else {
							direction.x = 0;
							direction.y = movement_vector.y;
						}
					}
					//simple key
					else if (movement_vector.x != 0) {
						direction.x = movement_vector.x;
						direction.y = 0;
					} else {
						direction.x = 0;
						direction.y = movement_vector.y;
					}
				}
				lastMovement = movement_vector;
				anim.SetFloat ("input_x", direction.x);
				anim.SetFloat ("input_y", direction.y);
			}
			//i is the A button used for the first equipment slot
			if (Input.GetKeyDown ("i")) {
				switch (GameController.control.playerStats.slotA) {
				case PlayerStats.Equipments.Sword: 
					{
						anim.SetBool ("is_sword", true);
						return;
					}
				case PlayerStats.Equipments.Bow: 
					{
						anim.SetBool ("is_bow", true);
						return;
					}
				case PlayerStats.Equipments.MoleClaws: 
					{
						anim.SetTrigger ("is_digging");
						return;
					}
				}
			}
			else if (Input.GetKeyDown ("o")) {
				switch (GameController.control.playerStats.slotB) {
				case PlayerStats.Equipments.Sword: 
					{
						anim.SetBool ("is_sword", true);
						return;
					}
				case PlayerStats.Equipments.Bow: 
					{
						anim.SetBool ("is_bow", true);
						return;
					}
				case PlayerStats.Equipments.MoleClaws: 
					{
						anim.SetTrigger ("is_digging");
						return;
					}
				}
			}
			if (movement_vector != Vector2.zero) {
				if (Input.GetKeyDown ("space")) {
					//roll
					anim.SetTrigger ("is_rolling");
				} else {
					//walk
					anim.SetBool ("is_walking", true);
					Move (direction, 1);
				}
			} else {
				//idle
				anim.SetBool ("is_walking", false);
			}
		}
	}
	void OnPause()
	{
		GetComponent<Animator> ().SetTrigger ("stop_action");
	}
}