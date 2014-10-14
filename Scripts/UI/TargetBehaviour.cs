using UnityEngine;
using System.Collections;

public class TargetBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        GameTargeting.SetResult(transform.position);
    }
}
