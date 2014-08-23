using UnityEngine;
using System.Collections;

public class ItemFood : MonoBehaviour {

	public int food_value = 20;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PickedUp(GameObject who){
		who.BroadcastMessage("Eat", food_value);
	}
}
