using UnityEngine;
using System.Collections;

public class ChestController : Conditionable
{
	public Collectible content;
	private IEnumerator coroutine;
	public Sprite openSprite;

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
				coroutine = WaitForEnterWhileUp (coll.transform.parent);
				yield return StartCoroutine(coroutine);
			}
		}
	}

	IEnumerator WaitForEnterWhileUp(Transform player)
	{
		while (!(Input.GetKeyDown (GameKeys.ENTER) && player.GetComponent<PlayerController>().characterOrientation.y == 1))
			yield return null;
		state = true;
		GetComponent<SpriteRenderer> ().sprite = openSprite;

//refactor maybe ??
        player.GetComponent<PlayerController>().GetItem(content.gameObject);
        //GameController.control.AddToZIndex(content, transform.position + new Vector3(0,0.5f,0), new Quaternion(0,0,0,0), transform);
        GetComponent<AudioSource> ().Play ();
        player.GetComponent<Animator>().SetBool("is_busy", false);

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
