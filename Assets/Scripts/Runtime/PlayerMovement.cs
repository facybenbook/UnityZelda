using UnityEngine;
using System.Collections;

public class PlayerMovement : CharacterMovement
{
    public bool lockedDirection;
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
                EquipmentSwitch(GameController.control.playerStats.slotA);
			}
			else if (Input.GetKeyDown (GameKeys.B)) {
                EquipmentSwitch(GameController.control.playerStats.slotB);
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

    public void CreateArrow()
    {
        GameObject arrow = Resources.Load("Prefabs/Arrow") as GameObject;

        if (lookingTowards == Vector2.left)
            GameController.control.AddToZIndex(arrow, transform.position + new Vector3(-0.5f, 0.28125f, 0), Quaternion.Euler(0, 0, 90));
        else if (lookingTowards == Vector2.right)
            GameController.control.AddToZIndex(arrow, transform.position + new Vector3(0.5625f, 0.40625f, 0), Quaternion.Euler(0, 0, 270));
        else if (lookingTowards == Vector2.up)
            GameController.control.AddToZIndex(arrow, transform.position + new Vector3(-0.09375f, 1, 0), Quaternion.Euler(0, 0, 0));
        else if (lookingTowards == Vector2.down)
            GameController.control.AddToZIndex(arrow, transform.position + new Vector3(0.09375f, 0, 0), Quaternion.Euler(0, 0, 180));
    }

    public void CreateBomb()
    {
        GameObject bomb;
        if (GameController.control.playerStats.HaveEquipment(Equipments.RemoteBomb))
            bomb = Resources.Load("Prefabs/RemoteBomb") as GameObject;
        else
            bomb = Resources.Load("Prefabs/Bomb") as GameObject;
        if (lookingTowards == Vector2.right)
            GameController.control.AddToZIndex(bomb, transform.position + new Vector3(+0.75f, 0.25f, 0), Quaternion.Euler(0, 0, 0));
        else if (lookingTowards == Vector2.left)
            GameController.control.AddToZIndex(bomb, transform.position + new Vector3(-0.75f, 0.25f, 0), Quaternion.Euler(0, 0, 0));
        else if (lookingTowards == Vector2.down)
            GameController.control.AddToZIndex(bomb, transform.position + new Vector3(0, -0.25f, 0), Quaternion.Euler(0, 0, 0));
        else if (lookingTowards == Vector2.up)
            GameController.control.AddToZIndex(bomb, transform.position + new Vector3(0, 0.75f, 0), Quaternion.Euler(0, 0, 0));
        
    }

    void EquipmentSwitch(Equipments slot)
    {
        switch (slot)
        {
            case Equipments.Sword:
                {
                    anim.SetBool("is_sword", true);
                    return;
                }
            case Equipments.Bow:
                {
                    anim.SetBool("is_bow", true);
                    return;
                }
            case Equipments.MoleClaws:
                {
                    anim.SetTrigger("is_digging");
                    return;
                }
            case Equipments.Bomb:
                {
                    CreateBomb();
                    return;
                }
            case Equipments.RemoteBomb:
                {
                    CreateBomb();
                    return;
                }
        }
    }
}