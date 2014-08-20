using UnityEngine;
using System.Collections;


public class Gui : MonoBehaviour {

	public GameObject player;
	public Texture2D iconHealth;
	public GUIStyle style;

	private BehaviourDamagable playerDamage;


	void Awake(){
		playerDamage = player.GetComponent<BehaviourDamagable>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnGUI(){
		string hp_string = playerDamage.hp.ToString();
		GUI.Box (new Rect (25,25,100,50), new GUIContent(hp_string, iconHealth), style);
	}
}
