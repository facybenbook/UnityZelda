using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour 
{
	public bool state;
	public Sprite on;
	public Sprite off;

	//stores the state of the trigger, used to know if a change happens

	// Use this for initialization
	void Start ()
	{
		state = false;
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
			this.state = state;
			GetComponent<AudioSource>().Play();
			foreach (Activable obj in FindObjectsOfType<Activable>()) {
				obj.gameObject.SendMessage ("OnCheckConditions", SendMessageOptions.DontRequireReceiver);
			}
		}
	}


}
