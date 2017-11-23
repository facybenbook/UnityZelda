using UnityEngine;
using System.Collections;

public class SwordAnimation : StateMachineBehaviour {
	bool charging;
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			charging = true;
	}
	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (!Input.GetKey ((GameController.control.playerStats.slotA.item.name == "sword") ? GameKeys.A: GameKeys.B))
		{
			charging = false;
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (charging) {
			animator.SetBool ("is_charging", true);
		}
		animator.SetBool ("is_sword", false);
	}
}
