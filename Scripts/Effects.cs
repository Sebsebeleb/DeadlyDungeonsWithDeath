using UnityEngine;
using System.Collections;

public class effectBurningAcid : ScriptableObject {

	public int dur_left = 5;

	public void OnSteppedUpon(GameObject stepper) {
		BehaviourMovement mov = stepper.GetComponent<BehaviourMovement>();
		if (mov == null){
			return;
		}

		if (mov.fAct == ActorType.WEAPON) {

		}
		else if (mov.fAct == ActorType.ACTOR) {
			mov.BroadcastMessage("TakeDamage", 1, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void OnTurn() {
		dur_left--;
	}
}