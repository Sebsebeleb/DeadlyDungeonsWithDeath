using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	public GameObject UHealth;
	public GameObject UHunger;

	public GameObject Player;

	private Text HealthText;
	private Text HungerText;

	private BehaviourDamagable playerDamage;
	private BehaviourHunger playerHunger;


	void Awake() {
		HealthText = UHealth.GetComponent<Text>();
		HungerText = UHunger.GetComponent<Text>();
		playerDamage = Player.GetComponent<BehaviourDamagable>();
		playerHunger = Player.GetComponent<BehaviourHunger>();
	}


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		HealthText.text = playerDamage.hp.ToString()+"/"+playerDamage.max_hp.ToString();
		HungerText.text = playerHunger.hunger.ToString();
	}
}
