using UnityEngine;
using System.Collections;

public class PlayerMovement : CharacterMovement
{
    public bool lockedDirection;
    public GameObject arrow;
	protected override void Action ()
	{
		rbody.velocity = Vector3.zero;
		//is busy if a blocking animation is playing
		if (!anim.GetBool ("is_busy") && !GameController.control.gamePaused) {
			inputVector = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
            
                //If direction changed
                if (inputVector != lastInputVector)
                {
                    //If a key is pressed
                    if (inputVector != Vector2.zero)
                    {
                        //If double key input
                        if (inputVector.x != 0 && inputVector.y != 0)
                        {
                            if (lastInputVector.x != inputVector.x)
                            {
                                direction.x = inputVector.x;
                                direction.y = 0;
                            }
                            else
                            {
                                direction.x = 0;
                                direction.y = inputVector.y;
                            }
                        }
                        //simple key
                        else if (inputVector.x != 0)
                        {
                            direction.x = inputVector.x;
                            direction.y = 0;
                        }
                        else
                        {
                            direction.x = 0;
                            direction.y = inputVector.y;
                        }
                    }
                    lastInputVector = inputVector;
                   
            }
            //if the direction is not locked, update direction for the animator
            if (lockedDirection == false)
            {
                anim.SetFloat("input_x", direction.x);
                anim.SetFloat("input_y", direction.y);
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
			if (inputVector != Vector2.zero) {
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