using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPage : MonoBehaviour {
	private List<GameObject> inventory;
	public GameObject inventoryItem;
	public GameObject inventoryBottle;
	private GameObject cursor;
	private int cursorIndex;
	private int oldCursorIndex;
	private Transform itemList;

	// Use this for initialization
	void Start () {
		itemList = transform.Find ("ItemList");
		cursor = transform.Find("Cursor").gameObject;
		cursor.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (-54, 38, 0);
		cursorIndex = 0;
		oldCursorIndex = 0;
		inventory = new List<GameObject> ();
		for (int i = 0; i < GameController.control.playerStats.inventorySlots.Count; i++) {
			GameObject item = Instantiate (inventoryItem);
			item.transform.SetParent (itemList);
			item.GetComponent<RectTransform>().anchoredPosition = new Vector3(-54 + i/4 * 36, 38.5f + i%4 * -24, 0);
			item.name = "Item" + (i);
			inventory.Add (item);
		}
		for (int i = 0; i < GameController.control.playerStats.bottleSlots.Count; i++) {
			GameObject item = Instantiate (inventoryBottle);
			item.transform.SetParent (transform);
			item.GetComponent<RectTransform>().anchoredPosition = new Vector3(-58 + i * 20, -34, 0);
			item.name = "Bottle" + (i);
			inventory.Add (item);
			item.GetComponent<Animator> ().SetInteger ("Content", (int)GameController.control.playerStats.bottleSlots [i].content);
		}
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			if (cursorIndex <= 11)
				cursorIndex += 4;
		}
		else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			if (cursorIndex >= 4)
				cursorIndex -= 4;
		}
		else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			//if we can move on the left
			if (cursorIndex % 4 > 0)
				cursorIndex -= 1;
		}
		else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			//if we can move on the left
			if (cursorIndex % 4 < 3)
				cursorIndex += 1;
		}
		if (oldCursorIndex != cursorIndex) {
			if (cursorIndex < 12) {
				cursor.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (-54 + cursorIndex % 4 * 36 , 38 + cursorIndex / 4 * -24, 0);
			}
			else
				cursor.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (-58 + cursorIndex % 4 * 20 , -34, 0);
			oldCursorIndex = cursorIndex;
		}
	}
}
