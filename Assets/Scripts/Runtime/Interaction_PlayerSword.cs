using UnityEngine;
using System.Collections;

public class Interaction_PlayerSword : MonoBehaviour 
{
	void OnTriggerEnter2D(Collider2D coll) {
        if (coll.isTrigger == false)
        {
            if (coll.gameObject.tag == "Destructible")
            {
                coll.gameObject.GetComponent<Destructible>().DestructionPhase();
            }
            else if (coll.gameObject.tag == "Enemy")
            {
                this.GetComponentInParent<Animator>().SetBool("is_charging", false);
                this.GetComponentInParent<Animator>().SetInteger("charge", 0);
                //			coll.GetComponent<PlayerMovement>().movement_vector = (coll.gameObject.transform.position - transform.position).normalized;
                //			MoveToPosition(coll.gameObject.transform, -direction, 1);
                if (this.GetComponentInParent<Animator>().GetBool("is_tornado"))
                    coll.gameObject.GetComponentInParent<LifeController>().Hurt(4, this.transform.position);
                else
                    coll.gameObject.GetComponentInParent<LifeController>().Hurt(2, this.transform.position);
            }
        }
        else
        {
            if (coll.gameObject.GetComponent<Destructible>())
            if (coll.gameObject.GetComponent<Destructible>().type == DestructibleType.small && !this.GetComponentInParent<Animator>().GetBool("is_charging"))
                coll.gameObject.GetComponent<Destructible>().DestructionPhase();
        }
	}
	public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
   {
      var currentPos = transform.position;
      var t = 0f;
       while(t < 1)
       {
             t += Time.deltaTime / timeToMove;
             transform.position = Vector3.Lerp(currentPos, position, t);
             yield return null;
      }
    }
}
