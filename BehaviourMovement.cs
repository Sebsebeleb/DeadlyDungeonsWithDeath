﻿using UnityEngine;
using System.Collections;

//TODO: Player should be converted to use this, most likely
public class BehaviourMovement : MonoBehaviour {

	//Coordinates on the level
	public int lx = 0;
	public int ly = 0;
	public int old_x = 0;
	public int old_y = 0;

	//What direction we are facing
	public int facing = 0;

	public MovementFlag mflag;
	public ActorType fAct;

	// Animation states
	private Vector3 anim_pos_target;
	private Vector3 anim_pos_origin;
	private Quaternion anim_rot_target;
	private Quaternion anim_rot_origin;
	private float anim_start_t;


	//Level reference
	private Level Lvl;

	void Awake() {
		Lvl =  GameObject.FindWithTag("GM").GetComponent<Level>();
	}

	void Start(){
		// Init the animation state
		anim_pos_origin = transform.position;
		anim_pos_target = transform.position;
		anim_rot_origin = transform.rotation;
		anim_rot_target = transform.rotation;

	}

	void Update(){
		UpdateAnimation();
	}

	void UpdateAnimation(){

		float t = (Time.time - anim_start_t) * 6;

		//If theres no need to animate, dont do it as it messes with the editor
		if (transform.position == anim_pos_target){
			return;
		}

		//Weapons are centered on the wielder, do not modify positon, only rotaiton.
		if (fAct != ActorType.WEAPON){
			transform.position = Vector3.Lerp(anim_pos_origin, anim_pos_target, t);
		}

		transform.rotation = Quaternion.RotateTowards(transform.rotation, anim_rot_target, Time.deltaTime  * 600);
	}


	//Currently doesnt actually move, just the positioning after moving. TODO: maybe rename.
	private void _Move(int x, int y){
		old_x = lx;
		old_y = ly;

		lx = x;
		ly = y;

		// Update our animation target
		if (fAct != ActorType.WEAPON){
			transform.position = anim_pos_target; //Force finish last update
			anim_pos_origin = transform.position;
			anim_pos_target = new Vector3(x, y, 0);
		}
		anim_start_t = Time.time;

		if (fAct != ActorType.WEAPON){
			MoveWeapons(old_x, old_y, lx, ly);
		}
	}

	//Force movement without checks, events, or animations
	public void ForceMove(int x, int y){
		old_x = lx;
		old_y = ly;

		lx = x;
		ly = y;


		anim_pos_target = new Vector3(x, y, 0);
		transform.position = anim_pos_target; //Force finish last update
		anim_pos_origin = anim_pos_target;
		anim_start_t = Time.time;

		//ForceMoveWeapons(x, y);
		if (fAct != ActorType.WEAPON){
			MoveWeapons(old_x, old_y, x, y);
		}
	}

	public bool MoveDirection(int dx, int dy){
		int tx = lx + dx;
		int ty = ly + dy;

		//Does level allow us to?
		bool moved = Lvl.MoveActor(gameObject, fAct, lx, ly, tx, ty);
		if (moved){
			_Move(tx, ty);
		}

		return moved;
	}

	public bool MoveTo(int x, int y){
		bool moved = Lvl.MoveActor(gameObject, fAct, lx, ly, x, y);
		if (moved){
			_Move(x, y);
		}

		return moved;
	}

	public void Rotate(int direction) {
		facing += direction;
		if (facing < 0){
			facing += 8;
		}
		else if (facing >= 8){
			facing -= 8;
		}

		int z = 45 - facing * 45;

		onRotate(z);

		//Rotate weapons if any
		gameObject.BroadcastMessage("WeaponRotate", direction, SendMessageOptions.DontRequireReceiver);
	}

	//Moves all weapons that are children
	public void MoveWeapons(int old_x, int old_y, int new_x, int new_y){
		paramsWeaponMove p = new paramsWeaponMove(AttackMotion.THRUST, old_x, old_y, new_x, new_y);
		gameObject.BroadcastMessage("WeaponMove", p, SendMessageOptions.DontRequireReceiver);
	}

	//TODO: Decide what to do with this
	public void onRotate(float angle){
		transform.rotation = anim_rot_target;
		Quaternion q = Quaternion.identity;
		q.eulerAngles = new Vector3(0, 0, angle);
		anim_rot_target = q;
		anim_start_t = Time.time;
	}

}
