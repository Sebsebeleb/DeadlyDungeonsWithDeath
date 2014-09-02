using UnityEngine;
using System.Collections;

public class WeaponPart : MonoBehaviour {

	int lx;
	int ly;
	int facing;

	public ActorType fAct;

	// Where the part is located from the center. e.g 0,0 is where the player is, 0,1 is 1 north while facing north.
	int slot_x;
	int slot_y;

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
		int x = p.new_x;
		int y = p.new_y;
		int old_facing = p.old_facing;
		int facing = p.new_facing;

		//Figure out our new relative positions
		Vector2 v = _getRelPos(slot_x, slot_y, facing);

		int tx = x + (int) v.x;
		int ty = y + (int) v.y;

		Lvl.MoveActor(gameObject, fAct, lx, ly, tx, ty);
	}


	private static Vector2 _getRelPos(int x, int y, int facing){
		switch (facing){
			// North
			case 0:
				return new Vector2(x, y);
			// East
			case 2:
				return new Vector2(y, x);
			// South
			case 4:
				return new Vector2(-x, -y);
			// West
			case 6:
				return new Vector2(y, -x);
			// Northeast
			case 1:
				return new Vector2(x*y + x, y);
			//Southeast
			case 3:
				return new Vector2(x*y + x, -y);
			//Southwest
			case 5:
				return new Vector2(-(x*y+x), -y);
			case 7:
				return new Vector2(-(x*y+x), y);
			default:
				return new Vector2(x, y);
		}
	}
}
