using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Conditionable {
    
    public bool startsVisible = true;
    public bool staysOn = true;
    public Sprite on;
	Sprite off;
    
    protected override void Start()
    {
        base.Start();
        off = GetComponent<SpriteRenderer>().sprite;
        if (startsVisible == false)
        {
            ToggleVisible();
            action = ToggleVisible;
        }
    }

    public override void OnCheckConditions()
    {
        if (startsVisible == false)
        {
            base.OnCheckConditions();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("enter");
        if (state == false)
        {
            if (collision.isTrigger == false)
            {
                ChangeState(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        print("exit");
        //return to inactive state if something is not 
        if (staysOn == false && collision.isTrigger == false)
        {
            print("statechanged");
            ChangeState(false);
        }
    }

    public void ChangeState(bool state)
	{
        this.state = state;
		if (state == true)
		{
			GetComponent<SpriteRenderer> ().sprite = on;
		} 
		else 
		{
			GetComponent<SpriteRenderer> ().sprite = off;
		}
		GetComponent<AudioSource>().Play();
		foreach (Activable obj in FindObjectsOfType<Activable>()) {
			obj.gameObject.SendMessage ("OnCheckConditions", SendMessageOptions.DontRequireReceiver);
		}
	}

    void ToggleVisible()
    {
        GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
        GetComponent<BoxCollider2D>().enabled = !GetComponent<BoxCollider2D>().enabled;
        action = null;
    }
}
