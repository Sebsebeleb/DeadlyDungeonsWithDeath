﻿using UnityEngine;
using System.Collections;

public class USkillItemBehaviour : MonoBehaviour {

	public Skill skill; // Skill to be activated upon button press

	private GameObject Player;

	void Awake() {
		Player = GameObject.FindGameObjectWithTag("Player");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Activate(){
		Debug.Log("Hello?");
		if (skill.CanCast(Player)) {
			skill.Cast(Player);
			skill.UseResources();
		}
		// TODO: temp solution 
		transform.parent.transform.parent.gameObject.SetActive(false);
	}
}