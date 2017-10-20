using UnityEngine;
using System.Collections;
using System;

public class PlayerController : CharactersController
{
    public bool lockedDirection;
    private bool lockedMovement;
    public Vector2 lastInput;
    Transform target;
    Transform grabbed;
    Transform targetParent;
    
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
        else if (target)
        {
            if (target.gameObject.tag == "Grabbable")
            {
                Grab();
            }
            else if (target.transform.gameObject.GetComponent<Throwable>() != null)
            {
                Lift();
            }
        }
    }

    void Grab()
    {
        targetParent = target.parent;

        target.SetParent(transform);
        target.position = transform.position + new Vector3 (characterOrientation.x, characterOrientation.y, 0);

        lockedDirection = true;
        lockedMovement = true;

        grabbed = target;
        StartCoroutine(WaitForKeyRelease(GameKeys.L, Release));
    }

    void Release()
    {
        target.parent = targetParent;
        lockedDirection = false;
        lockedMovement = false;
    }

    void Lift()
    {
        anim.SetBool("is_carrying", true);

        targetParent = target.parent;

       target.GetComponent<SpriteRenderer>().sortingLayerName = "Overall";
       target.GetComponent<Collider2D>().enabled = false;
       target.SetParent(transform);
       target.localPosition = new Vector3(0, 1, 0);
       grabbed = target;
        StartCoroutine(WaitForKeyPress(GameKeys.L, Throw));
    }

    void Throw()
    {
        print(grabbed);
        if (grabbed != null)
        {
            print("throwed");
            anim.SetBool("is_carrying", false);

            grabbed.GetComponent<Collider2D>().enabled = true;
            grabbed.GetComponent<Collider2D>().isTrigger = true;
            grabbed.SetParent(targetParent);

            StartCoroutine(Propulse());
        }
    }

    IEnumerator Propulse()
    {
        Vector3 position;
        Vector2 orientation = characterOrientation;
        float speed = 4;
        float z = 1f;
        Vector2 fallingVector;

        position = grabbed.localPosition;
        if (orientation == Vector2.zero)
            fallingVector = Vector2.down;
        else
        {
            fallingVector = orientation * 6;
            if (fallingVector.y == 0)
                fallingVector.y = -z;
        }
        while (z > 0)
        {
            yield return null;
            grabbed.transform.localPosition += new Vector3(fallingVector.x * speed * Time.deltaTime, fallingVector.y * speed * Time.deltaTime, 0);
            z -= speed * Time.deltaTime;
        }
        grabbed.transform.localPosition = position + new Vector3(fallingVector.x, fallingVector.y, 0);
        grabbed = null;
    }

    IEnumerator WaitForKeyRelease(KeyCode key, Action function)
    {
        while (Input.GetKey(key))
            yield return null;
        function();
    }

    IEnumerator WaitForKeyPress(KeyCode key, Action function)
    {
        yield return new WaitForSeconds(1/60f);
        while (!Input.GetKeyDown(key))
            yield return new WaitForEndOfFrame();
        function();
    }
}