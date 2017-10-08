using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {
	private bool pausedState;
    public HUDController hud;
    public GameObject inventory;
    public GameObject transitionLayer;
	// Use this for initialization
	void Start () {
		pausedState = false;
        hud = transform.Find("HUD").gameObject.GetComponent<HUDController>();
        inventory = transform.Find("Inventory").gameObject;
        transitionLayer = transform.Find("TransitionLayer").gameObject;
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
    public IEnumerator FadeIn(float time)
    {
        Color tmp;
        tmp = transitionLayer.GetComponent<Image>().color;
        tmp.a = 0;
        while (tmp.a <= 1)
        {
            print(tmp.a);
            tmp.a += time / 60f;
            transitionLayer.GetComponent<Image>().color = tmp;
            yield return new WaitForSeconds(time / 60f * Time.deltaTime);
        }
    }
    public IEnumerator FadeOut(float time)
    {
        Color tmp;
        tmp = transitionLayer.GetComponent<Image>().color;
        print("d" + tmp.a);
        while (tmp.a >= 0)
        {
            print("d"+tmp.a);
            tmp.a -= time / 60f;
            transitionLayer.GetComponent<Image>().color = tmp;
            yield return new WaitForSeconds(time / 60f * Time.deltaTime);
        }
    }
}
