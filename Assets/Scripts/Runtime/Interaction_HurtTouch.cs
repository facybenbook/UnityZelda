using UnityEngine;
using System.Collections.Generic;


public class Interaction_HurtTouch : MonoBehaviour {
    public enum CanDestroy {None, OnlySmall, OnlyMedium, OnlyBig, SmallAndMedium, All}
	public int damages = 1;
    public bool useTrigger = false;
    public CanDestroy destroys = CanDestroy.None;
    public bool hurtsPlayer = true;
    public bool hurtsEnemies = false;
    
    
    void OnCollisionEnter2D(Collision2D coll)
	{
        if ((hurtsPlayer && coll.gameObject.tag == "Player") || (hurtsEnemies && coll.gameObject.tag == "Enemy"))
		{
			//Hurt the player with damages, and gives the hurter's position for the character to escape
			coll.gameObject.GetComponent<LifeController>().Hurt(damages, this.transform.position);
		}
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (useTrigger && coll.isTrigger == false)
        {
            if ((hurtsPlayer && coll.gameObject.tag == "Player") || (hurtsEnemies && coll.gameObject.tag == "Enemy"))
            {
                //Hurt the player with damages, and gives the hurter's position for the character to escape
                coll.gameObject.GetComponent<LifeController>().Hurt(damages, this.transform.position);
            }
            else if (coll.gameObject.tag == "Destructible" && destroys != CanDestroy.None)
            {
                if(destroys == CanDestroy.All)
                    coll.GetComponent<Destructible>().DestructionPhase();

                //big if statement that burns your eyes
                if ((destroys == CanDestroy.OnlySmall || destroys == CanDestroy.SmallAndMedium) && coll.GetComponent<Destructible>().type == DestructibleType.small ||
                      destroys == CanDestroy.OnlyMedium && coll.GetComponent<Destructible>().type == DestructibleType.medium ||
                      destroys == CanDestroy.OnlyBig && coll.GetComponent<Destructible>().type == DestructibleType.big)
                    coll.GetComponent<Destructible>().DestructionPhase();

            }
        }
    }

}
