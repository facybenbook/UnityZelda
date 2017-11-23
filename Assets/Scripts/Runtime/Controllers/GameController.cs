using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SceneStats
{
    public Tiled2Unity.TiledMap map;
    public List<bool> objectsState;
    public WarpController lastWarp;

    void MapSaveState()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + map.gameObject.name + "_meta" + ".dat");
        GameObject gameObjects = map.transform.Find("GameObjects").gameObject;
        List<GameObject> data = new List<GameObject>();
        foreach (Transform child in gameObjects.transform)
        {
            if (child.gameObject.GetComponent<Conditionable>())
            {
                data.Add(child.gameObject);
            }
        }
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load(Tiled2Unity.TiledMap newMap)
    {
        //save the old state
        MapSaveState();
        map = newMap;
        if (File.Exists(Application.persistentDataPath + map.gameObject.name + "_meta" + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + map.gameObject.name + "_meta" + ".dat", FileMode.Open);
            List<GameObject> data = (List<GameObject>)bf.Deserialize(file);
            file.Close();
            GameObject gameObjects = map.transform.Find("GameObjects").gameObject;
            foreach (GameObject obj in data)
            {
                GameObject tmp;
                tmp = gameObjects.transform.Find(obj.name).gameObject;
                tmp = obj;
            }
        }
    }
}
[Serializable]
public class PlayerStats
{
    //public Scene scene;
    public String name;
    public InventorySlot slotA;
    public InventorySlot slotB;
    public int health;
    public int maxHealth;
    public int rupees;
    public int rupeeLimit;
    public int keys;
    public bool firstOneRupee;
    public bool firstFiveRupee;
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
        inventorySlots = new List<InventorySlot>();

        bottleSlots = new List<InventorySlot>();
    }
    public bool HaveEquipment(string equipment)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.item.name.ToLower() == equipment.ToLower())
                return true;
        }
        foreach (InventorySlot slot in bottleSlots)
        {
            if (slot.item.name.ToLower() == equipment.ToLower())
                return true;
        }
        return false;
    }

    [Serializable]
    public class InventorySlot
    {
        public enum BottleContent : int { None = -1, Empty = 0, Fairy = 1, Water = 2, RedPotion = 3, BluePotion = 4 };

        public int position;
        public InventoryItem item;
        public BottleContent content = BottleContent.Fairy;

        public InventorySlot(InventoryItem item)
        {
            this.item = item;
        }
        
    }

}
public class GameKeys
{
    public const KeyCode A = KeyCode.A;
    public const KeyCode B = KeyCode.B;
    public const KeyCode R = KeyCode.R;
    public const KeyCode L = KeyCode.L;
    public const KeyCode START = KeyCode.Delete;
    public const KeyCode ENTER = KeyCode.Return;
}

public class GameController : MonoBehaviour {
	
	public static GameController control;
	public PlayerStats playerStats;
	public bool gameOverState = false;
	public bool gamePaused = false;
    public SceneStats currentScene;
    GameObject player;
    public CameraController cameraController;
	public GUIController guiController;
	public bool pauseMenu;

    public ZIndex zIndexManager;
    public InventoryItemList itemDatabase;

	//singleton pattern
	void Awake () {
		if (control == null) {
			control = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else if (control != this)
			Destroy (gameObject);
        player = GameObject.Find("Player");
        cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
        guiController = GameObject.Find("GUI").GetComponent<GUIController>();
        zIndexManager = GameObject.Find("ZIndexManager").GetComponent<ZIndex>();
		//globally set the FPS to 60 maximum;
		Application.targetFrameRate = 60;
        currentScene = new SceneStats();
        Screen.SetResolution(240, 160, false);
    }
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

    public GameObject LoadMap(GameObject map)
    {
        GameObject go = Instantiate(map);


        return go;
    }

	public void GameOver()	{
		gameOverState = true;
	}
	public void PauseGame() {
		gamePaused = true;
		foreach (Animator go in FindObjectsOfType<Animator>()) {
			if (go.tag != "GUI") 
			{
				go.SendMessage ("OnPause", SendMessageOptions.DontRequireReceiver);
                go.GetComponent<Animator>().speed = 0;
			}
		}
	}
	public void ResumeGame() {
		gamePaused = false;
        foreach (Animator go in FindObjectsOfType<Animator>())
        {
            if (go.tag != "GUI")
            {
				go.GetComponent<Animator> ().speed = 1;
				go.SendMessage ("OnResume", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public IEnumerator DisplayMessage(string text)
	{
        PauseGame();
		yield return guiController.hud.messageBox.DisplayMessage (text);
        ResumeGame();
	}
	public IEnumerator NewHeart()
	{
		bool pausedEnter = gamePaused;
		if (gamePaused == false)
		PauseGame ();
		//if true, NewHeartPiece has been calling the method
		if (pausedEnter == false)
			yield return DisplayMessage ("You got a new heart container, you're harder, better, faster, stronger !");
		playerStats.maxHealth += 4;
		playerStats.health = playerStats.maxHealth;
        guiController.hud.UpdateLife();
		ResumeGame ();
	}
	public IEnumerator NewHeartPiece()
	{
		PauseGame ();
		playerStats.containerPieces++;
		if (playerStats.containerPieces == 4) {
			yield return DisplayMessage("You found 4 heart pieces, the 4 pieces make a whole heart container !");
			yield return NewHeart();
			playerStats.containerPieces = 0;
		}
		else
			yield return DisplayMessage("You found a heart piece, you now have " + playerStats.containerPieces + " , gather 4 of them to get a whole heart container.");
		if (gamePaused == true)
			ResumeGame ();
    }
	public IEnumerator AddRupees(int amount, bool isDrop)
    {
        if (amount != 0)
        {
            if (isDrop && amount == 1 && playerStats.firstOneRupee == false)
            {
                yield return DisplayMessage("Vous avez obtenu un rubis, c'est le début de la richesse !");
                playerStats.firstOneRupee = true;
            }
            else if (isDrop && amount == 5 && playerStats.firstFiveRupee == false)
            {
                yield return DisplayMessage("Vous avez obtenu 5 rubis, c'est pas mal !");
                playerStats.firstFiveRupee = true;
            }
            if ((playerStats.rupees + amount) < 0)
            {
                playerStats.rupees = 0;
            }
            else if (playerStats.rupees + amount > playerStats.rupeeLimit)
            {
                playerStats.rupees = playerStats.rupeeLimit;
            }
            else
                playerStats.rupees += amount;
            guiController.hud.UpdateRupees();
        }
    }

    public void GetItem(Collectible item)
    {
        if (item != null) { 
        if (item.GetComponent<Collectible>().BigItem == true)
            player.GetComponent<PlayerController>().anim.SetTrigger("BigItemGot");
        else
            player.GetComponent<PlayerController>().anim.SetTrigger("SmallItemGot");
        }
    }

    public void AddToZIndex(GameObject obj)
    {
        zIndexManager.layerZObjects.Add(obj);
    }

    public void Heal(int amount)
	{
		playerStats.health += amount;
		if (playerStats.health > playerStats.maxHealth)
			playerStats.health = playerStats.maxHealth;
        guiController.hud.UpdateLife ();
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
        guiController.hud.UpdateRupees ();
		ResumeGame ();
	}
    
}
