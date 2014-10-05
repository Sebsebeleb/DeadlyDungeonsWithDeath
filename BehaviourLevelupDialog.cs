using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BehaviourLevelupDialog : MonoBehaviour {

	private int _chosen = 0;
	private Skill[] skillChoices = new Skill[3];

	//References to the toggle ui elements
	public GameObject[] uToggles = new GameObject[3];

	public Text uLevelupText;

	private GameObject player;
	private BehaviourSkills pSkills;
	private PlayerExperience pExp;

	void Awake() {
		player = GameObject.FindGameObjectWithTag("Player");
		pSkills = player.GetComponent<BehaviourSkills>();
		pExp = player.GetComponent<PlayerExperience>();
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetChosen(int i) {
		_chosen = i;
	}

	public void Confirm() {
		pSkills.LearnSkill(skillChoices[_chosen]);
	}

	public void UpdateDialog() {
		uLevelupText.text = "Level up! You are now level " + pExp.Level;
	}

	public void setLearnable(Skill[] skills) {
		// We assume 3 skills for the moment
		skillChoices = skills;

		for (int i = 0; i < 3; i++) {
			Skill sk = skills[i];
			GameObject toggle = uToggles[i];

			Text skillName = toggle.GetComponentInChildren<Text>();
			skillName.text = sk.Name;
		}
	}	
}
