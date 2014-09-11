using UnityEngine;
using System.Collections;


public struct paramsWeaponMove{
	public int old_x, old_y, new_x, new_y;
	public int old_facing, new_facing;


	public paramsWeaponMove(int x1, int y1, int x2, int y2, int old_facing, int new_facing){
		this.old_x = x1;
		this.old_y = y1;
		this.new_x = x2;
		this.new_y = y2;
		this.old_facing = old_facing;
		this.new_facing = new_facing;
	}
}

// Describes an attack that has occoured
public struct WeaponAttack{
	public int damage;
	public AttackMotion attack_type;

	public WeaponAttack(AttackMotion at, int dmg){
		this.damage = dmg;
		this.attack_type = at;
	}
}

public class LevelData{
	public int w, h;

	public int entrance_x, entrance_y;

	public int exit_x, exit_y;

	public TileType[,] tiles;
	public bool[,] items;

	public LevelData(int width, int height){
		this.w = width;
		this.h = height;

		this.entrance_x = 0;
		this.entrance_y = 0;
		this.exit_x = 0;
		this.exit_y = 0;

		this.tiles = new TileType[width, height];
		this.items = new bool[width, height];
	}
}