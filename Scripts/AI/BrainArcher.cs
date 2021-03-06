﻿using UnityEngine;
using System.Collections;

public class BrainArcher : MonoBehaviour, IBrain {

	private BehaviourMovement player_movement;
	private BehaviourMovement movement;

	public GameObject PrefabArrow;

	//Level reference
	private Level Lvl;

	public int ReloadTime = 2;
	private int counterReload = 0;

	void Awake() {
		Lvl = GameObject.FindWithTag("GM").GetComponent<Level>();

		movement = gameObject.GetComponent<BehaviourMovement>();
		player_movement = GameObject.FindWithTag("Player").GetComponent<BehaviourMovement>();
	}

	void Start() {

	}

	// Called when it is this actors' turn
	public void Think(){

		counterReload--;

		//Move towards the player

		int dx = player_movement.lx - movement.lx;
		int dy = player_movement.ly - movement.ly;


		if ((dx == 0 || dy == 0) && (Mathf.Abs(dx) + Mathf.Abs(dy) < 5)) {
			//Fire arrow, 4 range
			if (TryFire(dx, dy)){
				return;
			}
			
		}

		// Direction to move
		int vx = 0;
		int vy = 0;
		if (Mathf.Abs(dx) > Mathf.Abs(dy)) {
			vy = Mathf.Clamp(dy, -1, 1);
		}
		else {
			vx = Mathf.Clamp(dx, -1, 1);
		}


		if (vx != 0 | vy != 0){
			movement.MoveDirection(vx, vy);
		}
	}

	private void FireArrow(int dx, int dy) {
		counterReload = ReloadTime;

		int sx, sy;
		if (dx == 0) {
			sy = (int)Mathf.Sign(dy);
			sx = 0;
		}
		else {
			sx = (int)Mathf.Sign(dx);
			sy = 0;
		}

		GameObject arrow = Lvl.Spawn(PrefabArrow, movement.lx + sx, movement.ly + sy);

		BehaviourProjectile proj = arrow.GetComponent<BehaviourProjectile>();
		proj.SetMovementDirection(sx, sy);
	}
	
	private bool TryFire(int dx, int dy){
		if (counterReload <= 0) {
			FireArrow(dx, dy);
			return true;
		}
		return false;
	}
}
