using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class HighscoreManager : MonoBehaviour {

	HighScore hs;
	public InputField InpName;

	void Awake() {
		hs = GetComponent<HighScore>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PostHighscore() {
		if (InpName.value == "") {
			return;
		}
		hs.StartCoroutine(hs.PostScores(InpName.value, 2));
	}
}
