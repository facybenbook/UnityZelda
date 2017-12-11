using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour {
    public Vector2 area;
    public GameObject map;
	// Use this for initialization
	void Start () {
        area = new Vector2(transform.localScale.x, transform.localScale.y);
        map = transform.root.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
