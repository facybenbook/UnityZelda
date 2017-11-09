using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class Activable : Conditionable
{
	public UnityEvent activate;
    public UnityEvent deactivate;
    private Animator anim;

    protected override void Start()
    {
        state = false;
        if (GetComponent<Animator>()) {
            anim = GetComponent<Animator>();
            anim.SetBool("activated", false);
        }
        action = activate.Invoke;
    }

    public override void OnCheckConditions()
    {
        base.OnCheckConditions();
        //invert the actions
        if (state)
            action = deactivate.Invoke;
        else
            action = activate.Invoke;
    }

    public void ToggleAnimator()
	{
		if (anim) {
            if (state)
            {
                print("on");
                anim.enabled = true;
                anim.SetBool("activated", true);
            }
            else
            {
                print("off");
                anim.SetBool("activated", false);
            }
        }
	}

}
