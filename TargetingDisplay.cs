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


    public void DisplayTarget(Vector2 pos)
    {

        GameObject target = Instantiate(PrefabTarget, new Vector3(pos.x, pos.y, 0), Quaternion.identity) as GameObject;
        if (target != null)
        {
            target.transform.parent = World;
        }
        targetList.Add(target);
    }

    public void RemoveTargets()
    {
        foreach (GameObject target in targetList)
        {
            Destroy(target);
        }
    }
}
