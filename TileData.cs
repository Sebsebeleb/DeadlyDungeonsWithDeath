using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Data for tiles on the grid

public class TileData{

	public GameObject actor;
	public GameObject floor;
	public GameObject wall;
	public List<GameObject> weapons;

	public TileData(){
		weapons = new List<GameObject>();
	}
}
