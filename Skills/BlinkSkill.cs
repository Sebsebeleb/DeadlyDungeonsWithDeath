using UnityEngine;
using System.Collections;

public class BlinkSkill : Skill
{
    private string _name = "Blink";

    public new int cooldown = 8;

    public override string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }

    public override IEnumerator Cast(GameObject caster)
    {
        int NumTargets = GameTargeting.GetTarget(caster, TargetingScheme.UNOCCUPIED, (int)caster.transform.position.x, (int)caster.transform.position.y, 4);
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

        BehaviourMovement movement = caster.GetComponent<BehaviourMovement>();
        movement.ForceMove((int) pos.x, (int) pos.y);
        UseResources(caster);


    }

    public override string GetTooltip(GameObject who)
    {
        return "Blink to an unoccupied tile";
    }

}