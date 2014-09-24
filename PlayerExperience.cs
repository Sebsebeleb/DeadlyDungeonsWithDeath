using UnityEngine;
using System.Collections;

public class PlayerExperience : MonoBehaviour {

	public GameObject LevelupDialog;

	private BehaviourLevelupDialog bDialog;

	public float xp = 0.0f;
	public int Level = 1;

	public void Start(){
		bDialog = LevelupDialog.GetComponent<BehaviourLevelupDialog>();
	}

	public void GiveXP(float exp){
		xp += exp;

		if (xp > XPNeeded()){
			LevelUp();
		}
	}

	void LevelUp(){
		xp -= XPNeeded();
		Level++;

		LevelupDialog.SetActive(true);
		_updateDialog();
	}

	private void _updateDialog() {
		Skill[] skills = new Skill[3];
		skills[0] = new ArcSkill();
		skills[1] = new CleaveSkill();
		skills[2] = new ArcSkill();

		bDialog.setLearnable(skills);
		bDialog.UpdateDialog();
	}

	public float XPNeeded(){
		return Mathf.Round(10+((Level+3) * (Level+3) * 0.4f));
	}
}
