using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class Activable : MonoBehaviour
{
    public List<GameObject> conditions = new List<GameObject>();
    public bool state;
	public UnityEvent actions;
	private Animator anim;
    // Use this for initialization
    void Start()
    {
        state = false;
		if (GetComponent<Animator> ()) {
			anim = GetComponent<Animator> ();
			anim.SetBool ("activated", false);
		}
    }

    protected void OnCheckConditions()
	{
		if (state == false) {
			foreach (GameObject trigger in conditions) {
				if (trigger.tag == "Enemy") {
					if (trigger.GetComponent<LifeController> ().health > 0)
						return;
				}
				if (trigger.GetComponent<Trigger> ()) {
					if (trigger.GetComponent<Trigger> ().state == false)
						return;
				}
			}
			actions.Invoke ();
			state = true;
		}
	}

	public void EnableAnimator()
	{
		print ("yoyoyo");
		if (GetComponent<Animator> ()) {
			anim.enabled = true;
			anim.SetBool ("activated", true);
		}
	}
}
