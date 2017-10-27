using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthBarScript : MonoBehaviour {

	public GameObject heart;
	public Sprite[] heartSprites = new Sprite[6];
	private int health = 0;
	private int maxHealth = 0;
	private List<GameObject> hearts;
	private int heartsLimit = 20;
	void Start ()
	{
		hearts = new List<GameObject>();
		for(int i = 0; i < heartsLimit; i++)
		{
			this.AddHeart(i);
			hearts[i].SetActive (false);
		}
	}
	public void UpdateInfos (PlayerStats infos)
	{
		//update life state
		if (health != infos.health || maxHealth != infos.maxHealth) {
			if (maxHealth != infos.maxHealth) {
				for (int i = 0; i < infos.maxHealth / 4; i++) {
					hearts [i].SetActive (true);
				}
				for (int i = infos.maxHealth / 4; i < heartsLimit; i++) {
					hearts [i].SetActive (false);
				}
			}
				//fill all the full hearts
				for (int i = 0; i < infos.health / 4; i++) {
					hearts [i].GetComponent<Image> ().sprite = heartSprites [0];
				}
				switch (infos.health % 4) {
				case 3:
					{
						hearts [infos.health / 4].GetComponent<Image> ().sprite = heartSprites [1];
						break;
					}
				case 2:
					{
						hearts [infos.health / 4].GetComponent<Image> ().sprite = heartSprites [2];
						break;
					}
				case 1:
					{
						hearts [infos.health / 4].GetComponent<Image> ().sprite = heartSprites [3];
						break;
					}
				case 0:
					{
						hearts [infos.health / 4].GetComponent<Image> ().sprite = heartSprites [4];
						break;
					}
				}
				for (int i = infos.health / 4 + 1; i < infos.maxHealth / 4; i++) {
					hearts [i].GetComponent<Image> ().sprite = heartSprites [4];
				}
			}
			health = infos.health;
			maxHealth = infos.maxHealth;
	}

	private void AddHeart(int heartNumber)
	{
		GameObject newHeart = GameObject.Instantiate(heart);
		hearts.Add(newHeart);
		newHeart.transform.SetParent(this.transform);
		newHeart.name = "Heart" + heartNumber;
		newHeart.GetComponent<RectTransform>().anchoredPosition = new Vector3(heartNumber % 10 * 16, heartNumber / 10 * -16, 0f);
		newHeart.GetComponent<Image>().sprite = heartSprites[0];
		if(heartNumber > 0)
			hearts[heartNumber - 1].GetComponent<Image>().sprite = heartSprites[0];
	}
}
