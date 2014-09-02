using UnityEngine;
using System.Collections;

public class BehaviourWeapon : MonoBehaviour {


	public int facing = 0;

	private BehaviourMovement movement;
	private Level Lvl;

	void Awake() {
		Lvl =  GameObject.FindWithTag("GM").GetComponent<Level>();
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

/*
	public void Move(int x, int y){
		foreach (part in weaponparts){
			//Calculate their new position
			part.Move(facing, new_x, new_y)
		}
	}
	*/

	public void WeaponMove(paramsWeaponMove p){
		// Store our old position
		int old_x = movement.lx;
		int old_y = movement.ly;

		int dx = 0;
		int dy = 0;
		getDirection(movement.facing, out dx,  out dy);


		// What we are passed are the coords for the moving actor, not ourselves. So we make a new one that is correct for us
		gameObject.BroadcastMessage("WeaponPartMove", p);
	}

	public void WeaponSetRotation(int face){
		movement.facing = face;

		int z = 45 - movement.facing * 45;
		movement.onRotate(z);
	}
}
