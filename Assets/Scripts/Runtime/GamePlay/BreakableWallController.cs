using UnityEngine;
using System.Collections;

public class BreakableWallController : MonoBehaviour {
    public bool state = false;
	public Sprite[] sprites = new Sprite[2];
    void OnTriggerEnter2D(Collider2D coll) {
        if (state == false)
        {
            if (coll.gameObject.tag == "Explosion")
            {
                GetComponent<SpriteRenderer>().sprite = sprites[1];
                GetComponent<BoxCollider2D>().enabled = false;
                state = true;
            }
        }
	}


}
