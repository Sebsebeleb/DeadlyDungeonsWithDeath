using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour {

	//Coordinates on the level
	public int lx = 0;
	public int ly = 0;
	public int old_x = 0;
	public int old_y = 0;

	//Level reference
	private Level Lvl;

	// Animation states
	private Vector3 anim_pos_target;
	private Vector3 anim_pos_origin;
	private Quaternion anim_rot_target;
	private Quaternion anim_rot_origin;
	private float anim_start_t;


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
		transform.position = Vector3.Lerp(anim_pos_origin, anim_pos_target, t);
		transform.rotation = Quaternion.Lerp(anim_rot_origin, anim_rot_target, t);
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

		BroadcastMessage("updateFOV");

		//ForceMoveWeapons(x, y);
		MoveWeapons(old_x, old_y, x, y);
	}

	//Currently doesnt actually mvoe, just the "on_move" thign really. TODO: maybe rename.
	private void _Move(int x, int y){
		old_x = lx;
		old_y = ly;

		lx = x;
		ly = y;

		// Update our animation target
		transform.position = anim_pos_target; //Force finish last update
		anim_pos_origin = transform.position;
		anim_pos_target = new Vector3(x, y, 0);
		anim_start_t = Time.time;

		BroadcastMessage("updateFOV");
		MoveWeapons(old_x, old_y, lx, ly);
	}

	public void MoveDirection(int dx, int dy){
		int tx = lx + dx;
		int ty = ly + dy;

		//Does level allow us to?
		if (Lvl.MoveActor(gameObject, ActorType.ACTOR, lx, ly, tx, ty)){
			_Move(tx, ty);
		}
	}

	// < 0 => cw, > 0 => ccw
	public void Rotate(int dr){
		gameObject.BroadcastMessage("WeaponRotate", dr);
	}

	//Moves all weapons that are children
	public void MoveWeapons(int old_x, int old_y, int new_x, int new_y){
		paramsWeaponMove p = new paramsWeaponMove(AttackMotion.THRUST, old_x, old_y, new_x, new_y);
		gameObject.BroadcastMessage("WeaponMove", p);
	}
}
