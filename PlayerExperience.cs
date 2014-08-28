using UnityEngine;
using System.Collections;

public class PlayerExperience : MonoBehaviour {

	public float xp = 0.0f;
	int Level = 1;

	public void GiveXP(float exp){
		xp += exp;

		if (xp > XPNeeded()){
			LevelUp();
		}
	}

	void LevelUp(){
		xp -= XPNeeded();
		Level++;
	}

	public float XPNeeded(){
		return Mathf.Round(10+((Level+3) * (Level+3) * 0.4f));
	}
}
