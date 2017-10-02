using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
	public bool pauseMenu;

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
		gameGUI = GameObject.Find("GUI");
		if (gameGUI)
			hudController = gameGUI.transform.Find("HUD").GetComponent<HUDController>();
		objects = (GameObject[])FindObjectsOfType (typeof(GameObject));
		//globally set the FPS to 60 maximum;
		Application.targetFrameRate = 60;
	}
	// Update is called once per frame
	void Update () {
		if (playerStats.health <= 0)
			GameOver ();
		if (Input.GetKeyDown (KeyCode.Delete))
		{
			if (gamePaused == false) {
				pauseMenu = true;
				PauseGame ();
			} else 
			{
				pauseMenu = false;
				ResumeGame ();
			}
		}
	}

	void OnGUI ()
	{
		if (GUI.Button (new Rect ( 10, 10,  50, 15), "health up"))
			Heal (1);
		if (GUI.Button (new Rect ( 10, 40,  50, 15), "health down"))
			Heal (-1);
		if (GUI.Button (new Rect (110, 10,  50, 15), "maxhealth up"))
			NewHeart ();
		if (GUI.Button (new Rect ( 10, 70,  50, 15), "save1"))
			Save(1);
		if (GUI.Button (new Rect ( 10, 100, 50, 15), "save2"))
			Save(2);
		if (GUI.Button (new Rect ( 10, 130, 50, 15), "save3"))
			Save(3);
		if (GUI.Button (new Rect ( 10, 160, 50, 15), "load"))
			Load(1);
	}

	public void Save(int saveNumber) {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/save"+ saveNumber +".dat");
		PlayerStats data = playerStats;
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
			playerStats = data;
		}
	}
	public void CameraFocus(GameObject target, bool transitionToTarget) {
		
	}
	public void GameOver()	{
		gameOverState = true;
	}
	public void PauseGame() {
		gamePaused = true;
		foreach (GameObject go in objects) {
			if (go != null)
			{
				if (go.tag != "GUI") 
				{
					if (go.GetComponent<Animator> () != null) 
					{
						if (go.tag == "Player")
							go.GetComponent<Animator> ().SetTrigger ("stop_action");
						go.GetComponent<Animator> ().speed = 0;
						go.SendMessage ("OnPause", SendMessageOptions.DontRequireReceiver);
					}
				}
			}
		}
	}
	public void ResumeGame() {
		gamePaused = false;
		foreach (GameObject go in objects) {
			//don't affect the GUI when pausing
			if (go != null) {
				if (go.tag != "GUI") {				
					if (go.GetComponent<Animator> () != null)
						go.GetComponent<Animator> ().speed = 1;
					go.SendMessage ("OnResume", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	void WaitForNextFrame()
	{
		float timer = 0;
		while(timer < 1/60f)
		{
			timer += Time.deltaTime;
		}
	}

	public IEnumerator DisplayMessage(string text)
	{
		yield return hudController.messageBox.DisplayMessage (text);
	}
	public IEnumerator NewHeart()
	{
		bool pausedEnter = gamePaused;
		if (gamePaused == false)
		PauseGame ();
		//if true, NewHeartPiece has been calling the method
		if (pausedEnter == false)
			yield return DisplayMessage ("Vous avez obtenu un nouveau receptacle de coeur !");
		playerStats.maxHealth += 4;
		playerStats.health = playerStats.maxHealth;
		if (hudController)
			hudController.UpdateLife();
		ResumeGame ();
	}
	public IEnumerator NewHeartPiece()
	{
		PauseGame ();
		playerStats.containerPieces++;
		if (playerStats.containerPieces == 4) {
			yield return DisplayMessage("Vous avez obtenu un nouveau quart de coeur, 4 quarts de coeur forment un nouveau receptacle !");
			yield return NewHeart();
			playerStats.containerPieces = 0;
		}
		else
			yield return DisplayMessage("Vous avez obtenu un nouveau quart de coeur, vous en avez maintenant " + playerStats.containerPieces + " rassemblez en 4 pour obtenir un receptacle entier.");
		if (gamePaused == true)
			ResumeGame ();
	}
	public void Heal(int amount)
	{
		playerStats.health += amount;
		if (playerStats.health > playerStats.maxHealth)
			playerStats.health = playerStats.maxHealth;
		if (hudController)
			hudController.UpdateLife ();
	}
//	static public void Hurt(int damage, Vector3 positionToEscape)
//	{
//		if (damage > 0)
//		{
//			playerStats.health -= damage;
//			if (playerStats.health < 0)
//				playerStats.health = 0;
//			//movement to escape hit && lock animator
//			GetComponent<Animator> ().SetBool ("is_busy", true);
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
	public IEnumerator AddRupees(int amount, bool isDrop) {
		if (amount != 0)
		{
			if (isDrop && amount == 1 && firstOneRupee == false) {
				yield return DisplayMessage ("Vous avez obtenu un rubis, c'est le début de la richesse !");
				firstOneRupee = true;
			}
			else if (isDrop && amount == 5 && firstFiveRupee == false) {
				yield return DisplayMessage ("Vous avez obtenu 5 rubis, c'est pas mal !");
				firstFiveRupee = true;
			}
			if ((playerStats.rupees + amount) < 0) {
				playerStats.rupees = 0;
			} else if (playerStats.rupees + amount > playerStats.rupeeLimit) {
				playerStats.rupees = playerStats.rupeeLimit;
			} else
				playerStats.rupees += amount;
			hudController.UpdateRupees ();
		}
	}
	public void ChangeRupeeStash(string size)
	{
		PauseGame ();
		switch(size)
		{
		case "S":
			{
				DisplayMessage ("Vous avez obtenu la <color=#f85030>petite bourse</color> !");

				if (playerStats.rupeeLimit < 150) {
					DisplayMessage ("Vous pouvez mainenant transporter jusqu'à 150 rubis !");
					playerStats.rupeeLimit = 150;
				} else {
					DisplayMessage ("Mais vous avez deja une plus grande bourse.");
				}
				break;
			}
		case "M":
			{
				DisplayMessage ("Vous avez obtenu la <color=#f85030>moyenne bourse</color>, transportez jusqu'à 300 rubis !");
				if (playerStats.rupeeLimit < 300) {
					DisplayMessage ("Vous pouvez mainenant transporter jusqu'à 300 rubis !");
					playerStats.rupeeLimit = 300;
				}
				else
					DisplayMessage ("Mais vous avez deja une plus grande bourse.");
				break;
			}
		case "L":
			{
				DisplayMessage ("Vous avez obtenu la <color=#f85030>grande bourse</color>, transportez jusqu'à 500 rubis !");
				if (playerStats.rupeeLimit < 600) {
					DisplayMessage ("Vous pouvez mainenant transporter jusqu'à 600 rubis !");
					playerStats.rupeeLimit = 600;
				} 
				else
					DisplayMessage ("Mais vous avez deja une plus grande bourse.");
				break;
			}
		case "XL":
			{
				DisplayMessage ("Vous avez obtenu la <color=#f85030>Super bourse</color>, vous pouvez maintenant transporter jusqu'à 999 rubis !");
				if (playerStats.rupeeLimit < 999) {
					DisplayMessage ("Vous pouvez mainenant transporter jusqu'à 999 rubis !");
					playerStats.rupeeLimit = 999;
				}
				else
					DisplayMessage ("Mais vous avez deja une plus grande bourse.");
				break;
			}
		}
		hudController.UpdateRupees ();
		ResumeGame ();
	}
}
[Serializable]
public class PlayerStats {
	//public Scene scene;
	public enum Equipments {None, Sword, Bow, MoleClaws, Bottle};
	public String name;
	public Equipments slotA = Equipments.Sword;
	public Equipments slotB;
	public int health;
	public int maxHealth;
	public int rupees;
	public int rupeeLimit;
	public int keys;
	public bool bossKey;
	public bool[] elements;
	public int containerPieces;
	public List<InventorySlot> inventorySlots;
	public List<InventorySlot> bottleSlots;

	public PlayerStats()
	{
		name = "Link";
		containerPieces = 0;
		health = 12;
		maxHealth = 12;
		rupees = 0;
		rupeeLimit = 100;
		keys = 0;
		bossKey = false;
		elements = new bool[8];
		slotA = Equipments.None;
		slotB = Equipments.None;
		inventorySlots = new List<InventorySlot>();
		inventorySlots.Add (new InventorySlot (0, Equipments.Sword));

		bottleSlots = new List<InventorySlot>();
		bottleSlots.Add (new InventorySlot (0, Equipments.Bottle));
	}

	public class InventorySlot {
		public enum BottleContent: int {None = -1, Empty = 0, Fairy = 1, Water = 2, RedPotion = 3, BluePotion = 4};

		public int position;
		public Equipments item;
		public BottleContent content = BottleContent.Fairy;

		public InventorySlot(int position, Equipments item)
		{
			this.position = position;
			this.item = item;
			if (item == Equipments.Bottle)
				content = BottleContent.Fairy;
		}
	}

}
public class GameKeys
{
	public const KeyCode A = KeyCode.I;
	public const KeyCode B = KeyCode.O;
	public const KeyCode R = KeyCode.L;
	public const KeyCode L = KeyCode.R;
	public const KeyCode START = KeyCode.Delete;
	public const KeyCode ENTER = KeyCode.Return;
}