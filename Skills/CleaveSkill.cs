using UnityEngine;
using System.Collections;

public class CleaveSkill : ISkill{

	public string _skillName = "Cleave";

	public int cooldown = 8;
	private int _counterCooldown = 0;


	public string SkillName {
		get { return _skillName; }
		set { _skillName = value; }
	}		

	public void Cast(GameObject caster) {
		BehaviourMovement movement = caster.GetComponent<BehaviourMovement>();
		int x, y;

		// TODO: Animation
		for (int i = 0; i < 8; i++) {
			movement.Rotate(1);
		}
	}

	public bool CanCast(GameObject who) {
		return true;
	}

	
	public string GetTooltip(GameObject who) {
		return "Spin completely around clockwise in one quick motion";
	}

	public void OnRegen() {
		_counterCooldown--;
	}

	public void UseResources() {
		_counterCooldown = cooldown;
	}
}
