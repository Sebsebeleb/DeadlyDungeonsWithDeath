using UnityEngine;
using System.Collections;

public class BehaviourDamagable : MonoBehaviour {

	public int hp = 1;
	public int max_hp = 1;
	public float xp_worth;

	// Use this for initialization
	void Start () {
	
	}

	// TODO: Should probably take the killer as an argument
	public void Die(){
		GameObject.FindWithTag("Player").BroadcastMessage("GiveXP", xp_worth);
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
