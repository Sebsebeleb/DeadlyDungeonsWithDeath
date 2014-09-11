using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour {

	private List<GameObject> inventory = new List<GameObject>();
	public int gold;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void GiveGold(int val) {
		gold += val;
	}

	void AddItem(GameObject item) {
		inventory.Add(item);
	}
}
