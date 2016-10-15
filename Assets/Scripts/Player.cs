using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {
	public int startingHealth;
	public Image damageImage;
	[HideInInspector]
	public int currentHealth;
	private float bleedTimer;
	public float secondsPerHealth;
	Transform bleedParent;
	GameObject[] bleedBar = new GameObject[5];

	private Animator ownerAnim;
	private SpriteRenderer ownerRender;
	private GameObject bloodRefresh;
	private Animator bloodAnim;
	bool onceTrigger, refreshTrigger;
//	float refreshTimeMark;

	// Use this for initialization
	void Start () {
		int i;

		ownerAnim = transform.GetComponent<Animator> ();
		ownerRender = transform.GetComponent<SpriteRenderer> ();

		bloodRefresh = transform.parent.FindChild ("Refresh").gameObject;
		bloodAnim = bloodRefresh.GetComponent<Animator> ();
		bloodRefresh.SetActive (false);
		refreshTrigger = false;

		currentHealth = startingHealth;
		onceTrigger = true;
		bleedTimer = 0.0f;
		bleedParent = transform.parent.FindChild ("Bleed");
		for (i = 0; i < 5; i++) {
			bleedBar [i] = bleedParent.GetChild (i).gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
		int i, currentBleedBar;

		//回血机制
		if (ownerAnim.GetCurrentAnimatorStateInfo (0).IsTag ("player_refresh")) {
			bleedTimer += Time.deltaTime;
			if (bleedTimer > secondsPerHealth) {
				if (currentHealth < startingHealth) {
					bloodRefresh.SetActive (true);
					bloodAnim.SetTrigger ("beginRefresh");
					currentHealth++;
					refreshTrigger = true;
//					refreshTimeMark = Time.time;
				} else {
					bloodRefresh.SetActive (false);
					refreshTrigger = false;
				}
				bleedTimer -= secondsPerHealth;
			}
		} else {
			bloodRefresh.SetActive (false);
			refreshTrigger = false;
			bleedTimer = 0.0f;
		}

		if(refreshTrigger)
			//ownerRender.color = new Color(1, 1, 1, Mathf.Abs(Mathf.Cos(Time.time-refreshTimeMark)));
			ownerRender.color = new Color(1, 1, 1, (1 - Random.value*Time.timeScale));
		else
			ownerRender.color = new Color(1, 1, 1, 1);

		//刷新血条
		currentBleedBar = Mathf.Max (0, Mathf.CeilToInt ((currentHealth * 5.0f) / startingHealth));
		for(i=0; i<currentBleedBar; i++)
			bleedBar [i].SetActive (true);
		for (; i < 5; i++)
			bleedBar [i].SetActive (false);

		//血尽死亡
		if ((currentHealth <= 0) && onceTrigger)
		{ 
			onceTrigger = false;
			currentHealth = 0;
			ownerAnim.SetTrigger("die");
		}
	}
}
