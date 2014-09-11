using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BehaviourSkillList : MonoBehaviour {

	public GameObject prefabSkillItem;

	private BehaviourSkills pSkills;

	void Awake() {
		pSkills = GameObject.FindWithTag("Player").GetComponent<BehaviourSkills>();
		UpdateList();
		transform.parent.gameObject.SetActive(false);
		
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void UpdateList() {

		//Delete children then add list items
		foreach (Transform item in transform) {
			Destroy(item.gameObject);
		}

		foreach(ISkill skill in pSkills.knownSkills){
			GameObject new_item = Instantiate(prefabSkillItem, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			new_item.transform.parent = transform;
			Text skillText = new_item.transform.FindChild("SkillText").GetComponent<Text>();
			skillText.text = skill.SkillName;
			UISkillItemBehaviour behaviourSkillItem = new_item.GetComponent<UISkillItemBehaviour>();
			behaviourSkillItem.skill = skill;
		}
	}
}
