using UnityEngine;
using System.Collections;

public class ActorStats : MonoBehaviour {

	public int hp;
	public int max_hp;

	public float life_regen = 0.00f; //life regen'd per turn
	public float life_regen_mod = 1.00f;
	private float regen_counter = 0.0f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void onUseTurn(){
		regen_counter += life_regen * life_regen_mod;
		while ( hp < max_hp && regen_counter > 1.0f){
			hp += 1;
			regen_counter -= 1.0f;
		}
	}
}
