using UnityEngine;
using System.Collections;

public class WeaponSword : MonoBehaviour {

	public int damage = 1;

	private Level Lvl;

	void Awake() {
		Lvl =  GameObject.FindWithTag("GM").GetComponent<Level>();
	}

	void Start(){
	}


	public bool WeaponAttack(paramsWeaponMove p){
		TileData tile = Lvl.getAt(p.new_x, p.new_y);
		if (tile.actor != null){
			WeaponAttack wp = new WeaponAttack(AttackMotion.SWING, damage);
			tile.actor.BroadcastMessage("TakeAttack", wp);
		}
		return false;
	}
}
