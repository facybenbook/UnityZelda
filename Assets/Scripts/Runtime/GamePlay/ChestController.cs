using UnityEngine;
using System.Collections;

public class ChestController : Conditionable
{
	public GameObject content;
	private IEnumerator coroutine;
	public Sprite open;
    public bool visible = true;

    private void Start()
    {
        state = false;
        if (visible == false)
        { 
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
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
		while (!(Input.GetKeyDown (GameKeys.ENTER) && coll.transform.parent.GetComponent<PlayerMovement>().lookingTowards.y == 1))
			yield return null;
		state = true;
		GetComponent<SpriteRenderer> ().sprite = open;
		GameController.control.AddToZIndex(content, transform.position + new Vector3(0,0.5f,0), new Quaternion(0,0,0,0), transform);
        GetComponent<AudioSource> ().Play ();
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.name == "PlayerShadow") {
			StopCoroutine (coroutine);
		}
	}

    protected override void DoSomething()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }





}
