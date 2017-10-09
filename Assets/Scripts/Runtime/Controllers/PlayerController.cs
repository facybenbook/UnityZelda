﻿using UnityEngine;
using System.Collections;

public class PlayerController : CharactersController
{
    public bool lockedDirection;
	public Vector2 lastInput;

    protected override void Start()
    {
        base.Start();
        characterOrientation = Vector2.down;
        movementDirection = Vector2.down;
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rbody.velocity = Vector3.zero;
    }
    protected override void Action ()
	{
		rbody.velocity = Vector3.zero;
		//is busy if a blocking animation is playing
		if (!anim.GetBool ("is_busy") && !GameController.control.gamePaused) {
			movementDirection = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
			if (lockedDirection == false) {
				//If direction changed
				if (movementDirection != lastInput) {
					//If a key is pressed
					if (movementDirection != Vector2.zero) {
						if (movementDirection.y == 0) {
							characterOrientation.x = movementDirection.x;
							if (movementDirection.x != 0)
								characterOrientation.y = 0;
						}
						else if (movementDirection.x == 0) {
								characterOrientation.y = movementDirection.y;
							if (movementDirection.y != 0)
								characterOrientation.x = 0;
						}
						anim.SetFloat ("input_x", characterOrientation.x);
						anim.SetFloat ("input_y", characterOrientation.y);
						lastInput = movementDirection;
					}
				}
			}
            if (Input.GetKeyDown (GameKeys.A)) {
                EquipmentInSlot(GameController.control.playerStats.slotA);
			}
			else if (Input.GetKeyDown (GameKeys.B)) {
                EquipmentInSlot(GameController.control.playerStats.slotB);
			}
			if (movementDirection != Vector2.zero) {
				if (Input.GetKeyDown ("space")) {
					//roll
					anim.SetTrigger ("is_rolling");
                } else {
					//walk
					anim.SetBool ("is_walking", true);
					Move (1);
				}
			} else {
				//idle
				anim.SetBool ("is_walking", false);
			}
		}
	}
    protected override void OnPause()
    {
        GetComponent<Animator>().SetTrigger("stop_action");
    }

    /// <summary>
    /// W.I.P
    /// Triggers the GetItem animations in the player's animator and...?
    /// </summary>
    /// <param name="item"></param>
    public void GetItem(GameObject item)
    {
        if (item.GetComponent<Collectible>().BigItem == true)
            anim.SetTrigger("BigItemGot");
        else
            anim.SetTrigger("SmallItemGot");
    }
    
    /// <summary>
    /// Creates an instance of arrow and rotates it to follow the character sprite orientation,
    /// the arrow will move along its orientation
    /// </summary>
    public void CreateArrow()
    {
        GameObject arrow = Instantiate(Resources.Load("Prefabs/Arrow") as GameObject);
        
        if (characterOrientation == Vector2.left)
        {
            arrow.transform.position = transform.position + new Vector3(-0.5f, 0.28125f, 0);
            arrow.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (characterOrientation == Vector2.right)
        {
            arrow.transform.position = transform.position + new Vector3(0.5625f, 0.40625f, 0);
            arrow.transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (characterOrientation == Vector2.up)
        {
            arrow.transform.position = transform.position + new Vector3(-0.09375f, 1, 0);
            arrow.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (characterOrientation == Vector2.down)
        {
            arrow.transform.position = transform.position + new Vector3(0.09375f, 0, 0);
            arrow.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        GameController.control.AddToZIndex(arrow);
    }
    
    /// <summary>
    /// Creates an instance of bomb, if the player has remote bombs, create a remote bomb
    /// </summary>
    public void CreateBomb()
    {
        GameObject bomb;
        if (GameController.control.playerStats.HaveEquipment(Equipments.RemoteBomb))
            bomb = Instantiate(Resources.Load("Prefabs/RemoteBomb") as GameObject);
        else
            bomb = Instantiate(Resources.Load("Prefabs/Bomb") as GameObject);
        if (characterOrientation == Vector2.right)
            bomb.transform.position = transform.position + new Vector3(+0.75f, 0.25f, 0);
        else if (characterOrientation == Vector2.left)
            bomb.transform.position = transform.position + new Vector3(-0.75f, 0.25f, 0);
        else if (characterOrientation == Vector2.down)
            bomb.transform.position = transform.position + new Vector3(0, -0.25f, 0);
        else if (characterOrientation == Vector2.up)
            bomb.transform.position = transform.position + new Vector3(0, 0.75f, 0);
        bomb.transform.rotation = Quaternion.Euler(0, 0, 0);
        GameController.control.AddToZIndex(bomb);
    }

    /// <summary>
    /// performs actions according to the item passed in parameter
    /// </summary>
    /// <param name="item"></param>
    private void EquipmentInSlot(Equipments item)
    {
        switch (item)
        {
            case Equipments.Sword:
                    anim.SetBool("is_sword", true);
                    return;
            case Equipments.Bow:
                    anim.SetBool("is_bow", true);
                    return;
            case Equipments.MoleClaws:
                    anim.SetTrigger("is_digging");
                    return;
            case Equipments.Bomb:
                    CreateBomb();
                    return;
            case Equipments.RemoteBomb:
                    CreateBomb();
                    return;
        }
    }


    

}