using UnityEngine;
using System.Collections;

public class BehaviourHunger	: MonoBehaviour {

	public int hunger = 100;

	private BehaviourDamagable stats;

	void Awake()
	{
		stats = GetComponent<BehaviourDamagable>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	void onUseTurn() {
		hunger -= 1;

		//TODO: More general version
		if (hunger < -100) {
			stats.life_regen_mod = -1.0f;
		}
		else if (hunger < -50) {	
			stats.life_regen_mod = 0.0f;
		}
		else if (hunger < -25) {
			stats.life_regen_mod = 0.50f;
		}
		else if (hunger < 0) {
			stats.life_regen_mod = 0.75f;
		}
	}

	void Eat(int value){
		hunger += value;
	}
}
