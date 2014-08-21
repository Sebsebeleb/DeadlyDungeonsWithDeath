using UnityEngine;
using System.Collections;

public class BehaviourHunger : MonoBehaviour {

	public int hunger = 100;

	// Use this for initialization
	void Start () {
	
	}
	
	void onUseTurn() {
		hunger -= 1;
	}
}
