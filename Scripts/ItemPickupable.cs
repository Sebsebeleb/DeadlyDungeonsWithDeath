using UnityEngine;
using System.Collections;

public class ItemPickupable : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	bool CanBePickedUp(GameObject who){
		if (who.tag == "Player"){
			return true;
		}
		return false;
	}


	void OnSteppedUpon(GameObject who){
		if (CanBePickedUp(who)) {
			BroadcastMessage("OnPickedUp", who);
		}
	}
}
