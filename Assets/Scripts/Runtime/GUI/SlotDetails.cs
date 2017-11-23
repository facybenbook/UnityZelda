using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SlotDetails : MonoBehaviour {

	public GameObject heartsBar;
	public GameObject elementsBar;
	public GameObject playerNameLabel;
	PlayerStats detailsSlot1;
	PlayerStats detailsSlot2;
	PlayerStats detailsSlot3;
	public GameObject[] slotsTexts = new GameObject[3];
	// Use this for initialization
	void Start () {
		elementsBar = transform.Find("ElementsBar").gameObject;
		heartsBar = transform.Find("HeartsBar").gameObject;
		playerNameLabel = transform.Find("PlayerNameLabel").gameObject;
		if (playerNameLabel)
			playerNameLabel.GetComponent<Text> ().text = "";
		slotsTexts[0] = GameObject.Find("Slot1/Button/Text").gameObject;
		slotsTexts[1] = GameObject.Find("Slot2/Button/Text").gameObject;
		slotsTexts[2] = GameObject.Find("Slot3/Button/Text").gameObject;
		GameController.control.Load(1);
		detailsSlot1 = GameController.control.playerStats;
		GameController.control.Load(2);
		detailsSlot2 = GameController.control.playerStats;
		GameController.control.Load (3);
		detailsSlot3 = GameController.control.playerStats;
		slotsTexts [0].GetComponent<Text> ().text = detailsSlot1.name;
		slotsTexts [1].GetComponent<Text> ().text = detailsSlot2.name;
		slotsTexts [2].GetComponent<Text> ().text = detailsSlot3.name;
	}

	public void SaveSlotInfos (int index)
	{
		PlayerStats displayed;
		switch (index) {
		case 1:
			displayed = detailsSlot1;
			break;
		case 2:
			displayed = detailsSlot2;
			break;
		case 3:
			displayed = detailsSlot3;
			break;
		default:
			print ("Error in save slot index");
			displayed = null;
			break;
		}
		heartsBar.GetComponent<HealthBar> ().UpdateInfos (displayed);
		elementsBar.GetComponent<ElementsBar> ().UpdateInfos (displayed);
		playerNameLabel.GetComponent<Text>().text = displayed.name;
	}
}
