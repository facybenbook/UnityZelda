using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour {
	private bool pausedState;
	// Use this for initialization
	void Start () {
		pausedState = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameController.control.pauseMenu != pausedState) {
			if (GameController.control.pauseMenu == true)
				GetComponent<Animator> ().SetTrigger ("InventoryMenu");
			else
				GetComponent<Animator> ().SetTrigger ("Exit");
			pausedState = GameController.control.pauseMenu;
		}
		if (pausedState)
		if (Input.GetKeyDown (KeyCode.R)) {
			GetComponent<Animator> ().SetTrigger ("Right");
		}
		else if (Input.GetKeyDown (KeyCode.L)) {
			GetComponent<Animator> ().SetTrigger ("Left");
		}
	}
}
