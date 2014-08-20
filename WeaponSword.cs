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
		GameObject enemy = Lvl.getAt(EntityType.ACTOR, p.new_x, p.new_y);
		if (enemy != null){
			WeaponAttack wp = new WeaponAttack(AttackMotion.SWING, damage);
			enemy.BroadcastMessage("TakeAttack", wp);
		}
		return false;
	}
}
