using UnityEngine;
using System.Collections;

public class ItemGold : MonoBehaviour {

	public int gold_min = 1;
	public int gold_max = 1;

	private int gold_value;

	// Use this for initialization
	void Start () {
		gold_value = Random.Range(gold_min, gold_max);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPickedUp(GameObject who) {
		who.BroadcastMessage("GiveGold", gold_value);
		Destroy(gameObject);
	}
}
