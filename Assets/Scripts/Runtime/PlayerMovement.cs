using UnityEngine;
using System.Collections;

public class PlayerMovement : CharacterMovement
{
    public bool lockedDirection;
    public GameObject arrow;
	public  Vector2 lookingTowards;
	public Vector2 lastInput;
	protected override void Action ()
	{
		rbody.velocity = Vector3.zero;
		//is busy if a blocking animation is playing
		if (!anim.GetBool ("is_busy") && !GameController.control.gamePaused) {
			direction = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
			if (lockedDirection == false) {
				//If direction changed
				if (direction != lastInput) {
					//If a key is pressed
					if (direction != Vector2.zero) {
						if (direction.y == 0) {
							lookingTowards.x = direction.x;
							if (direction.x != 0)
								lookingTowards.y = 0;
						}
						else if (direction.x == 0) {
								lookingTowards.y = direction.y;
							if (direction.y != 0)
								lookingTowards.x = 0;
						}
						anim.SetFloat ("input_x", lookingTowards.x);
						anim.SetFloat ("input_y", lookingTowards.y);

						lastInput = direction;
					}
				}
			}
            //if the direction is not locked, update direction for the animator
            
            //i is the A button used for the first equipment slot
            if (Input.GetKeyDown (GameKeys.A)) {
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
			else if (Input.GetKeyDown (GameKeys.B)) {
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
			if (direction != Vector2.zero) {
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

    public void CreateArrow(int direction)
    {
        if (direction == 0)
            GameObject.Instantiate(arrow, transform.position + new Vector3(-0.5f, 0.28125f, 0), Quaternion.Euler(0, 0, 90));
        else if (direction == 1)
            GameObject.Instantiate(arrow, transform.position + new Vector3(0.5625f, 0.40625f, 0), Quaternion.Euler(0, 0, 270));
        else if (direction == 2)
            GameObject.Instantiate(arrow, transform.position + new Vector3(-0.09375f, 1, 0), Quaternion.Euler(0, 0, 0));
        else if (direction == 3)
            GameObject.Instantiate(arrow, transform.position + new Vector3(0.09375f, 0, 0), Quaternion.Euler(0, 0, 180));
    }
}