using UnityEngine;
using System.Collections;

public class BrainBurrower : MonoBehaviour {

	private BehaviourMovement player_movement;
	private BehaviourMovement movement;

	public Sprite sprVuln;
	public Sprite sprInvuln;

	private int state = 0; // 0 is burrowign and invulnerable, 1 is out of ground and vulnerable
	private int attackTimer = 0; //After attacking we will stay vulnerable for a couple of turns where we dont do anything

	void Awake() {
		movement = gameObject.GetComponent<BehaviourMovement>();
		player_movement = GameObject.FindWithTag("Player").GetComponent<BehaviourMovement>();
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Think(){

		//Move towards the player

		if (state == 0){
			int dx = player_movement.lx - movement.lx;
			int dy = player_movement.ly - movement.ly;

			//If we are next to player, attack it.
			if  (Mathf.Abs(dx) + Mathf.Abs(dy) == 1){
				player_movement.BroadcastMessage("TakeDamage", 1);
				return;
			}

			// Direction to move
			int vx = 0;
			int vy = 0;
			if (Mathf.Abs(dx) > Mathf.Abs(dy)) {
				vx = Mathf.Clamp(dx, -1, 1);
			}
			else {
				vy = Mathf.Clamp(dy, -1, 1);
			}


			if (vx != 0 | vy != 0){
				movement.MoveDirection(vx, vy);
			}
		}
		else{
			attackTimer--;
			if (attackTimer < 0){
				_setState(0);
				BroadcastMessage("setInvulnerable", false);
			}
		}
	}

	private void _setState(int s){
		state = s;
		SpriteRenderer r = GetComponent<SpriteRenderer>();
		if (s == 1){
			r.sprite = sprVuln;
		}
		else{
			r.sprite = sprInvuln;
		}
	}
}
