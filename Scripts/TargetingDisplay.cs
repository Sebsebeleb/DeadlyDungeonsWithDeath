using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System.Collections;

public class TargetingDisplay : MonoBehaviour
{

    public GameObject PrefabTarget;
    public Transform World;

    private List<GameObject> targetList = new List<GameObject>();

    public void Update()
    {
        if (GameTargeting.IsTargeting)
        {
            //Display stuff
        }
    }


    public void DisplayTarget(paramsTargetingDisplay p)
    {
        Vector2 pos = p.pos;
        TargetType typ = p.typ;

        GameObject target = Instantiate(PrefabTarget, new Vector3(pos.x, pos.y, 0), Quaternion.identity) as GameObject;
        if (target != null)
        {
            target.transform.parent = World;
        }
        targetList.Add(target);

        // Color it depending on targeting type
        switch (typ)
        {
            case TargetType.ENEMY:
            {
                target.renderer.material.color = Color.red;
                break;
            }
            case TargetType.ALLY:
            {
                target.renderer.material.color = Color.blue;
                break;
            }
            case TargetType.UNOCCUPIED:
            {
                target.renderer.material.color = Color.yellow;
                break;
            }
        }
    }

    public void RemoveTargets()
    {
        foreach (GameObject target in targetList)
        {
            Destroy(target);
        }
    }
}
