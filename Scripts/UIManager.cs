using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	public GameObject Player;

	//GUI Image objects
	public GameObject UHealth;
	public GameObject UHunger;
	public GameObject UExperience;
	public GameObject UGold;

	public GameObject LevelupDialog;

	//GUI Text objects
	private Text HealthText;
	private Text HungerText;
	private Text ExperienceText;
	private Text GoldText;

	//Player script references
	private BehaviourDamagable playerDamage;
	private BehaviourHunger playerHunger;
	private PlayerExperience playerXP;
	private PlayerInventory playerInventory;


	void Awake() {
		HealthText = UHealth.GetComponent<Text>();
		HungerText = UHunger.GetComponent<Text>();
		ExperienceText = UExperience.GetComponent<Text>();
		GoldText = UGold.GetComponent<Text>();

		playerDamage = Player.GetComponent<BehaviourDamagable>();
		playerHunger = Player.GetComponent<BehaviourHunger>();
		playerXP = Player.GetComponent<PlayerExperience>();
		playerInventory = Player.GetComponent<PlayerInventory>();
	}


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		HealthText.text = playerDamage.hp.ToString()+"/"+playerDamage.max_hp.ToString();
		HungerText.text = playerHunger.hunger.ToString();
		ExperienceText.text = string.Format("{0}/{1}", playerXP.xp, playerXP.XPNeeded());
		GoldText.text = playerInventory.gold.ToString();
	}
}
