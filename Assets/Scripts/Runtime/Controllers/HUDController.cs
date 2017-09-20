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

    private AudioSource GUISource;

    public GameObject heart;
    public Sprite[] heartSprites = new Sprite[6];
    public AudioClip heartSound;
    private int health;
    private int maxHealth;
    private List<GameObject> hearts;

    // Use this for initialization
    void Start()
    {
        rupeeLimit = GameController.control.playerStats.rupeeLimit;
		rupees = GameController.control.playerStats.rupees;
        rupeesToMove = 0;
        rupeesLabel = transform.FindChild("RupeesPanel").FindChild("RupeesLabel").GetComponent<Text>();
        rupeesImage = transform.FindChild("RupeesPanel").FindChild("RupeesImage").GetComponent<Image>();
        rupeesLabel.text = 0.ToString("D3");
        rupeesImage.sprite = rupeeSprites[3];
        GUISource = gameObject.AddComponent<AudioSource>();
        maxHealth = GameController.control.playerStats.maxHealth;
		health = GameController.control.playerStats.health;
        hearts = new List<GameObject>();
        for(int i = 0; i < maxHealth / 4; i++)
        {
            this.AddHeart(i);
        }
    }
    void Update()
    {
		if(health != GameController.control.playerStats.health || maxHealth != GameController.control.playerStats.maxHealth)
            UpdateLife();
        if(rupeesToMove != 0)
		{
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
    public void UpdateLife()
    {
        //update life state
		if(maxHealth != GameController.control.playerStats.maxHealth)
        {
			maxHealth = GameController.control.playerStats.maxHealth;
            this.AddHeart(maxHealth / 4 - 1);
        }
		if(health > GameController.control.playerStats.health)
        {
			for(int i = health - 1; i >= GameController.control.playerStats.health; i--)
            {
                switch(i % 4)
                {
                    case 3:
                        {
                            hearts[i / 4].GetComponent<Image>().sprite = heartSprites[1];
                            break;
                        }
                    case 2:
                        {
                            hearts[i / 4].GetComponent<Image>().sprite = heartSprites[2];
                            break;
                        }
                    case 1:
                        {
                            hearts[i / 4].GetComponent<Image>().sprite = heartSprites[3];
                            break;
                        }
                    case 0:
                        {
                            if(i != 0)
                            {
                                hearts[(i - 1) / 4].GetComponent<Image>().sprite = heartSprites[0];
                            }
                            hearts[(i / 4)].GetComponent<Image>().sprite = heartSprites[4];
                            break;
                        }
                }
            }
        }
        else
        {
			if(health < GameController.control.playerStats.health)
            {
				for(int i = health - 1; i <= GameController.control.playerStats.health; i++)
                {
                    switch(i % 4)
                    {
                        case 3:
                            {
                                hearts[(i / 4)].GetComponent<Image>().sprite = heartSprites[1];
                                break;
                            }
                        case 2:
                            {
                                hearts[i / 4].GetComponent<Image>().sprite = heartSprites[2];
                                break;
                            }
                        case 1:
                            {
                                hearts[i / 4].GetComponent<Image>().sprite = heartSprites[3];
                                if(i > 4)
                                    hearts[(i / 4) - 1].GetComponent<Image>().sprite = heartSprites[5];
                                break;
                            }
                        case 0:
                            {
                                if(i > 0)
                                    hearts[(i - 1) / 4].GetComponent<Image>().sprite = heartSprites[0];
                                break;
                            }
                    }
                    //heartSound.Play ();
                }
            }
        }
		health = GameController.control.playerStats.health;
    }
    void AddHeart(int heartNumber)
    {
        GameObject newHeart = GameObject.Instantiate(heart);
        hearts.Add(newHeart);
        newHeart.transform.SetParent(transform.FindChild("LifeBar"));
        newHeart.name = "Heart" + heartNumber;
        newHeart.transform.localPosition = new Vector3(heartNumber % 10 * 16, heartNumber / 10 * -16, 0f);
        newHeart.GetComponent<Image>().sprite = heartSprites[0];
        if(heartNumber > 0)
            hearts[heartNumber - 1].GetComponent<Image>().sprite = heartSprites[5];
    }
	public void UpdateRupees()
    {
		if (GameController.control.playerStats.rupees != rupees) 
		{
			rupeesToMove = GameController.control.playerStats.rupees - rupees;
		}
    }
    public void RupeeStash(string size)
    {
        switch(size)
        {
            case "S":
                {
                    if(rupeeLimit < 150)
                    {
                        rupeeLimit = 150;
                        rupeesImage.sprite = rupeeSprites[3];
                    }
                    break;
                }
            case "M":
                {
                    if(rupeeLimit < 300)
                    {
                        rupeeLimit = 300;
                        rupeesImage.sprite = rupeeSprites[2];
                    }
                    break;
                }
            case "L":
                {
                    if(rupeeLimit < 600)
                    {
                        rupeeLimit = 600;
                        rupeesImage.sprite = rupeeSprites[1];
                    }
                    break;
                }
            case "XL":
                {
                    rupeeLimit = 999;
                    rupeesImage.sprite = rupeeSprites[0];
                    break;
                }
        }
    }

    public void DisplayMessage(string text)
    {
        StartCoroutine(DisplayOneByOne(text));
    }

    IEnumerator DisplayOneByOne(string text)
    {
		GameController.control.PauseGame();
        cursor.GetComponent<Image>().enabled = false;
		messagePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        Text textGUI = messagePanel.GetComponentInChildren<Text>();
        textGUI.text = "";
        //rich text assuming coupled tags
        for(int i = 0; i < text.Length; i++)
        {
            if(text[i] == '<')
            {
                while(text[i] != '>' && i < text.Length)
                    textGUI.text += text[i++];
                textGUI.text += text[i++];
                int wordStart = i;
                string wordString = "";
                //get the word and add spaces for future adding
                while(text[i] != '<' && i < text.Length)
                {
                    textGUI.text += ' ';
                    wordString += text[i];
                    i++;
                }
                //add closing tag
                while(text[i] != '>' && i < text.Length)
                    textGUI.text += text[i++];
                textGUI.text += text[i++];
                //adding the rich word one by one
                char[] strBuffer = textGUI.text.ToCharArray();
                for(int j = 0; j < wordString.Length; j++)
                {
                    strBuffer[wordStart + j] = wordString[j];
                    textGUI.text = new string(strBuffer);
                    yield return WaitForRealTime(1 / 60f);
                }
            }
            textGUI.text += text[i];
            yield return WaitForRealTime(1 / 60f);
        }
        cursor.GetComponent<Image>().enabled = true;
		while (!Input.GetKeyDown (KeyCode.Return))
			yield return null;
		messagePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -110, 0);
		GameController.control.ResumeGame();
    }
    public static IEnumerator WaitForRealTime(float delay)
    {
        while(true)
        {
            float pauseEndTime = Time.realtimeSinceStartup + delay;
            while(Time.realtimeSinceStartup < pauseEndTime)
            {
                yield return 0;
            }
            break;
        }
    }
}
