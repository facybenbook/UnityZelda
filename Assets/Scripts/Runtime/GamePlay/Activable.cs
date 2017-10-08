using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class Activable : Conditionable
{
	public UnityEvent actions;
    private Animator anim;
    // Use this for initialization
    void Start()
    {
        state = false;
        if (GetComponent<Animator>()) {
            anim = GetComponent<Animator>();
            anim.SetBool("activated", false);
        }
    }

    protected override void DoSomething()
    {
        actions.Invoke();
    }

    public void EnableAnimator()
	{
		if (GetComponent<Animator> ()) {
			anim.enabled = true;
			anim.SetBool ("activated", true);
		}
	}
}
