using UnityEngine;
using System.Collections;

public class GameTurn : MonoBehaviour {

	public GameObject player;
	public BehaviourSkillList uSkillList;
	public GameObject world; // World is where all entities live. They will all recieve onTurn events

	private Level Lvl;

	// Use this for initialization
	void Start () {
		Lvl = GameObject.FindWithTag("GM").GetComponent<Level>();
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

		foreach (GameObject e in Lvl.GetActingEntities()) {
			// TODO: Level needs to remove the references itself
			if (e == null) {
				continue;
			}
			e.BroadcastMessage("onUseTurn");
		}

		//world.BroadcastMessage("onUseTurn");

		//Update skill list
		uSkillList.OnTurn();
	}
}
