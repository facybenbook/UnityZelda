using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPage : MonoBehaviour {
	private List<GameObject> inventory;
	public GameObject inventoryItem;
	public GameObject inventoryBottle;

	// Use this for initialization
	void Start () {
		inventory = new List<GameObject> ();
		for (int i = 0; i < GameController.control.playerStats.inventorySlots.Count; i++) {
			GameObject item = Instantiate (inventoryItem);
			item.transform.SetParent (transform);
			item.GetComponent<RectTransform>().anchoredPosition = new Vector3(-54 + i/4 * 36, 38 + i%4 * -24, 0);
			item.name = "Item" + (i);
			inventory.Add (item);
		}
		for (int i = 0; i < GameController.control.playerStats.bottleSlots.Count; i++) {
			GameObject item = Instantiate (inventoryBottle);
			item.transform.SetParent (transform);
			item.GetComponent<RectTransform>().anchoredPosition = new Vector3(-58 + i * 20, -34, 0);
			item.name = "Bottle" + (i);
			inventory.Add (item);
			print (GameController.control.playerStats.bottleSlots[i].content);
			item.GetComponent<Animator> ().SetInteger ("Content", (int)GameController.control.playerStats.bottleSlots [i].content);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
