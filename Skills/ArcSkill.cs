using System.Collections;
using UnityEngine;

public class ArcSkill : Skill
{
    private string _name = "Arc";

    public new int cooldown = 8;

    public override string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public override IEnumerator Cast(GameObject caster)
    {
        for (int i = 0; i < 4; i++)
        {
            var movement = caster.GetComponent<BehaviourMovement>();
            movement.Rotate(-1);
        }
        UseResources(caster);
        return null;
    }

    public override string GetTooltip(GameObject who)
    {
        return "Spin in a half circle with your weapon";
    }
}