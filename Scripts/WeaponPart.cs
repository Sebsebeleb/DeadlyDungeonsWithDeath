﻿using UnityEngine;
using System.Collections;

public class WeaponPart : MonoBehaviour {

	int lx;
	int ly;
	int facing;

	public ActorType fAct;

	// Where the part is located from the center. e.g 0,0 is where the player is, 0,1 is 1 north while facing north.
	public int slot_x;
	public int slot_y;

	public int dmg;

	private Level Lvl;
	

	void Awake() {
		Lvl =  GameObject.FindWithTag("GM").GetComponent<Level>();
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}



	//x/y is x/y of the wielder/center
	void WeaponPartMove(paramsWeaponMove p){
		int old_x = lx;
		int old_y = ly;
		int x = p.new_x;
		int y = p.new_y;
		int old_facing = p.old_facing;
		int facing = p.new_facing;

		//Figure out our new relative positions
		Vector2 v = _getRelPos(slot_x, slot_y, facing);

		int tx = x + (int) v.x;
		int ty = y + (int) v.y;

		bool moved = Lvl.MoveActor(gameObject, fAct, lx, ly, tx, ty);
		if (moved){
			lx = tx;
			ly = ty;
		}

		paramsWeaponMove wep_mov = new paramsWeaponMove(old_x, old_y, lx, ly, old_facing, facing);
		//BroadcastMessage("WeaponAttack", wep_mov); 
		WeaponAttack(wep_mov);
	}

	private void WeaponAttack(paramsWeaponMove wep_mov)
	{
		TileData t = Lvl.getAt(wep_mov.new_x, wep_mov.new_y);
		if (t.actor != null)
		{
			WeaponAttack atk = new WeaponAttack(AttackMotion.SWING, dmg);
			t.actor.BroadcastMessage("TakeAttack", atk);
		}
	}


	private static Vector2 _getRelPos(int x, int y, int facing){
		int mag = y;
		int i = (facing + x) / mag;

		if (i > 7) {
			i -= 8;
		}

		switch (i) {
			case 0:
				return new Vector2(0, mag);
			case 1:
				return new Vector2(mag, mag);
			case 2:
				return new Vector2(mag, 0);
			case 3:
				return new Vector2(mag, -mag);
			case 4:
				return new Vector2(0, -mag);
			case 5:
				return new Vector2(-mag, -mag);
			case 6:
				return new Vector2(-mag, 0);
			case 7:
				return new Vector2(-mag, mag);
			default:
				//Something went wrong
				return new Vector2(0, 0);
		}
	}
}
