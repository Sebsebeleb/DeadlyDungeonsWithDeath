using UnityEngine;
using System.Collections;

public class GameTurn : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UseTurn(){
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemies){
			//Called by enemy ai
			enemy.BroadcastMessage("Think");
		}
	}
}
