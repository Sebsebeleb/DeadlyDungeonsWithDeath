using UnityEngine;
using System.Collections;

public class BrainSkeleton : MonoBehaviour {

	private PlayerMovement player_movement;
	private BehaviourMovement movement;
	private GameObject weapon;


	void Awake() {
		movement = gameObject.GetComponent<BehaviourMovement>();
		player_movement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
	}

	// Called when spawned on the level
	public void OnSpawn() {
		movement.facing = Random.Range(0, 8);
		movement.MoveWeapons(movement.lx, movement.ly, movement.lx, movement.ly);
		//gameObject.BroadcastMessage("WeaponSetRotation", movement.facing);
	}

	// Called when it is this actors' turn
	public void Think(){

		//Move towards the player

		int dx = player_movement.lx - movement.lx;
		int dy = player_movement.ly - movement.ly;

		// 0 -> north, 1 -> east ...
		int[] movePriority = new int[4];
		// Big ugly way to setup our movement priority
		if (Mathf.Abs(dy) > Mathf.Abs(dx)) {
			if (dy > 0){
				movePriority[0] = 0;
				movePriority[3] = 2;
			}
			else{
				movePriority[0] = 2;
				movePriority[3] = 0;
			}
			if (dx > 0) {
				movePriority[1] = 1;
				movePriority[2] = 3;
			}
			else{
				movePriority[1] = 3;
				movePriority[2] = 1;
			}
		}
		else {
			if (dx > 0){
				movePriority[0] = 1;
				movePriority[3] = 4;
			}
			else{
				movePriority[0] = 2;
				movePriority[3] = 0;
			}
			if (dy > 0) {
				movePriority[1] = 0;
				movePriority[2] = 2;
			}
			else {
				movePriority[1] = 2;
				movePriority[2] = 3;
			}
		}

		//We always move as long as it is possible

		bool moved = false;
		int i = 0;
		int vx = 0;
		int vy = 0;

		while (!moved && i < 4){
			int direction = movePriority[i];
			switch (direction){
				case 0:
					vx = 0;
					vy = 1;
					break;
				case 1:
					vx = 1;
					vy = 0;
					break;
				case 2:
					vx = 0;
					vy = -1;
					break;
				case 3:
					vx = -1;
					vy = 0;
					break;
			}
			i++;

			moved = movement.MoveDirection(vx, vy);
		}

		movement.Rotate(-1);
	}
}
