using UnityEngine;
using System.Collections;

public class BreakableWallController : MonoBehaviour {
	public int offset = 0;
	public Sprite[] sprites = new Sprite[12];
	void start()
	{
		sprites = Resources.LoadAll<Sprite> ("breakableWall");
		print (sprites);
		transform.Find ("0").GetComponent<SpriteRenderer> ().sprite = sprites[0 + 6 * offset];
		transform.Find ("1").GetComponent<SpriteRenderer> ().sprite = sprites[1 + 6 * offset];
		transform.Find ("2").GetComponent<SpriteRenderer> ().sprite = sprites[2 + 6 * offset];
		transform.Find ("3").GetComponent<SpriteRenderer> ().sprite = sprites[3 + 6 * offset];
		transform.Find ("4").GetComponent<SpriteRenderer> ().sprite = sprites[4 + 6 * offset];
		transform.Find ("5").GetComponent<SpriteRenderer> ().sprite = sprites[5 + 6 * offset];
	}
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.name == "Explosion") {
			transform.Find ("0").GetComponent<SpriteRenderer> ().sprite = sprites[6 + 6 * offset];
			transform.Find ("1").GetComponent<SpriteRenderer> ().sprite = sprites[7 + 6 * offset];
			transform.Find ("2").GetComponent<SpriteRenderer> ().sprite = sprites[8 + 6 * offset];
			transform.Find ("3").GetComponent<SpriteRenderer> ().sprite = sprites[9 + 6 * offset];
			transform.Find ("4").GetComponent<SpriteRenderer> ().sprite = sprites[10 + 6 * offset];
			transform.Find ("4").GetComponent<SpriteRenderer> ().sortingOrder = 199;
			transform.Find ("4").GetComponent<BoxCollider2D> ().enabled = false;
			transform.Find ("5").GetComponent<SpriteRenderer> ().sprite = sprites[11 + 6 * offset];
		}
	}


}
