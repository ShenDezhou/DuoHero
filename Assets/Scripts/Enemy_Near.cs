using UnityEngine;
using System.Collections;

public class Enemy_Near : MonoBehaviour {
	public float distanceToAttack;
	public float attackTimeMin;
	public float attackTimeMax;

	float attackTimeTrigger;

	Animator ownerAnim;
	private Enemy ownerEnemy;
	AnimatorStateInfo stateInfo;
	bool oncePerAttack;

	// Use this for initialization
	void Awake()
	{
		ownerAnim = GetComponent<Animator>();
		ownerEnemy = transform.parent.GetComponent<Enemy> ();
		attackTimeTrigger = 0;
		oncePerAttack = false;
	}

	// Update is called once per frame
	void Update () {
		stateInfo = ownerAnim.GetCurrentAnimatorStateInfo(0);
		if ((stateInfo.IsTag ("nearEnemy_walk")) || stateInfo.IsTag("nearEnemy_die")) {
			oncePerAttack = false;
			if (Mathf.Abs (ownerEnemy.objTransform.position.x) < distanceToAttack) {
				ownerAnim.SetTrigger ("attack");
			} else {
				ownerEnemy.objTransform.Translate (ownerEnemy.speed * Time.deltaTime, 0, 0);
			}
		}else if (stateInfo.IsTag ("nearEnemy_idle")) {
			oncePerAttack = false;
			attackTimeTrigger -= Time.deltaTime;
			ownerAnim.SetFloat ("TimeTrigger", attackTimeTrigger);
		} else if(stateInfo.IsTag("nearEnemy_attack")){
			if(!oncePerAttack){
				oncePerAttack = true;
				attackTimeTrigger = Random.Range(attackTimeMin, attackTimeMax);
				ownerAnim.SetFloat ("TimeTrigger", attackTimeTrigger);
			}
		} else if(stateInfo.IsTag("nearEnemy_damage")){
			ownerEnemy.objTransform.Translate(-ownerEnemy.speed * Time.deltaTime * 0.7f, 0, 0);
			ownerAnim.ResetTrigger("attack");
		}

		if ((ownerEnemy.currentHealth <= 0) && (!stateInfo.IsTag("nearEnemy_die"))) {
			ownerAnim.SetTrigger ("die");
			GM.main.scoreTotal += ownerEnemy.score;
		}
	}

	// include "Enemy_BigGuy_Attack", "Enemy_BigGuy_Die", "Enemy_BigGuy_Hurt", "Enemy_Novice_Attack", "Enemy_Novice_Die"
	void frame_PlayNearEnemySound(string soundName)
	{
		GM_Sounds.main.PlaySounds(soundName, true, 1);
	}

	void frame_RemoveEnemy()
	{
		ownerEnemy.disableSelf (gameObject.tag);
	}
}
