using UnityEngine;
using System.Collections.Generic;

public class Hurt_touch : MonoBehaviour {
	public int damages = 1;
    public bool useTrigger = false;
    
    void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.name == "Player")
		{
			//Hurt the player with damages, and gives the hurter's position for the character to escape
			coll.gameObject.GetComponent<LifeController>().Hurt(damages, this.transform.position);
		}
	}
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (useTrigger)
        {
            if (coll.gameObject.name == "Player")
            {
                //Hurt the player with damages, and gives the hurter's position for the character to escape
                coll.gameObject.GetComponent<LifeController>().Hurt(damages, this.transform.position);
            }
        }
    }

}
