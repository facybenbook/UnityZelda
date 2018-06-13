using UnityEngine;

public class AnimatedMechanism : HideableMechanism
{
    private Animator anim;

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetBool("activated", false);
    }

    public override void ActivateOrDeactivate()
    {
        base.ActivateOrDeactivate();
        ToggleAnimator();
    }

    public void ToggleAnimator()
	{
		if (anim) {
            if (state)
            {
                //DEBUG
                print("on");
                anim.enabled = true;
                anim.SetBool("activated", true);
            }
            else
            {
                //DEBUG
                print("off");
                anim.SetBool("activated", false);
            }
        }
	}

}
