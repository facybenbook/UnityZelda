using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Conditionable {
	
	public Sprite on;
	Sprite off;
    
	// Use this for initialization
	void Start ()
	{
        off = GetComponent<SpriteRenderer>().sprite;
		state = false;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ChangeState(true);
        }
    }

    public void ChangeState(bool state)
	{
		if (state != this.state)
		{
			if (state == true)
			{
				GetComponent<SpriteRenderer> ().sprite = on;
			} 
			else 
			{
				GetComponent<SpriteRenderer> ().sprite = off;
			}
			this.state = state;
			GetComponent<AudioSource>().Play();
			foreach (Activable obj in FindObjectsOfType<Activable>()) {
				obj.gameObject.SendMessage ("OnCheckConditions", SendMessageOptions.DontRequireReceiver);
			}
		}
	}


}
