using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Data for tiles on the grid

public class TileData{

	public GameObject actor;
	public GameObject floor;
	public GameObject on_floor; // on top of floor; traps, lingering effects etc. Has triggers for weapons and actors moving onto them
	public GameObject item;
	public GameObject wall;
	
	public List<GameObject> weapons;

	public TileData(){
		weapons = new List<GameObject>();
	}
}
