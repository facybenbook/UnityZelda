using UnityEngine;

public class PressurePlate : HideableMechanism {

    protected bool pressed = false;
    public bool staysOn = true;
    public Sprite onSprite;
	public Sprite offSprite;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //DEBUG
        print("collision");
        //if is not already activated and collides with a physical object
        if (pressed == false && collision.isTrigger == false)
        {
            //DEBUG
            print("enter");
            SetPressed(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //DEBUG
        print("exiting");
        //if the state does not persist when not pressed and the object leaving is physical
        if (pressed == true && staysOn == false && collision.isTrigger == false)
        {
            //DEBUG
            print("statechanged");
            SetPressed(false);
        }
    }

    public void SetPressed(bool isPressed)
    {
        //if not already pressed or unpressed
        if (isPressed != pressed)
        {
            pressed = isPressed;
            if (pressed == true)
            {
                if (onSprite)
                    GetComponent<SpriteRenderer>().sprite = onSprite;
                else
                    print("error");
            }
            else
            {
                if (offSprite)
                    GetComponent<SpriteRenderer>().sprite = offSprite;
                else
                    print("error");
            }
            GetComponent<AudioSource>().Play();
            foreach (LogicMechanism mech in FindObjectsOfType<LogicMechanism>())
            {
                mech.gameObject.SendMessage("CheckActivationConditions", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
