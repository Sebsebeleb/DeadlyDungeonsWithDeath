using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BehaviourSkills : MonoBehaviour {


	public List<Skill> knownSkills = new List<Skill>();

	public BehaviourSkillList uSkillList;

	// Use this for initialization
	void Start () {
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
			skill.OnRegen();
		}
	}
}
