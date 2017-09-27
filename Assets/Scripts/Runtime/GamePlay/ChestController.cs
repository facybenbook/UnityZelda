using UnityEngine;
using System.Collections;

public class ChestController : MonoBehaviour
{
	public GameObject content;
	bool state;
	private IEnumerator coroutine;
	public Sprite close;
	public Sprite open;
	void Start()
	{
		state = false;
	}

	IEnumerator OnTriggerEnter2D(Collider2D coll)
	{
		if (state == false){
			if (coll.gameObject.name == "PlayerShadow") {
				coroutine = WaitForEnterWhileUp (coll);
				yield return StartCoroutine(coroutine);
			}
		}
	}

	IEnumerator WaitForEnterWhileUp(Collider2D coll)
	{
		while (!(Input.GetKeyDown (GameKeys.ENTER) && coll.transform.parent.GetComponent<PlayerMovement>().direction.y == 1))
			yield return null;
		state = true;
		GetComponent<SpriteRenderer> ().sprite = open;
		GameObject.Instantiate (content);
		GetComponent<AudioSource> ().Play ();
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.name == "PlayerShadow") {
			StopCoroutine (coroutine);
		}
	}



}
