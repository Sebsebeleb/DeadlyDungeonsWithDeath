using UnityEngine;
using System.Collections;

public class WeaponThrow : Skill {
	private string _name = "Weapon Throw";

	public new int cooldown = 8;

	public override string Name {
		get {
			return _name;
		}
		set {
			_name = value;
		}
	}

	public override IEnumerator Cast(GameObject caster) {
        int NumTargets = GameTargeting.GetTarget(caster, TargetingScheme.ENEMIES, (int) caster.transform.position.x, (int) caster.transform.position.y, 4  );
	    if (NumTargets == 0)
	    {
	        GameTargeting.CancelTargeting();
            yield break;
	    }
	    while (GameTargeting.IsTargeting)
	    {
	        yield return null;

        }
	    Vector2 pos = GameTargeting.Result;
        
        UseResources(caster);
	}

	public override string GetTooltip(GameObject who) {
		return "Spin in a half circle with your weapon";
	}

}