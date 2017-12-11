using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LifeBar : MonoBehaviour {
    public GameObject heart;
    public Sprite[] heartSprites = new Sprite[6];
    public AudioClip heartSound;
    public AudioSource audioSource;

    private List<GameObject> heartList;
    private int health;
    private int maxHealth;
    // Use this for initialization
    void Start () {
        maxHealth = GameController.control.playerStats.maxHealth;
        health = GameController.control.playerStats.health;
        heartList = new List<GameObject>();
        for (int i = 0; i < maxHealth / 4; i++)
        {
            this.AddHeart(i);
        }
    }

    private void Update()
    {
        if (health != GameController.control.playerStats.health || maxHealth != GameController.control.playerStats.maxHealth)
            UpdateLife();
    }

    public void UpdateLife()
    {
        //update life state
        if (maxHealth != GameController.control.playerStats.maxHealth)
        {
            maxHealth = GameController.control.playerStats.maxHealth;
            this.AddHeart(maxHealth / 4 - 1);
        }
        if (health > GameController.control.playerStats.health)
        {
            for (int i = health - 1; i >= GameController.control.playerStats.health; i--)
            {
                switch (i % 4)
                {
                    case 3:
                        {
                            heartList[i / 4].GetComponent<Image>().sprite = heartSprites[1];
                            break;
                        }
                    case 2:
                        {
                            heartList[i / 4].GetComponent<Image>().sprite = heartSprites[2];
                            break;
                        }
                    case 1:
                        {
                            heartList[i / 4].GetComponent<Image>().sprite = heartSprites[3];
                            break;
                        }
                    case 0:
                        {
                            if (i != 0)
                            {
                                heartList[(i - 1) / 4].GetComponent<Image>().sprite = heartSprites[0];
                            }
                            heartList[(i / 4)].GetComponent<Image>().sprite = heartSprites[4];
                            break;
                        }
                }
            }
        }
        else
        {
            if (health < GameController.control.playerStats.health)
            {
                for (int i = health - 1; i <= GameController.control.playerStats.health; i++)
                {
                    switch (i % 4)
                    {
                        case 3:
                            {
                                heartList[(i / 4)].GetComponent<Image>().sprite = heartSprites[1];
                                break;
                            }
                        case 2:
                            {
                                heartList[i / 4].GetComponent<Image>().sprite = heartSprites[2];
                                break;
                            }
                        case 1:
                            {
                                heartList[i / 4].GetComponent<Image>().sprite = heartSprites[3];
                                if (i > 4)
                                    heartList[(i / 4) - 1].GetComponent<Image>().sprite = heartSprites[5];
                                break;
                            }
                        case 0:
                            {
                                if (i > 0)
                                    heartList[(i - 1) / 4].GetComponent<Image>().sprite = heartSprites[0];
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
        heartList.Add(newHeart);
        newHeart.transform.SetParent(transform);
        newHeart.name = "Heart" + heartNumber;
        newHeart.transform.localScale = Vector3.one;
        newHeart.GetComponent<RectTransform>().anchoredPosition = new Vector3(heartNumber % 10 * 8, heartNumber / 10 * -8);
        newHeart.GetComponent<Image>().sprite = heartSprites[0];
        if (heartNumber > 0)
            heartList[heartNumber - 1].GetComponent<Image>().sprite = heartSprites[5];
    }
}
