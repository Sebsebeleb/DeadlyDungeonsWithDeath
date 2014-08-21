using UnityEngine;
using System.Collections;

public class BehaviourDamagable : MonoBehaviour {

	public int hp = 1;
	public int max_hp = 1;

	// Use this for initialization
	void Start () {
	
	}

	public void Die(){
		Destroy(gameObject);
	}

	public bool TakeDamage(int damage){
		hp -= damage;

		if (hp <= 0 ){
			Die();
		}
		return true;
	}

	// Return true if we took damage
	public bool TakeAttack(WeaponAttack p){
		int dmg = p.damage;



		return TakeDamage(dmg);
	}
}
