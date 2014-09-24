using UnityEngine;
using System.Collections;

public class ArcSkill : Skill{

	private string _name = "Arc";
	
	public int cooldown = 8;
	private int _counterCooldown = 0;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override string SkillName {
		get {
			return _name;
		}
		set {
			_name = value;
		}
	}

	public override void Cast(GameObject caster) {
		for (int i = 0; i < 4; i++) {
			BehaviourMovement movement = caster.GetComponent<BehaviourMovement>();
			movement.Rotate(-1);
		}
	}

	public override bool CanCast(GameObject who) {
		return (_counterCooldown <= 0);
	}

	public override void OnRegen() {
		_counterCooldown--;
	}

	public override string GetTooltip(GameObject who) {
		return "Spin in a half circle with your weapon";
	}

	public override void UseResources() {
		_counterCooldown = cooldown;
	}
}
