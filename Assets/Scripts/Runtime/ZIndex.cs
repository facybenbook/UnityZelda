using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZIndex : MonoBehaviour {
	public List<GameObject> layerZObjects;
	// Use this for initialization
	void Start () {
		layerZObjects = new List<GameObject> ();
		GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
		for (int i = 0; i < objects.Length; i++) {
			if (objects [i].layer == 8)
				layerZObjects.Add (objects [i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//clear array from deleted objects
		for (int i = 0; i < layerZObjects.Count; i++) {
			if (layerZObjects [i] == null) {
				layerZObjects.Remove (layerZObjects [i]);
				i--;
			}
		}
		//classic descendant sort
		int max;
		GameObject swap;
		for (int i = 0; i < layerZObjects.Count; i++) {
			max = i;
			for (int j = i; j < layerZObjects.Count; j++) {
				if (layerZObjects[j].transform.position.y > layerZObjects [i].transform.position.y) {
					max = j;
				}
			}
			if (max != i) {
				swap = layerZObjects [i];
				layerZObjects [i] = layerZObjects [max];
				layerZObjects [max] = swap;
			}
		}
		//maximum index
		int zIndex = 2;
		float lastY = layerZObjects [0].transform.position.y;
		for (int i = 1; i < layerZObjects.Count; i++) {
			if (layerZObjects [i].transform.position.y <= lastY) {
				zIndex++;
				if (zIndex >= 100)
					print ("Zindex error");
			}
			if (layerZObjects [i].GetComponent<SpriteRenderer> ())
			layerZObjects [i].GetComponent<SpriteRenderer> ().sortingOrder = zIndex;
			lastY = layerZObjects [i].transform.position.y;
		}
	}
}
