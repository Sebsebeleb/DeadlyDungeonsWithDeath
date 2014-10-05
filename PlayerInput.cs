using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	public GameObject player;
	public GameObject menuCanvas;
	public GameObject uSkillList;
	public GameObject uHelpScreen;

	public KeyCode KeyUp;
	public KeyCode KeyDown;
	public KeyCode KeyLeft;
	public KeyCode KeyRight;
	public KeyCode KeyRotateCCW;
	public KeyCode KeyRotateCW;

	private BehaviourMovement playerMove;
	private GameTurn turnScript;
	private Canvas canvas;


	void Awake() {
		playerMove = player.GetComponent<BehaviourMovement>();
		turnScript = GameObject.FindWithTag("GM").GetComponent<GameTurn>();
		//canvas = menuCanvas.GetComponent<Canvas>();
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.F1)) {
			uHelpScreen.SetActive(true);
		}

        //TODO: Should this use the event system isntead?
        if (Input.GetButtonDown("Cancel"))
        {
            //canvas.enabled = !canvas.enabled;
            if (GameTargeting.IsTargeting)
            {
                GameTargeting.CancelTargeting();
            }
        }


        // Dont do actions if targeting
        if (GameTargeting.IsTargeting)
        {
            return;
        }

		//TODO: actually check if stuff should take a turn
		bool used_turn = false;

		if (playerMove == null) {
			return;
		}

		if (Input.GetKeyUp(KeyUp)){
			playerMove.MoveDirection(0, 1);
			used_turn = true;
		}
		else if (Input.GetKeyUp(KeyDown)){
			playerMove.MoveDirection(0, -1);
			used_turn = true;
		}
		else if (Input.GetKeyUp(KeyLeft)){
			playerMove.MoveDirection(-1, 0);
			used_turn = true;
		}
		else if (Input.GetKeyUp(KeyRight)){
			playerMove.MoveDirection(1, 0);
			used_turn = true;
		}

		else if (Input.GetKeyUp(KeyRotateCCW)){
			playerMove.Rotate(-1);
			used_turn = true;
		}
		else if (Input.GetKeyUp(KeyRotateCW)){
			playerMove.Rotate(1);
			used_turn = true;
		}

		if (Input.GetButtonDown("SkillList")) {
			uSkillList.SetActive(!uSkillList.activeSelf);
		}



		//Touch
		//If the menu canvas is enabled we let it handle stuff
		//!canvas.enabled && 
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            Vector2 pos = Input.GetTouch(0).position;
            switch (_getGridSquare(pos.x, pos.y)){
                case 6:
					playerMove.Rotate(-1);
					used_turn = true;
					break;
				case 8:
					playerMove.Rotate(1);
					used_turn = true;
					break;
				case 7:
					playerMove.MoveDirection(0, 1);
					used_turn = true;
					break;
				case 5:
					playerMove.MoveDirection(1, 0);
					used_turn = true;
					break;
				case 3:
					playerMove.MoveDirection(-1, 0);
					used_turn = true;
					break;
				case 1:
					playerMove.MoveDirection(0, -1);
					used_turn = true;
					break;
				case 4:
					canvas.enabled = !canvas.enabled;
					break;

            }
        }

		if (used_turn){
			// TODO: Check dirty etc.
			player.BroadcastMessage("updateFOV");
			turnScript.UseTurn();
		}



	}

	private int _getGridSquare(float x, float y){
		float vx = Mathf.Floor(x / Screen.width * 3);
		float vy = Mathf.Floor(y / Screen.height * 3);
		return (int) (vy * 3 + vx);

	}
}
