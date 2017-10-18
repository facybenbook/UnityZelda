using UnityEngine;
using System.Collections;

public class Chuchu_AI : CharactersController
{
	bool hidden;
    protected Collider2D visionCollider;

    protected override void Start ()
	{
        base.Start();
		hidden = true;
    }
    //if the monster sees the player (visual radius), appears
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player" && hidden == true)
        {
            GetComponent<Animator>().SetBool("is_coming", true);
            GetComponent<CircleCollider2D>().radius = 6;
            hidden = false;
        }
    }

    //while the player is in the visual radius of the monster, chase the player
    void OnTriggerStay2D(Collider2D coll)
    {
        //verify if the object in the radius is the player
        if (coll.gameObject.tag == "Player" && hidden == false)
        {
            //move toward the player
            movementDirection.x = (coll.gameObject.transform.position.x - rbody.position.x);
            movementDirection.y = coll.gameObject.transform.position.y - rbody.position.y;
            movementDirection.Normalize();
        }
    }
    //when the player leaves the visual zone
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            
            movementDirection = Vector2.zero;
        }
    }
}
