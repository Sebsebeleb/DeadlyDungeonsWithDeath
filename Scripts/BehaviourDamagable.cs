using UnityEngine;
using System.Collections;

public class BehaviourDamagable : MonoBehaviour {

	public int hp = 1;
	public int max_hp = 1;
	public float xp_worth;
	public bool invulnerable = false;
	public float life_regen = 0.00f; //life regen'd per turn
	public float life_regen_mod = 1.00f;

	private float regen_counter = 0.0f;

    private Level Lvl;
	private GameObject gm;

    void Awake()
    {
		gm = GameObject.FindWithTag("GM");
        Lvl =  gm.GetComponent<Level>();
    }

	// Use this for initialization
	void Start () {
	
	}

	// TODO: Should probably take the killer as an argument
	public void Die(){
		gameObject.BroadcastMessage("OnDie", SendMessageOptions.DontRequireReceiver);
		GameObject.FindWithTag("Player").BroadcastMessage("GiveXP", xp_worth);
		
		if (gameObject.tag == "Player") {
			GameManagement manage = gm.GetComponent<GameManagement>();
			manage.LoseGame();
		}

		Destroy(gameObject);
	}

	public bool TakeDamage(int damage){
		if (invulnerable){
			return false;
		}	
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

	public void setInvulnerable(bool b){
		invulnerable = b;
	}

	void onUseTurn() {
		
		//Life regen
		regen_counter += life_regen * life_regen_mod * 0.01f;
		while (hp < max_hp && regen_counter > 1.0f) {
			hp += 1;
			regen_counter -= 1.0f;
		}
		regen_counter = Mathf.Min(regen_counter, 1.0f);
	}
} 