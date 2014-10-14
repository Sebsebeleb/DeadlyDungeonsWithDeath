using UnityEngine;
using System.Collections;

public class FireballSkill : Skill {

    private string _name = "Fireball";

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
        int NumTargets = GameTargeting.GetTarget(caster, TargetingScheme.UNOCCUPIED, (int)caster.transform.position.x, (int)caster.transform.position.y, 1);
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

        Level lvl = GameObject.FindGameObjectWithTag("GM").GetComponent<Level>();
        Vector2 casterPos = new Vector2(caster.transform.position.x, caster.transform.position.y);
        Vector2 spawnDir = new Vector2(pos.x - casterPos.x, pos.y - casterPos.y);


        GameObject fireball = lvl.MakeProjectile(casterPos + spawnDir, spawnDir);
        Sprite fb = Resources.Load<Sprite>("Fireball");
        Debug.Log(fb);
        fireball.GetComponent<SpriteRenderer>().sprite = fb;
        BehaviourProjectile fireProj = fireball.GetComponent<BehaviourProjectile>();
        fireProj.SetMovementDirection((int)spawnDir.x, (int)spawnDir.y);

        UseResources(caster);


    }

    public override string GetTooltip(GameObject who)
    {
        return "Shoot a fireball in the specified direction, traveling untill it hits something when it will explode";
    }
}
