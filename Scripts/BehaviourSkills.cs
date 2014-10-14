using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BehaviourSkills : MonoBehaviour {


	public List<Skill> knownSkills = new List<Skill>();

	public BehaviourSkillList uSkillList;
	public GameTurn turnManager;

	// Use this for initialization
	void Start () {
		turnManager = GameObject.FindWithTag("GM").GetComponent<GameTurn>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LearnSkill(Skill skill) {
		if (!knownSkills.Contains(skill)) {
			knownSkills.Add(skill);
			uSkillList.UpdateList();
		}
	}

	public void onUseTurn() {
		foreach (Skill skill in knownSkills) {
			skill.OnRegen(gameObject);
		}
	}

	// Called by skills when they want to consume the player's turn
	// TODO: Currently assumes the player is the only one who uses skills
	public void useEnergy() {
		turnManager.UseTurn();
	}
}
