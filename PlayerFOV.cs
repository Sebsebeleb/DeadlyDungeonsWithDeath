using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerFOV : MonoBehaviour {

	public int vision_range = 3;

	private Level Lvl;
	private BehaviourMovement movement;

	void Awake(){
		movement = GetComponent<BehaviourMovement>();
		Lvl =  GameObject.FindWithTag("GM").GetComponent<Level>();
	}

	void Start(){
		updateFOV();
	}

	public void updateFOV(){
		int px = movement.lx;
		int py = movement.ly;

		foreach (Vector2 pos in coordsRange(vision_range, px, py)){
			//Enable renderers of everything we can see
			int x = (int) pos.x;
			int y = (int) pos.y;
			TileData tile = Lvl.getAt(x, y);
			if (tile == null){
				continue;
			}
			GameObject floor = tile.floor;
			GameObject wall = tile.wall;
			GameObject actor = tile.actor;
			GameObject item = tile.item;

			if (floor != null){
				floor.renderer.enabled = true;
			}
			if (wall != null){
				wall.renderer.enabled = true;
			}
			if (actor != null){
				actor.renderer.enabled = true;
			}
			if (item != null) {
				item.renderer.enabled = true;
			}
			foreach (GameObject wep in tile.weapons){
                if (wep == null)
                {
                    continue;
                }

				// FIXME: short term solution
				if (wep.transform.parent.renderer != null) {
					wep.transform.parent.renderer.enabled = true;
				}
				else {
					wep.renderer.enabled = true;
				}
			}
		}
	}

	public bool CanSee(int x, int y) {
		if (Mathf.Abs(movement.lx - x) > vision_range || Mathf.Abs(movement.ly - y) > vision_range) {
			return false;
		}
		else {
			return true;
		}
	}

	//Returns coordinates within a "range" from a certain position.
	// TODO: Maybe not use vector2? we only need ints
	public List<Vector2> coordsRange(int range, int ox, int oy){
		List<Vector2> coords = new List<Vector2>();

		for (int xx = ox - range; xx < ox + range; xx++){
			for (int yy = oy - range; yy < oy + range; yy++){
				coords.Add(new Vector2(xx, yy));
			}
		}

		return coords;
	}

}
