using UnityEngine;
using System.Collections;

public class BrainSlime : MonoBehaviour, IBrain {

	private BehaviourMovement player_movement;
	private BehaviourMovement movement;


	// Use this for initialization
	void Awake () {
		movement = gameObject.GetComponent<BehaviourMovement>();
		player_movement = GameObject.FindWithTag("Player").GetComponent<BehaviourMovement>();
	}

	// Called when it is this actors' turn
	public void Think(){

		//Move towards the player

		int dx = player_movement.lx - movement.lx;
		int dy = player_movement.ly - movement.ly;

		//If we are next to player, attack it.
		if  (Mathf.Abs(dx) + Mathf.Abs(dy) == 1){
			player_movement.BroadcastMessage("TakeDamage", 1);
			return;
		}

		// Direction to move
		int vx = 0;
		int vy = 0;
		if (Mathf.Abs(dx) > Mathf.Abs(dy)) {
			vx = Mathf.Clamp(dx, -1, 1);
		}
		else {
			vy = Mathf.Clamp(dy, -1, 1);
		}


		if (vx != 0 | vy != 0){
			movement.MoveDirection(vx, vy);
		}
	}
}
