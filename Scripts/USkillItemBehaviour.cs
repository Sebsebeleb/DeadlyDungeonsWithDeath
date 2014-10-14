using UnityEngine;
using System.Collections;

public class USkillItemBehaviour : MonoBehaviour {

	public Skill skill; // Skill to be activated upon button press
    public GameManagement gm;

	private GameObject Player;

	void Awake() {
		Player = GameObject.FindGameObjectWithTag("Player");
	    gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManagement>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Activate(){
		if (skill.CanCast(Player)) {
			gm.StartRemoteCoroutine(skill.Cast(Player));
		}
		// TODO: temp solution 
		transform.parent.transform.parent.gameObject.SetActive(false);
	}
}
