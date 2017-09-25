using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameController : MonoBehaviour {
	
	public static GameController control;
	public PlayerStats playerStats;
	public bool gameOverState = false;
	public bool gamePaused = false;
	//GameObject player;
	//GameObject mainCamera;
	GameObject gameGUI;
	public HUDController hudController;
	public bool firstOneRupee;
	public bool firstFiveRupee;
	public  GameObject[] objects;

	//singleton pattern
	void Awake () {
		if (control == null) {
			control = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else if (control != this)
			Destroy (gameObject);
		//player = GameObject.Find("Player");
		//mainCamera = GameObject.Find("MainCamera");
		gameGUI = GameObject.Find("HUD");
		if (gameGUI)
			hudController = gameGUI.GetComponent<HUDController>();
		objects = (GameObject[])FindObjectsOfType (typeof(GameObject));
	}
	void OnGUI ()
	{
		if (GUI.Button (new Rect (10, 10, 100, 30), "health up"))
			GameController.control.Heal (1);
		if (GUI.Button (new Rect (10, 40, 100, 30), "health down"))
			GameController.control.Heal (-1);
		if (GUI.Button (new Rect (110, 10, 100, 30), "maxhealth up"))
			GameController.control.NewHeart ();
		if (GUI.Button (new Rect (10, 70, 100, 30), "save1"))
			GameController.control.Save(1);
		if (GUI.Button (new Rect (10, 100, 100, 30), "save2"))
			GameController.control.Save(2);
		if (GUI.Button (new Rect (10, 130, 100, 30), "save3"))
			GameController.control.Save(3);
		if (GUI.Button (new Rect (10, 160, 100, 30), "load"))
			GameController.control.Load(1);
	}

	// Update is called once per frame
	void Update () {
		if (playerStats.health <= 0)
			GameOver ();
	}
	public void Save(int saveNumber) {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/save"+ saveNumber +".dat");
		PlayerStats data = control.playerStats;
		bf.Serialize (file, data);
		file.Close ();

	}
	public void Load(int saveNumber) {
		if (File.Exists (Application.persistentDataPath + "/save"+ saveNumber +".dat")) 
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/save"+ saveNumber +".dat", FileMode.Open);
			PlayerStats data = (PlayerStats)bf.Deserialize (file);
			file.Close ();
			control.playerStats = data;
		}
	}
	public void CameraFocus(GameObject target, bool transitionToTarget) {
		
	}
	public void GameOver()	{
		control.gameOverState = true;
	}
	public void PauseGame() {
		control.gamePaused = true;
		foreach (GameObject go in objects) {
			if (go.tag != "GUI") {
				if (go != null) {
					if (go.GetComponent<Animator> () != null)
						go.GetComponent<Animator> ().speed = 0;
					go.SendMessage ("OnPause", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
	public void ResumeGame() {
		control.gamePaused = false;
		foreach (GameObject go in objects) {
			//don't affect the GUI when pausing
			if (go.tag != "GUI") {
				if (go != null) {
					if (go.GetComponent<Animator> () != null)
						go.GetComponent<Animator> ().speed = 1;
					go.SendMessage ("OnResume", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	public void DisplayMessage(string text)
	{
		hudController.DisplayMessage (text);
	}
	public void NewHeart()
	{
		playerStats.maxHealth += 4;
		playerStats.health = playerStats.maxHealth;
		if (gameGUI)
			hudController.UpdateLife();
	}
	public void Heal(int amount)
	{
		playerStats.health += amount;
		if (playerStats.health > playerStats.maxHealth)
			playerStats.health = playerStats.maxHealth;
		if (control.hudController)
			control.hudController.UpdateLife ();
	}
//	static public void Hurt(int damage, Vector3 positionToEscape)
//	{
//		if (damage > 0)
//		{
//			control.playerStats.health -= damage;
//			if (control.playerStats.health < 0)
//				control.playerStats.health = 0;
//			//movement to escape hit && lock animator
//			control.GetComponent<Animator> ().SetBool ("is_busy", true);
//			GetComponent<Animator> ().SetBool ("is_hurt", true);
//			//sets the objects movement vector to escape
//			positionToEscape = this.transform.position - positionToEscape;
//			Vector2 direction = new Vector2 (positionToEscape.x, positionToEscape.y).normalized;
//			GetComponent<Animator> ().SetFloat ("input_x", -direction.x);
//			GetComponent<Animator> ().SetFloat ("input_y", -direction.y);
//			this.gameObject.GetComponent<PlayerMovement> ().Jump(positionToEscape, 2);
//			audioSource.PlayOneShot (audioClips[5]);
//			if (playerStats.health == 0) {
//				GetComponent<Animator> ().SetBool ("is_dead", true);
//				audioSource.PlayOneShot (audioClips[4]);
//				gameOver();
//			}
//		}
//	}
	public void AddRupees(int amount) {
		if (amount != 0)
		{
			if ((playerStats.rupees + amount) < 0) {
				playerStats.rupees = 0;
			} else if (playerStats.rupees + amount > playerStats.rupeeLimit) {
				playerStats.rupees = playerStats.rupeeLimit;
			} else
				playerStats.rupees += amount;
			control.hudController.UpdateRupees ();
		}
	}
	public void ChangeRupeeStash(string size)
	{
		switch(size)
		{
		case "S":
			{
				control.hudController.DisplayMessage ("Vous avez obtenu la <color=#f85030>petite bourse</color>, transportez jusqu'à 100 rubis !");
				if(control.playerStats.rupeeLimit < 150)
					control.playerStats.rupeeLimit = 150;
				else
					control.hudController.DisplayMessage ("Mais vous avez deja une plus grande bourse.");
				break;
			}
		case "M":
			{
				control.hudController.DisplayMessage ("Vous avez obtenu la <color=#f85030>moyenne bourse</color>, transportez jusqu'à 300 rubis !");

				if(control.playerStats.rupeeLimit < 300)
					control.playerStats.rupeeLimit = 300;
				else
					control.hudController.DisplayMessage ("Mais vous avez deja une plus grande bourse.");

				break;
			}
		case "L":
			{
				control.hudController.DisplayMessage ("Vous avez obtenu la <color=#f85030>grande bourse</color>, transportez jusqu'à 500 rubis !");
				if(control.playerStats.rupeeLimit < 600)
					control.playerStats.rupeeLimit = 600;
				else
					control.hudController.DisplayMessage ("Mais vous avez deja une plus grande bourse.");
				break;
			}
		case "XL":
			{
				control.hudController.DisplayMessage ("Vous avez obtenu la <color=#f85030>Super bourse</color>, vous pouvez maintenant transporter jusqu'à 999 rubis !");
				if(control.playerStats.rupeeLimit < 999)
					control.playerStats.rupeeLimit = 999;
				else
					control.hudController.DisplayMessage ("Mais vous avez deja une plus grande bourse.");
				break;
			}
		}
		control.hudController.UpdateRupees ();
	}
}
[Serializable]
public class PlayerStats {
	public String name;
	//public Scene scene;
	public enum Equipments {Default, Sword, Bow, MoleClaws};
	public Equipments slotA = Equipments.Sword;
	public Equipments slotB;
	public int health = 10;
	public int maxHealth = 12;
	public int rupees = 0;
	public int rupeeLimit = 100;
	public int keys = 0;
	public bool bossKey = false;
	public bool[] elements = new bool[8];
}