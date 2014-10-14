using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {

	private const string secretKey = "v1"; // Edit this value and make sure it's the same as the one stored on the server
    private const string addScoreURL = "http://bbg.terminator.net/bbg/ddwd/addscore.php?"; //be sure to add a ? to your url
    private const string highscoreURL = "http://badlybadgames.com/bbg/ddwd/display.php";

	private bool EntryBlue = true; //Should the next entry be blue or gray
	public Color blue = new Color(0.858f, 0.709f, 0.509f, 1f);
	public Color gray = new Color(0.427f, 0.427f, 0.427f, 1f);

	public Text tHighscores;
	public GameObject PrefabHighscoreEntry;
	public Transform uHighscoreList;

	void Start() {
		StartCoroutine(GetScores());
	}

	public IEnumerator PostScores(string name, int score) {
		//This connects to a server side php script that will add the name and score to a MySQL DB.
		// Supply it with a string representing the players name and the players score.
		string hash = Utils.Md5Sum(name + score + secretKey);

		string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;

		// Post the URL to the site and create a download object to get the result.
		WWW hs_post = new WWW(post_url);
	    Debug.Log("HEEELOOOO?");
        Debug.Log(hs_post.url);
		yield return hs_post; // Wait until the download is done

	    Debug.Log(hs_post.url);
        Debug.Log(hs_post.bytesDownloaded);

		Debug.Log(hs_post.text);

		if (hs_post.error != null) {
			print("There was an error posting the high score: " + hs_post.error);
		}
	
		//Update scores after updating?
		StartCoroutine(GetScores());
	}

	IEnumerator GetScores() {
		//tHighscores.text = "Loading Scores";
		WWW hs_get = new WWW(highscoreURL);
		yield return hs_get;

		if (hs_get.error != null) {
            
			print("There was an error getting the high score: " + hs_get.error);
		}
		else {
			UpdateScores(hs_get.text);
		}
	}

	private void UpdateScores(string text) {
		EntryBlue = true;

		// Destroy old entries
		foreach (Transform old_e in uHighscoreList.transform) {
			Destroy(old_e.gameObject);
		}

		// Add the updated ones
		string[] scores = new string[40];
		string[] seps = new string[1];
		seps[0] = "\n";
		scores = text.Split(seps, System.StringSplitOptions.RemoveEmptyEntries);
		foreach (string entry in scores) {
			GameObject e = Instantiate(PrefabHighscoreEntry) as GameObject;
			e.transform.parent = uHighscoreList;
			Image im = e.GetComponent<Image>();
			Text t = e.GetComponentInChildren<Text>();
			t.text = entry;

			if (EntryBlue){
				im.color = blue;
			}
			else{
				im.color = gray;
			}
			EntryBlue = !EntryBlue;
		}
	}

}