using UnityEngine;
using System.Collections;

public class CleaveSkill : Skill{

	private string _name = "Cleave";

	public override string name {
		get {
			return _name;
		}
		set {
			_name = value;
		}
	}

	public CleaveSkill() {
		cooldown = 8;
	}

	public override void Cast(GameObject caster) {
		BehaviourMovement movement = caster.GetComponent<BehaviourMovement>();
		int x, y;

		// TODO: Animation
		for (int i = 0; i < 8; i++) {
			movement.Rotate(1);
		}
	}
	
	public override string GetTooltip(GameObject who) {
		return "Spin completely around clockwise in one quick motion";
	}

}
