using UnityEngine;
using System.Collections;

public class CleaveSkill : Skill{

	public string _skillName = "Cleave";

	public int cooldown = 8;
	private int _counterCooldown = 0;


	public override string SkillName {
		get { return _skillName; }
		set { _skillName = value; }
	}		

	public override void Cast(GameObject caster) {
		BehaviourMovement movement = caster.GetComponent<BehaviourMovement>();
		int x, y;

		// TODO: Animation
		for (int i = 0; i < 8; i++) {
			movement.Rotate(1);
		}
	}

	public override bool CanCast(GameObject who) {
		return true;
	}

	
	public override string GetTooltip(GameObject who) {
		return "Spin completely around clockwise in one quick motion";
	}

	public override void OnRegen() {
		_counterCooldown--;
	}

	public override void UseResources() {
		_counterCooldown = cooldown;
	}
}
