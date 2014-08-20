using UnityEngine;
using System.Collections;

public class BehaviourWeapon : MonoBehaviour {

	public int facing = 0;

	private BehaviourMovement movement;
	private Level Lvl;

	void Awake() {
		Lvl =  GameObject.FindWithTag("GM").GetComponent<Level>();
		movement = gameObject.GetComponent<BehaviourMovement>();
	}

	// Use this for initialization
	void Start () {
	}
	

	public void WeaponMove(paramsWeaponMove p){
		// Store our old position
		int old_x = movement.lx;
		int old_y = movement.ly;

		// Calculate the direction moved
		int dx = p.new_x - p.old_x;
		int dy = p.new_y - p.old_y;

		movement.MoveTo(p.new_x, p.new_y);

		// What we are passed are the coords for the moving actor, not ourselves. So we make a new one that is correct for us
		paramsWeaponMove wp = new paramsWeaponMove(p.motion, old_x, old_y, movement.lx, movement.ly);
		gameObject.BroadcastMessage("WeaponAttack", wp);
	}

	public void WeaponSetRotation(int face){
		facing = face;

		int z = 45 - facing * 45;
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
			switch(facing){
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
			switch(facing){
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

		facing += direction;
		if (facing < 0){
			facing += 8;
		}
		else if (facing >= 8){
			facing -= 8;
		}


		int z = 45 - facing * 45;
		movement.onRotate(z);

		paramsWeaponMove wep_mov = new paramsWeaponMove(AttackMotion.SWING, old_x, old_y, old_x+dx, old_y+dy);

		WeaponMove(wep_mov);

	}
}
