using UnityEngine;
using System.Collections;

public class WeaponPart : MonoBehaviour {

	int lx;
	int ly;
	int facing;

	// Where the part is located from the center. e.g 0,0 is where the player is, 0,1 is 1 north while facing north.
	int slot_x;
	int slot_y;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	//x/y is x/y of the wielder/center
	void WeaponPartMove(int x, int y, int facing){
		lx = x;
		ly = y;
		facing = facing;

		//level.Move(ActorType.WEAPON, lx, ly);
	}

	private void _getRelativePosition(int x, int y, int facing){
	}

}
