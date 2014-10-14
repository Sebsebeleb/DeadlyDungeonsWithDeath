using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null) {
			Vector3 ppos = player.transform.position;
			transform.position = new Vector3(ppos.x, ppos.y, ppos.z);
		}
	}
}
