using UnityEngine;
using System.Collections;

public class Enemy_Fly : MonoBehaviour {
	Animator ownerAnim;
	Enemy ownerEnemy;
	AnimatorStateInfo stateInfo;
	float jumpTimeStart, flyTimer, yPos, initSpeedY, gPara, yuemo_tree;
	public float initSpeedX, distanceToAttack;
	Vector3 deltaV3, initV3;
	bool getStart;

	// Use this for initialization
	void Awake () {
		ownerAnim = GetComponent<Animator> ();
		ownerEnemy = transform.root.GetComponent<Enemy> ();
		jumpTimeStart = 0.0f;
		yPos = ownerEnemy.transform.position.y;
		ownerAnim.SetBool ("toIdle", false);
	}
	
	// Update is called once per frame
	void Update(){
		stateInfo = ownerAnim.GetCurrentAnimatorStateInfo(0);

		if (ownerEnemy.onlyOnce && (ownerEnemy.currentHealth <= 0) && (!stateInfo.IsTag("flyEnemy_die"))) {
			ownerEnemy.onlyOnce = false;
			ownerAnim.SetTrigger ("die");
			GM.main.scoreTotal += ownerEnemy.score;
		}

		if (stateInfo.IsTag ("flyEnemy_Jump")) {
			if(!getStart){
				getStart = true;
				jumpTimeStart = Time.time;
				initV3 = ownerEnemy.transform.position;
				initSpeedY = Random.Range(20.0f, 22.0f);
				gPara = -2.0f * initSpeedY;
			}
			flyTimer = Time.time - jumpTimeStart;
			deltaV3.x = initV3.x + initSpeedX * flyTimer;
			deltaV3.y = initV3.y + initSpeedY * flyTimer + (0.5f * gPara * flyTimer * flyTimer);
			ownerEnemy.transform.position = deltaV3;

			if (deltaV3.y < (yPos-0.001f)) {
				if (Mathf.Abs (deltaV3.x) < distanceToAttack)
					ownerAnim.SetBool ("toAttack", true);
				else
					ownerAnim.SetBool ("toIdle", true);
			}

			yuemo_tree = initSpeedY + gPara * flyTimer;
			if(yuemo_tree > 5.0f)
				ownerAnim.SetInteger ("yuemo_jump", 0);
			else if(yuemo_tree > -5.0f)
				ownerAnim.SetInteger ("yuemo_jump", 1);
			else if(yuemo_tree > -19.0f)
				ownerAnim.SetInteger ("yuemo_jump", 2);
			else
				ownerAnim.SetInteger ("yuemo_jump", 3);
		} else if (stateInfo.IsTag ("flyEnemy_Idle")) {
			ownerEnemy.transform.position = new Vector3 (deltaV3.x, yPos, deltaV3.z);
			ownerAnim.SetBool ("toIdle", false);
			ownerAnim.SetBool ("toAttack", false);
			getStart = false;
			if (ownerEnemy.bidno == 0) {
				if (deltaV3.x < -9.62f) {		//come on
					initSpeedX = -1.0f * deltaV3.x - distanceToAttack + Random.Range(-0.2f, 0.8f);
				} else if (deltaV3.x < 0.001f) {
					initSpeedX = distanceToAttack + Random.Range (1.5f, 3.0f) - deltaV3.x;
				} else {
					initSpeedX = Random.Range (7.0f, 10.0f);
				}
			} else {
				if (deltaV3.x > 9.62f) {
					initSpeedX = -1.0f * deltaV3.x + distanceToAttack + Random.Range (-0.8f, 0.2f);
				} else if (deltaV3.x > 0.001f) {
					initSpeedX = -1.0f * (distanceToAttack + Random.Range (1.5f, 3.0f) + deltaV3.x);
				} else {
					initSpeedX = Random.Range (-10.0f, -7.0f);
				}
			}
			ownerAnim.SetInteger ("yuemo_jump", 0);
		} else if(stateInfo.IsTag ("flyEnemy_Attack")){
			ownerEnemy.transform.position = new Vector3 (deltaV3.x, yPos-0.7f, deltaV3.z);
		}
	}
		
	void OnEnable()
	{
		ownerEnemy.transform.position = new Vector3 (Random.Range(-14.0f, -11.0f), -0.4f, 0.0f);
//		ownerEnemy.transform.position = new Vector3 (-5.0f, -0.4f, 0.0f);
		initSpeedX = Random.Range(-5.5f, -2.8f) - ownerEnemy.transform.position.x;
		ownerAnim.SetInteger ("yuemo_jump", 0);
		ownerEnemy.onlyOnce = true;
		getStart = false;
	}

	void jumpDone()
	{
//		ownerAnim.SetBool("toIdle", true);
	}

	void frame_RemoveEnemy()
	{
		ownerEnemy.disableSelf (gameObject.tag);
	}
}
