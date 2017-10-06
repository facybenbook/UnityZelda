using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {
	public GameObject Explosion;
    public bool remoteControlled;
    // Use this for initialization

    private void Start()
    {
        GameController.control.AddToZIndex(Explosion, transform.position, transform.rotation);
    }
    // Update is called once per frame
    void Update () {
        
        Destroy(this.gameObject);
	}
}
