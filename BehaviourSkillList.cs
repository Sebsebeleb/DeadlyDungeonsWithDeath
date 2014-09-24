using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BehaviourSkillList : MonoBehaviour {

	public GameObject prefabSkillItem;

	private BehaviourSkills pSkills;
	private GameObject player;

	void Awake() {
		player = GameObject.FindWithTag("Player");
		pSkills = player.GetComponent<BehaviourSkills>();
		UpdateList();
		transform.parent.gameObject.SetActive(false);		
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	// Called when a skill is learned
	public void UpdateList() {

		//Delete children then add list items
		foreach (Transform item in transform) {
			Destroy(item.gameObject);
		}

		foreach(Skill skill in pSkills.knownSkills){
			GameObject new_item = Instantiate(prefabSkillItem, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			new_item.transform.parent = transform;
			Text skillText = new_item.transform.FindChild("SkillText").GetComponent<Text>();
			skillText.text = skill.SkillName;
			USkillItemBehaviour behaviourSkillItem = new_item.GetComponent<USkillItemBehaviour>();
			Debug.Log(behaviourSkillItem);
			Debug.Log(skill);
			behaviourSkillItem.skill = skill;
		}
	}

	// Called each turn
	public void OnTurn() {
		foreach (Transform item in transform) {
			USkillItemBehaviour behaviourSkillitem = item.GetComponent<USkillItemBehaviour>();
			Skill skill = behaviourSkillitem.skill;


			Image im = item.GetComponent<Image>();
			if (!skill.CanCast(player)) {
				im.color = Color.red;
				//im.color = new Color(1.0f, 0.3f, 0.3f, im.color.a);
			}
			else {
				im.color = Color.white;
				//im.color = new Color(1.0f, 1.0f, 1.0f, im.color.a);
			}
		}
	}
}
