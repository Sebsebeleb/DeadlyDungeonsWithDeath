using UnityEngine;
using System.Collections;

public class ArcSkill : Skill{

	private string _name = "Arc";

	public override string name {
		get {
			return _name;
		}
		set {
			_name = value;
		}
	}

	public ArcSkill(){
		cooldown = 8;
	}

	public override void Cast(GameObject caster) {
		for (int i = 0; i < 4; i++) {
			BehaviourMovement movement = caster.GetComponent<BehaviourMovement>();
			movement.Rotate(-1);
		}
	}

	public override string GetTooltip(GameObject who) {
		return "Spin in a half circle with your weapon";
	}

}
