using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BehaviourSkills : MonoBehaviour {


	public List<ISkill> knownSkills = new List<ISkill>();

	public BehaviourSkillList uiSkillList;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LearnSkill(ISkill skill) {
		if (!knownSkills.Contains(skill)) {
			knownSkills.Add(skill);
			uiSkillList.UpdateList();
		}
	}

	public void onUseTurn() {
		foreach (ISkill skill in knownSkills) {
			skill.OnRegen();
		}
	}
}
