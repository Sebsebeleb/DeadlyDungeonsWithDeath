using UnityEngine;
using System.Collections;

public class BehaviourDownstairs : MonoBehaviour {

	//Level reference
	private Level Lvl;

	void Awake() {
		Lvl =  GameObject.FindWithTag("GM").GetComponent<Level>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnSteppedUpon(GameObject stepper){
		if (stepper.tag == "Player"){
			Lvl.NextLevel(this);
		}
	}
}
