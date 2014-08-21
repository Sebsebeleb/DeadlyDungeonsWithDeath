using UnityEngine;
using System.Collections;

public class BehaviourWeapon : MonoBehaviour {


	private BehaviourMovement movement;
	private Level Lvl;

	void Awake() {
		Lvl =  GameObject.FindWithTag("GM").GetComponent<Level>();
		movement = gameObject.GetComponent<BehaviourMovement>();
	}

	// Use this for initialization
	void Start () {
	}

	// Static utility function, maybe it should be in a utility class for other direction stuff too
	private void getDirection(int dir, out int dx, out int dy) {
		dx = 0;
		dy = 0;
		if (dir == 0 || dir == 6 || dir == 7){
			dx = -1;
		}
		else if (dir == 2 || dir == 3 || dir == 4){
			dx = 1;
		}
		if (dir < 3){
			dy = 1;
		}
		else if (dir == 5 || dir == 6 || dir == 7){
			dy = -1;
		}
	}
	

	public void WeaponMove(paramsWeaponMove p){
		// Store our old position
		int old_x = movement.lx;
		int old_y = movement.ly;

		int dx = 0;
		int dy = 0;
		getDirection(movement.facing, out dx,  out dy);

		movement.MoveTo(p.new_x + dx, p.new_y + dy);

		// What we are passed are the coords for the moving actor, not ourselves. So we make a new one that is correct for us
		paramsWeaponMove wp = new paramsWeaponMove(p.motion, old_x, old_y, movement.lx, movement.ly);
		gameObject.BroadcastMessage("WeaponAttack", wp);
	}

	public void WeaponSetRotation(int face){
		movement.facing = face;

		int z = 45 - movement.facing * 45;
		movement.onRotate(z);
	}

	// > 1 is CW, < 0 is CCW
	public void WeaponRotate(int direction){
		// TODO: Handling rotation of more than 1 step
		if (direction == 0){
			return;
		}


		int old_x = movement.lx;
		int old_y = movement.ly;

		int dx = 0;
		int dy = 0;

		//TODO: improve
		// CW
		if (direction < 0){
			switch(movement.facing){
				case 0:
					dy = -1;
					break;
				case 1:
					dx = -1;
					break;
				case 2:
					dx = -1;
					break;
				case 3:
					dy = 1;
					break;
				case 4:
					dy = 1;
					break;
				case 5:
					dx = 1;
					break;
				case 6:
					dx = 1;
					break;
				case 7:
					dy = -1;
					break;
			}
		}
		else {
			switch(movement.facing){
				case 0:
					dx = 1;
					break;
				case 1:
					dx = 1;
					break;
				case 2:
					dy = -1;
					break;
				case 3:
					dy =-1;
					break;
				case 4:
					dx = -1;
					break;
				case 5:
					dx = -1;
					break;
				case 6:
					dy = 1;
					break;
				case 7:
					dy = 1;
					break;
			}
		}

		movement.facing += direction;
		if (movement.facing < 0){
			movement.facing += 8;
		}
		else if (movement.facing >= 8){
			movement.facing -= 8;
		}


		int z = 45 - movement.facing * 45;
		movement.onRotate(z);

		paramsWeaponMove wep_mov = new paramsWeaponMove(AttackMotion.SWING, old_x, old_y, old_x, old_y);

		movement.MoveDirection(dx, dy);

		// What we are passed are the coords for the moving actor, not ourselves. So we make a new one that is correct for us
		paramsWeaponMove wp = new paramsWeaponMove(AttackMotion.SWING, old_x, old_y, movement.lx, movement.ly);
		gameObject.BroadcastMessage("WeaponAttack", wp);

	}
}
