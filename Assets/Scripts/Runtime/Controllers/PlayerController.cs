using UnityEngine;
using System.Collections;
using System;

public class PlayerController : CharactersController
{
    public bool lockedDirection;
    public bool lockedMovement;
    public Vector2 lastInput;
    Transform target;
    Transform grabbed;
    Transform targetParent;

    protected override void Start()
    {
        base.Start();
        
        anim.SetFloat("input_x", characterOrientation.x);
        anim.SetFloat("input_y", characterOrientation.y);
    }

    protected override void Action ()
	{
        GetObjectInFront();
		rbody.velocity = Vector3.zero;
		//is busy if a blocking animation is playing
		if (!anim.GetBool ("is_busy") && !GameController.control.gamePaused) {
            if (lockedMovement)
                movementDirection = new Vector2(Mathf.Abs(characterOrientation.x) * Input.GetAxisRaw("Horizontal"), Mathf.Abs(characterOrientation.y) * Input.GetAxisRaw("Vertical"));
            else
                movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (lockedDirection == false) {
				//If direction changed
				if (movementDirection != characterOrientation) {
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
            else if (Input.GetKeyDown(GameKeys.L))
            {
                LActions();
            }
            if (movementDirection != Vector2.zero) {
				if (Input.GetKeyDown ("space") && !target) {
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
        GetComponent<Animator>().SetBool("is_busy", true);
    }

    protected override void OnResume()
    {
        GetComponent<Animator>().SetBool("is_busy", false);
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

    private void GetObjectInFront()
    {
        if (!anim.GetBool("is_busy"))
        {
            Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(transform.position.x, transform.position.y + 0.5f) + characterOrientation);
            target = Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(transform.position.x, transform.position.y + 0.5f) + characterOrientation).transform;
            string text = "";
            if (target)
            {
                print(target.transform.gameObject.name);
                if (target.transform.gameObject.tag == "MapObject")
                {
                    text = "";
                }
                else if (target.transform.gameObject.tag == "Grabbable")
                {
                    text = "Grab";
                }
                else if (target.transform.gameObject.GetComponent<Throwable>())
                {
                    text = "Lift";
                }
                else if (target.transform.gameObject.GetComponent<ChestController>())
                {
                    text = "Open";
                }
                else if (target.transform.gameObject.GetComponent<Activable>())
                {
                    text = "Activate";
                }
            }
            else
                text = "Roll";
            GameController.control.guiController.hud.UpdateRButton(text);
        }
    }
 
    private void LActions()
    {
        if (anim.GetBool("is_carrying"))
        {
            Throw();
        }
        else if (target != null)
        {
            if (target.gameObject.tag == "Grabbable")
            {
                Grab();
            }
            else if (target.transform.gameObject.GetComponent<Throwable>() != null)
            {
                Lift();
            }
            //if the object in front is interactible
            else if (target.GetComponent<Conditionable>())
            {
                target.GetComponent<Conditionable>().action();
            }
        }      
    }

    void Grab()
    {
        grabbed = target;
        targetParent = grabbed.parent;

        grabbed.SetParent(transform);

        lockedDirection = true;
        lockedMovement = true;

       
        StartCoroutine(WaitForKeyRelease(GameKeys.L, Release));
    }

    void Release()
    {
        grabbed.SetParent(targetParent);
        lockedDirection = false;
        lockedMovement = false;
    }

    void Lift()
    {
        targetParent = target.parent;

       target.GetComponent<SpriteRenderer>().sortingLayerName = "Overall";
       target.GetComponent<Collider2D>().enabled = false;
       target.SetParent(transform);
       target.localPosition = new Vector3(0, 1, 0);
       grabbed = target;
       anim.SetBool("is_carrying", true);
       anim.SetBool("is_walking", true);
       anim.SetBool("is_busy", true);
    }

    void Throw()
    {
        if (grabbed != null)
        {
            anim.SetTrigger("Throw");
            anim.SetBool("is_carrying", false);

            grabbed.GetComponent<Collider2D>().enabled = true;
            grabbed.GetComponent<Collider2D>().isTrigger = true;
            grabbed.SetParent(targetParent);

            StartCoroutine(grabbed.GetComponent<Throwable>().Propulse(movementDirection));
            grabbed = null;
        }
    }

    IEnumerator WaitForKeyRelease(KeyCode key, Action function)
    {
        while (!Input.GetKeyUp(key))
            yield return null;
        print("released");
        function();
    }
}