using UnityEngine;
using System.Collections;

public class GameManagement : MonoBehaviour {

	public GameObject HelpScreen;
	public GameObject uHighscores;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt("NotFirstTime") == 0) {
			HelpScreen.SetActive(true);
			PlayerPrefs.SetInt("NotFirstTime", 1);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void QuitGame(){
		Application.LoadLevel(0);
	}

	public void LoseGame() {

		int score = GetScore();
		uHighscores.SetActive(true);

		HighscoreManager hsManage = uHighscores.GetComponent<HighscoreManager>();
		hsManage.player_score = score;
	}

	private int GetScore() {
		GameObject player = GameObject.FindWithTag("Player");
		PlayerInventory inventory = player.GetComponent<PlayerInventory>();
		return inventory.gold;
	}
}
