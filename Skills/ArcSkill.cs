using UnityEngine;
using System.Collections;

public class ArcSkill : ISkill{

	private string _name = "Arc";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string SkillName {
		get {
			return _name;
		}
		set {
			_name = value;
		}
	}

	public void Cast(GameObject caster) {
		for (int i = 0; i < 4; i++) {
			BehaviourMovement movement = caster.GetComponent<BehaviourMovement>();
			movement.Rotate(-1);
		}
	}

	public bool CanCast(GameObject who) {
		return true;
	}

	public string GetTooltip(GameObject who) {
		return "Spin in a half circle with your weapon";
	}
}
