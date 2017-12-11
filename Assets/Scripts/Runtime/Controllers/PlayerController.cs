using UnityEngine;
using System.Collections;
using System;

public class PlayerController : CharactersController
{
    public Vector2 lastInput;
    Transform target;
    Transform grabbed;
    Transform targetParent;
    private bool automatedAction;

    protected override void Start()
    {
        base.Start();
        automatedAction = false;
        anim.SetFloat("input_x", CharacterOrientation.x);
        anim.SetFloat("input_y", CharacterOrientation.y);
    }

    protected override void Action()
    {
        if (!automatedAction && !anim.GetBool("stop_action"))
        {
            //update the label for the R button action
            GetObjectInFront();
            rbody.velocity = Vector3.zero;
            //is busy if a blocking animation is playing
            if (!GameController.control.gamePaused && !anim.GetBool("is_busy"))
            {
                //get the axis
                UpdateMovementDirection();
                UpdateOrientation();
                if (Input.GetKeyDown(GameKeys.A))
                {
                    EquipmentInSlot(GameController.control.playerStats.slotA);
                }
                else if (Input.GetKeyDown(GameKeys.B))
                {
                    EquipmentInSlot(GameController.control.playerStats.slotB);
                }
                else if (Input.GetKeyDown(GameKeys.R))
                {
                    RActions();
                }
                //if the player moves by input
                else if (movementDirection != Vector2.zero)
                {
                    //walk
                    anim.SetBool("is_walking", true);
                    Move(1);
                }
                else
                {
                    //idle
                    anim.SetBool("is_walking", false);
                }
            }
        }
    }

    void UpdateMovementDirection()
    {
        movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (lockedMovement)
        {
            //just in case
            if (lastInput == Vector2.zero)
                lastInput = movementDirection;
            movementDirection = new Vector2(lastInput.x == 0 ? 0 : movementDirection.x, lastInput.y == 0 ? 0 : movementDirection.y);
        }
    }

    public IEnumerator Walk(float tiles)
    {
        GameController.control.PauseGame();
        automatedAction = true;
        anim.SetBool("is_walking", true);
        while (tiles >= 0)
        {
            Move(1);
            tiles -= 0.625f * transform.lossyScale.x * pixelPerFrameSpeed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        anim.SetBool("is_walking", false);
        automatedAction = false;
        GameController.control.ResumeGame();
    }

    void UpdateOrientation()
    {
        if (lockedOrientation == false)
        {
            //If direction changed
            if (movementDirection != CharacterOrientation)
            {
                //If a key is pressed
                if (movementDirection != Vector2.zero)
                {
                    if (movementDirection.y == 0)
                    {
                        characterOrientation.x = movementDirection.x;
                        if (movementDirection.x != 0)
                            characterOrientation.y = 0;
                    }
                    else if (movementDirection.x == 0)
                    {
                        characterOrientation.y = movementDirection.y;
                        if (movementDirection.y != 0)
                            characterOrientation.x = 0;
                    }
                    anim.SetFloat("input_x", CharacterOrientation.x);
                    anim.SetFloat("input_y", CharacterOrientation.y);
                    lastInput = movementDirection;
                }
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
        
        if (CharacterOrientation == Vector2.left)
        {
            arrow.transform.position = transform.position + new Vector3(-0.5f, 0.28125f, 0);
            arrow.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (CharacterOrientation == Vector2.right)
        {
            arrow.transform.position = transform.position + new Vector3(0.5625f, 0.40625f, 0);
            arrow.transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (CharacterOrientation == Vector2.up)
        {
            arrow.transform.position = transform.position + new Vector3(-0.09375f, 1, 0);
            arrow.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (CharacterOrientation == Vector2.down)
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
        if (GameController.control.playerStats.HaveEquipment("Remote Bomb"))
            bomb = Instantiate(Resources.Load("Prefabs/RemoteBomb") as GameObject);
        else
            bomb = Instantiate(Resources.Load("Prefabs/Bomb") as GameObject);
        if (CharacterOrientation == Vector2.right)
            bomb.transform.position = transform.position + new Vector3(+0.75f, 0.25f, 0);
        else if (CharacterOrientation == Vector2.left)
            bomb.transform.position = transform.position + new Vector3(-0.75f, 0.25f, 0);
        else if (CharacterOrientation == Vector2.down)
            bomb.transform.position = transform.position + new Vector3(0, -0.25f, 0);
        else if (CharacterOrientation == Vector2.up)
            bomb.transform.position = transform.position + new Vector3(0, 0.75f, 0);
        bomb.transform.rotation = Quaternion.Euler(0, 0, 0);
        GameController.control.AddToZIndex(bomb);
    }

    /// <summary>
    /// performs actions according to the item passed in parameter
    /// </summary>
    /// <param name="item"></param>
    private void EquipmentInSlot(InventorySlot slot)
    {
        switch (slot.item.name)
        {
            case "sword":
                    anim.SetBool("is_sword", true);
                    return;
            case "bow":
                    anim.SetBool("is_bow", true);
                    return;
            case "mole claws":
                    anim.SetTrigger("is_digging");
                    return;
            case "bomb":
                    CreateBomb();
                    return;
            case "remote bomb":
                    CreateBomb();
                    return;
        }
    }

    private void GetObjectInFront()
    {
        if (!anim.GetBool("is_busy"))
        {
            Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(transform.position.x, transform.position.y + 0.5f) + CharacterOrientation);
            target = Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(transform.position.x, transform.position.y + 0.5f) + CharacterOrientation).transform;
            string text = "";
            if (target)
            {
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
                else if (target.GetComponent<ChestController>() && 
                    target.GetComponent<ChestController>().state == false)
                {
                    text = "Open";
                }
                else if (target.GetComponent<Activable>() && 
                    target.GetComponent<Activable>().state == false)
                {
                    text = "Activate";
                }
            }
            else
                text = "Roll";
            GameController.control.guiController.hud.UpdateRButton(text);
        }
    }
 
    private void RActions()
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
            else if (target.GetComponent<Throwable>() != null)
            {
                Lift();
            }
            else if (target.GetComponent<ChestController>() &&
                    target.GetComponent<ChestController>().state == false)
            {
                target.GetComponent<ChestController>().OpenChest();
            }
            else if (target.GetComponent<Activable>() &&
                target.GetComponent<Activable>().state == false)
            {
                target.GetComponent<Activable>().ChangeState(true);
            }
        }
        else if (movementDirection != Vector2.zero)
        {
            //roll
            anim.SetTrigger("is_rolling");
        }
    }

    void Grab()
    {
        grabbed = target;
        targetParent = grabbed.parent;

        grabbed.SetParent(transform);

        lockedOrientation = true;
        lockedMovement = true;
        StartCoroutine(WaitForKeyRelease(GameKeys.R, Release));
    }

    void Release()
    {
        grabbed.SetParent(targetParent);
        lockedOrientation = false;
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

            grabbed.GetComponent<Throwable>().Throw(movementDirection);
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