using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour 
{
	public bool state;
	public Sprite on;
	public Sprite off;

	//stores the state of the trigger, used to know if a change happens
	private bool currentState;

	// Use this for initialization
	void Start ()
	{
		currentState = false;
	}

	public void ChangeState(bool state)
	{
		if (state != currentState) 
		{
			if (state == true)
			{
				GetComponent<SpriteRenderer> ().sprite = on;
			} 
			else 
			{
				GetComponent<SpriteRenderer> ().sprite = off;
			}
			currentState = state;
			GetComponent<AudioSource>().Play();
		}
	}


}
