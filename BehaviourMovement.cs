using UnityEngine;
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
	public bool isAnchored = false; // Should the transform be updated on movement, or are we a part of another gameobject.

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

		// Init the animation state
		anim_pos_origin = transform.position;
		anim_pos_target = transform.position;
		anim_rot_origin = transform.rotation;
		anim_rot_target = transform.rotation;
	}

	void Start(){

	}

	void Update(){
		UpdateAnimation();
	}

	void UpdateAnimation(){

		float t = (Time.time - anim_start_t) * 6;


		//If theres no need to animate, dont do it as it messes with the editor

		//Weapons are centered on the wielder, do not modify positon or rotation.
		if (!isAnchored){
			transform.position = Vector3.Lerp(anim_pos_origin, anim_pos_target, t);
			transform.rotation = Quaternion.Lerp(anim_rot_origin, anim_rot_target, t);
		}

	}



	void _FinishAnimation(){
		transform.position = anim_pos_target;
		anim_pos_origin = transform.position;

		transform.rotation.Set(anim_rot_target.x, anim_rot_target.y, anim_rot_target.z, anim_rot_target.w);
		anim_rot_origin = transform.rotation;
	}


	public void SetPos(int x, int y) {
		old_x = lx;
		old_y = ly;
		lx = x;
		ly = y;
	}
	//Currently doesnt actually move, just the positioning after moving. TODO: maybe rename.
	private void _Move(int x, int y){

		// Update our animation target
		if (!isAnchored){
			_FinishAnimation();
			anim_pos_target = new Vector3(x, y, 0);
		}
		anim_start_t = Time.time;

		if (fAct == ActorType.ACTOR){
			MoveWeapons(old_x, old_y, lx, ly);
		}
	}

	//Force movement without checks, events, or animations
	public bool ForceMove(int x, int y){

		SetPos(x, y);
		_Move(x, y);

		return true;
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
		int old_facing = facing;
		facing += direction;
		if (facing < 0){
			facing += 8;
		}
		else if (facing >= 8){
			facing -= 8;
		}

		int z =  90 -facing * 45;

		onRotate(z);

		//Rotate weapons if any
		paramsWeaponMove p = new paramsWeaponMove(lx, ly, lx, ly, old_facing, facing);
		gameObject.BroadcastMessage("WeaponPartMove", p, SendMessageOptions.DontRequireReceiver);
	}

	//Moves all weapons that are children
	public void MoveWeapons(int old_x, int old_y, int new_x, int new_y){
		paramsWeaponMove p = new paramsWeaponMove(old_x, old_y, new_x, new_y, facing, facing);
		gameObject.BroadcastMessage("WeaponPartMove", p, SendMessageOptions.DontRequireReceiver);
	}

	//TODO: Decide what to do with this
	public void onRotate(float angle){
		_FinishAnimation();
		anim_rot_origin = transform.rotation;
		Quaternion q = Quaternion.identity;
		q.eulerAngles = new Vector3(0, 0, angle);
		anim_rot_target = q;

		anim_start_t = Time.time;
	}
}
