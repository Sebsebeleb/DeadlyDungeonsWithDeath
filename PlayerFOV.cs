using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerFOV : MonoBehaviour {

	public int vision_range = 3;

	private Level Lvl;
	private PlayerMovement movement;

	void Awake(){
		movement = GetComponent<PlayerMovement>();
		Lvl =  GameObject.FindWithTag("GM").GetComponent<Level>();
	}

	void Start(){
	}

	public void updateFOV(){
		int px = movement.lx;
		int py = movement.ly;

		foreach (Vector2 pos in coordsRange(vision_range, px, py)){
			//Enable renderers of everything we can see
			int x = (int) pos.x;
			int y = (int) pos.y;
			GameObject floor = Lvl.getAt(EntityType.FLOOR, x, y);
			GameObject wall = Lvl.getAt(EntityType.WALL, x, y);
			GameObject actor = Lvl.getAt(EntityType.ACTOR, x, y);
			GameObject weapon = Lvl.getAt(EntityType.WEAPON, x, y);

			if (floor != null){
				floor.renderer.enabled = true;
			}
			if (wall != null){
				wall.renderer.enabled = true;
			}
			if (actor != null){
				actor.renderer.enabled = true;
			}
			if (weapon != null){
				weapon.renderer.enabled = true;
			}
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
