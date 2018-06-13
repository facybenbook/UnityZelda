using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {
	private bool pausedState;
    public HUDController hud;
    public GameObject inventory;
    public GameObject transitionLayer;
    public GameObject reducedVisionLayer;
	// Use this for initialization
	void Start () {
        GetComponent<Canvas>().enabled = true;
        pausedState = false;
        hud = transform.Find("HUD").gameObject.GetComponent<HUDController>();
        inventory = transform.Find("Inventory").gameObject;
        transitionLayer = transform.Find("TransitionLayer").gameObject;
        reducedVisionLayer = transform.Find("ReducedVisionLayer").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		if (GameController.control.pauseMenu != pausedState) {
			if (GameController.control.pauseMenu == true)
				GetComponent<Animator> ().SetTrigger ("InventoryMenu");
			else
				GetComponent<Animator> ().SetTrigger ("Exit");
			pausedState = GameController.control.pauseMenu;
		}
		if (pausedState)
		if (Input.GetKeyDown (KeyCode.R)) {
			GetComponent<Animator> ().SetTrigger ("Right");
		}
		else if (Input.GetKeyDown (KeyCode.L)) {
			GetComponent<Animator> ().SetTrigger ("Left");
		}
	}
    public IEnumerator FadeIn(Color transitionTo, float time)
    {
        Color tmp;
        tmp = transitionTo;
        tmp.a = 0;
        while (tmp.a <= 1)
        {
            tmp.a += 1/time / 60f;
            transitionLayer.GetComponent<Image>().color = tmp;
            yield return new WaitForSeconds(1/time / 60f * Time.deltaTime);
        }
    }
    public IEnumerator FadeOut(float time)
    {
        Color tmp;
        tmp = transitionLayer.GetComponent<Image>().color;
        while (tmp.a >= 0)
        {
            tmp.a -= 1/time / 60f;
            transitionLayer.GetComponent<Image>().color = tmp;
            yield return new WaitForSeconds(1/time / 60f * Time.deltaTime);
        }
    }
    /// <summary>
    /// Displays the circular vision field, i.e: too dark to see
    /// </summary>
    /// <returns></returns>
    public void ReducedVision()
    {
        reducedVisionLayer.SetActive(true);
        transitionLayer.GetComponent<Image>().color = new Color(transitionLayer.GetComponent<Image>().color.r, transitionLayer.GetComponent<Image>().color.g, transitionLayer.GetComponent<Image>().color.b, 254);
    }
}
