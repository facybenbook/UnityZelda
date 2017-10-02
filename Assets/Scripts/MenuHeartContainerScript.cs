using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHeartContainerScript : MonoBehaviour {
	int pieces;
	public Sprite[] images = new Sprite[4];
	private Text text;
	// Use this for initialization
	void Start () {
		pieces = 0;
		text = transform.Find("Text").gameObject.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (pieces != GameController.control.playerStats.containerPieces) {
			GetComponent<Image> ().sprite = images [GameController.control.playerStats.containerPieces];
			pieces = GameController.control.playerStats.containerPieces;
			text.text = GameController.control.playerStats.containerPieces + "/4";
		}
	}
}
