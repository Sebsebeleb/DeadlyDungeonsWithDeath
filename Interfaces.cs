using UnityEngine;
using System.Collections;


public interface IMoveable
{

	void Move(int x, int y);
}

public interface IBrain
{
    void Think();
}

public interface ISkill {

	string SkillName {
		get;
		set;
	}

	void Cast(GameObject caster);

	bool CanCast(GameObject who);

	string GetTooltip(GameObject who);


}