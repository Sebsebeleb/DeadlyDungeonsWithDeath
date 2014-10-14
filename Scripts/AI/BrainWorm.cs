using UnityEngine;
using System.Collections;

public class BrainWorm : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDie() {
		effectBurningAcid acid = new effectBurningAcid();
		
		//level.setOnground(acid)
	}
}
