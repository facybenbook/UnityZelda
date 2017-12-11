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
	public LifeBar lifeBar;
	public GameObject rupeesPanel;
	private AudioSource soundSource;
	public MessageBox messageBox;
	private bool rupeeLimited;

	// Use this for initialization
	void Start ()
	{
		rupeeLimited = false;
		rupeeLimit = GameController.control.playerStats.rupeeLimit;
		rupees = GameController.control.playerStats.rupees;
		rupeesToMove = 0;
		messageBox = transform.Find ("MessageBox").GetComponent<MessageBox> ();
		rupeesPanel = transform.Find ("RupeesPanel").gameObject;
		rupeesLabel = rupeesPanel.transform.Find ("RupeesLabel").GetComponent<Text> ();
		rupeesImage = rupeesPanel.transform.Find ("RupeesImage").GetComponent<Image> ();
		rupeesLabel.text = 0.ToString ("D3");
		rupeesImage.sprite = rupeeSprites [0];
		soundSource = gameObject.AddComponent<AudioSource> ();
	}

	void Update ()
	{
		
		if (rupeesToMove != 0) {
			if (rupeesToMove > 0) {
				rupees++;
				rupeesToMove--;
			} else if (rupeesToMove < 0) {
				rupees--;
				rupeesToMove++;
			}
			rupeesLabel.text = rupees.ToString ("D3");//Update label
			soundSource.PlayOneShot (rupeeSound);
		}
	}

	public void UpdateRupees ()
	{
		if (GameController.control.playerStats.rupees != rupees) {
			rupeesToMove = GameController.control.playerStats.rupees - rupees;
			if (rupees + rupeesToMove == rupeeLimit && rupeeLimited == false) {
				rupeesLabel.font = Resources.Load<Font> ("Fonts/HUD_Numbers_Yellow");
				rupeeLimited = true;
			} else if (rupeeLimited == true) {
				rupeesLabel.font = Resources.Load<Font> ("Fonts/HUD_Numbers");
				rupeeLimited = false;
			}
		}
		else if (GameController.control.playerStats.rupeeLimit != rupeeLimit) {
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
		else
			soundSource.PlayOneShot (rupeeSound);
	}

    public void UpdateRButton(string text)
    {
        transform.Find("Buttons/RButton/Text").GetComponent<Text>().text = text;
    }
}