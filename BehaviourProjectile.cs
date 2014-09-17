using UnityEngine;
using System.Collections;

public class BehaviourProjectile : MonoBehaviour {

	public int dir_x = 0;
	public int dir_y = 0;

	private BehaviourMovement move;
	private bool hasEnergy = false; // We cannot move the first turn we are spawned

	void Awake() {
		move = GetComponent<BehaviourMovement>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetMovementDirection(int x, int y) {
		dir_x = x;
		dir_y = y;

		float rot = 0;
		if (x == -1) {
			rot = 90;
		}
		else if (x == 1) {
			rot = 270;
		}
		else if (y == -1) {
			rot = 180;
		}
		else if (y == 1) {
			rot = 0;
		}
		transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rot));
	}

	void onUseTurn() {
		if (!hasEnergy) {
			hasEnergy = true;
			return;
		}
		bool moved = move.MoveDirection(dir_x, dir_y);
		if (!moved) {
			Destroy(gameObject);
		}
	}
}
