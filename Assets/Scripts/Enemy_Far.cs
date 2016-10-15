using UnityEngine;
using System.Collections;

public class Enemy_Far : MonoBehaviour
{
    public float attackTimeMin;
    public float attackTimeMax;
	float attackTimeTrigger;

	private Animator ownerAnim;
	private Enemy ownerEnemy;
	AnimatorStateInfo stateInfo;
	bool oncePerAttack;
	Transform weaponTrans;

	void Awake()
    {
		ownerEnemy = transform.parent.GetComponent<Enemy> ();
        ownerAnim = GetComponent<Animator>();
		attackTimeTrigger = 0;
		oncePerAttack = false;
    }

    void Update()
    {
		stateInfo = ownerAnim.GetCurrentAnimatorStateInfo(0);
		if (stateInfo.IsTag ("farEnemy_readyAttack")) {
			if (!oncePerAttack) {
				oncePerAttack = true;
				attackTimeTrigger = Random.Range (attackTimeMin, attackTimeMax);
			} else {
				attackTimeTrigger -= Time.deltaTime;
			}
			ownerAnim.SetFloat ("attackTimer", attackTimeTrigger);
		} else if(stateInfo.IsTag("farEnemy_attack")){
			oncePerAttack = false;
		}

		if ((ownerEnemy.currentHealth <= 0) && (!stateInfo.IsTag("farEnemy_die"))) {
			ownerAnim.SetTrigger ("die");
			GM.main.scoreTotal += ownerEnemy.score;
		}
    }

	// include "Enemy_BigGuy_Attack", "Enemy_BigGuy_Die", "Enemy_BigGuy_Hurt", "Enemy_Novice_Attack", "Enemy_Novice_Die"
	void frame_PlayFarEnemySound(string soundName)
	{
		GM_Sounds.main.PlaySounds(soundName, true, 1);
	}

    void frame_ShootArrow()
    {
        GameObject arrow = GM_ObjectPool.instance.GetPoolObject("Arrow");
        if (arrow == null)
			return;
		weaponTrans = transform.FindChild ("Transform_Shot_Arrow");
		arrow.transform.position = weaponTrans.position;
		arrow.transform.rotation = weaponTrans.rotation;
        arrow.SetActive(true);
    }

	void frame_RemoveEnemy()
	{
		ownerEnemy.disableSelf (gameObject.tag);
	}
}
