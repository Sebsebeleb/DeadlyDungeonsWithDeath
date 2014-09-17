using UnityEngine;
using System.Collections;

public class GameManagement : MonoBehaviour {

	public GameObject HelpScreen;

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
}
