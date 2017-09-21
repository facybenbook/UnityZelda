using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HUDController : MonoBehaviour
{
	public int rupeeLimit;
	public Sprite[] rupeeSprites = new Sprite[4];
	public AudioClip rupeeSound;
	private int rupees;
	private int rupeesToMove;
	private Text rupeesLabel;
	private Image rupeesImage;
	public GameObject lifeBar;
	public GameObject rupeesPanel;
	public GameObject messagePanel;
	public GameObject cursor;
	public bool messageDisplayed;
	private AudioSource GUISource;

	public GameObject heart;
	public Sprite[] heartSprites = new Sprite[6];
	public AudioClip heartSound;
	private int health;
	private int maxHealth;
	private List<GameObject> hearts;

	// Use this for initialization
	void Start ()
	{
		messageDisplayed = false;
		rupeeLimit = GameController.control.playerStats.rupeeLimit;
		rupees = GameController.control.playerStats.rupees;
		rupeesToMove = 0;
		rupeesLabel = transform.FindChild ("RupeesPanel").FindChild ("RupeesLabel").GetComponent<Text> ();
		rupeesImage = transform.FindChild ("RupeesPanel").FindChild ("RupeesImage").GetComponent<Image> ();
		rupeesLabel.text = 0.ToString ("D3");
		rupeesImage.sprite = rupeeSprites [0];
		GUISource = gameObject.AddComponent<AudioSource> ();
		maxHealth = GameController.control.playerStats.maxHealth;
		health = GameController.control.playerStats.health;
		hearts = new List<GameObject> ();
		for (int i = 0; i < maxHealth / 4; i++) {
			this.AddHeart (i);
		}
	}

	void Update ()
	{
		if (health != GameController.control.playerStats.health || maxHealth != GameController.control.playerStats.maxHealth)
			UpdateLife ();
		if (rupeesToMove != 0) {
			if (rupeesToMove > 0) {
				rupees++;
				rupeesToMove--;
			} else if (rupeesToMove < 0) {
				rupees--;
				rupeesToMove++;
			}
			rupeesLabel.text = rupees.ToString ("D3");//Update label
			GUISource.PlayOneShot (rupeeSound);
		}
	}
	// Update is called once per frame
	public void UpdateLife ()
	{
		//update life state
		if (maxHealth != GameController.control.playerStats.maxHealth) {
			maxHealth = GameController.control.playerStats.maxHealth;
			this.AddHeart (maxHealth / 4 - 1);
		}
		if (health > GameController.control.playerStats.health) {
			for (int i = health - 1; i >= GameController.control.playerStats.health; i--) {
				switch (i % 4) {
				case 3:
					{
						hearts [i / 4].GetComponent<Image> ().sprite = heartSprites [1];
						break;
					}
				case 2:
					{
						hearts [i / 4].GetComponent<Image> ().sprite = heartSprites [2];
						break;
					}
				case 1:
					{
						hearts [i / 4].GetComponent<Image> ().sprite = heartSprites [3];
						break;
					}
				case 0:
					{
						if (i != 0) {
							hearts [(i - 1) / 4].GetComponent<Image> ().sprite = heartSprites [0];
						}
						hearts [(i / 4)].GetComponent<Image> ().sprite = heartSprites [4];
						break;
					}
				}
			}
		} else {
			if (health < GameController.control.playerStats.health) {
				for (int i = health - 1; i <= GameController.control.playerStats.health; i++) {
					switch (i % 4) {
					case 3:
						{
							hearts [(i / 4)].GetComponent<Image> ().sprite = heartSprites [1];
							break;
						}
					case 2:
						{
							hearts [i / 4].GetComponent<Image> ().sprite = heartSprites [2];
							break;
						}
					case 1:
						{
							hearts [i / 4].GetComponent<Image> ().sprite = heartSprites [3];
							if (i > 4)
								hearts [(i / 4) - 1].GetComponent<Image> ().sprite = heartSprites [5];
							break;
						}
					case 0:
						{
							if (i > 0)
								hearts [(i - 1) / 4].GetComponent<Image> ().sprite = heartSprites [0];
							break;
						}
					}
					//heartSound.Play ();
				}
			}
		}
		health = GameController.control.playerStats.health;
	}

	void AddHeart (int heartNumber)
	{
		GameObject newHeart = GameObject.Instantiate (heart);
		hearts.Add (newHeart);
		newHeart.transform.SetParent (transform.FindChild ("LifeBar"));
		newHeart.name = "Heart" + heartNumber;
		newHeart.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (heartNumber % 10 * 16, heartNumber / 10 * -16, 0f);
		newHeart.GetComponent<Image> ().sprite = heartSprites [0];
		if (heartNumber > 0)
			hearts [heartNumber - 1].GetComponent<Image> ().sprite = heartSprites [5];
	}

	public void UpdateRupees ()
	{
		if (GameController.control.playerStats.rupees != rupees) {
			rupeesToMove = GameController.control.playerStats.rupees - rupees;
		}
		if (GameController.control.playerStats.rupeeLimit != rupeeLimit) {
			switch (GameController.control.playerStats.rupeeLimit) {
			case 150:
				rupeesImage.GetComponent<Image> ().sprite = rupeeSprites [0];
				break;
			case 300:
				rupeesImage.GetComponent<Image> ().sprite = rupeeSprites [1];
				break;
			case 600:
				rupeesImage.GetComponent<Image> ().sprite = rupeeSprites [2];
				break;
			case 999:
				rupeesImage.GetComponent<Image> ().sprite = rupeeSprites [3];
				break;
			}
		}
	}

	public void DisplayMessage (string text)
	{
		if (messageDisplayed == false)
			StartCoroutine (DisplayOneByOne (text));
	}

	IEnumerator DisplayOneByOne (string message)
	{
		int charsOnLine = 0;
		int lineNumber = 0;
		int maxCharsOnLine = 30;
		GameController.control.PauseGame ();
		messageDisplayed = true;
		cursor.GetComponent<Image> ().enabled = false;
		messagePanel.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, 0, 0);
		Text textGUI = messagePanel.GetComponentInChildren<Text> ();
		textGUI.text = "";
		for (int i = 0; i < message.Length; i++) {
			string wordString = "";
			//if rich text
			if (message [i] == '<') {
				wordString = GetWord (textGUI, message, i);
			}
			//otherwise
			else
				wordString = GetWord (textGUI, message, i);
			i += wordString.Length;
			//insert the word in the GUIText one letter at a time
			for (int j = 0; j < wordString.Length; j++) {
				textGUI.text.Insert (i, wordString [j].ToString());

			}

			//---------------------------WIP---------------
			//add opening tag
			while (message [i] != '>' && i < message.Length) {
				textGUI.text += message [i++];
			}
			textGUI.text += message [i++];
			int wordStart = i;
			//get the enriched word(s) between the > and <
			while (message [i] != '<' && i < message.Length) {
				//grab a word
				//wordString = GetWord (message);
				if (wordString.Length + charsOnLine >= maxCharsOnLine) {
					textGUI.text += '\n';
					message.Insert (i, "\n");
				}
				for (int j = 0; j < wordString.Length; j++)
					textGUI.text += ' ';
				i += wordString.Length;
				charsOnLine += wordString.Length;
			}
			//add spaces for the word to be inserted
			for (int j = 0; j < wordString.Length; i++)
				message += ' ';
			charsOnLine += wordString.Length;
			//add closing tag
			while (message [i] != '>' && i < message.Length)
				textGUI.text += message [i++];
			textGUI.text += message [i++];
			//adding the word between tags one letter by one
			char[] strBuffer = textGUI.text.ToCharArray ();
			for (int j = 0; j < wordString.Length; j++) {
				strBuffer [wordStart + j] = wordString [j];
				textGUI.text = new string (strBuffer);
				yield return WaitForRealTime (1 / 60f);
			}
			//fetch the next word to make sure it fits in the left space of the line otherwise, add a return
			//wordString = GetWord (message);
			if (wordString.Length + charsOnLine >= maxCharsOnLine) {
				textGUI.text += ' ';
				lineNumber++;
			}
			for (int j = 0; j < wordString.Length; j++) {
				textGUI.text += message [i++];
				charsOnLine++;
				yield return WaitForRealTime (1 / 60f);
			}
		}
		cursor.GetComponent<Image> ().enabled = true;
		while (!Input.GetKeyDown (KeyCode.Return))
			yield return null;
		messagePanel.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -55, 0);
		messageDisplayed = false;
		GameController.control.ResumeGame ();
	}

	string GetWord (Text text, string message, int i)
	{
		int start = 0;
		int length = 0;
		while (message [i] != '>' && i < message.Length) {
			text.text += message [i++];
		}
		start = i;
		//'>' reached
		while (message [i] != '<' && i < message.Length) {
			i++;
		}
		length = i;
		while (message [i] != '>' && i < message.Length) {
			text.text += message [i++];
		}
		return (message.Substring (start, length));
	}

	public static IEnumerator WaitForRealTime (float delay)
	{
		while (true) {
			float pauseEndTime = Time.realtimeSinceStartup + delay;
			while (Time.realtimeSinceStartup < pauseEndTime) {
				yield return 0;
			}
			break;
		}
	}
}
