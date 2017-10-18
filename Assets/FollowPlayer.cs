using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public GameObject player;
    public GameObject mainCamera;
    private RectTransform rectTransform;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        mainCamera = GameObject.Find("Main Camera");
        rectTransform = GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void LateUpdate () {
        
        rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(player.transform.position);
    }
}
