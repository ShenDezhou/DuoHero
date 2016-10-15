using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour
{
    public int damage;

	private Animator gufengAnim;
	private int gufengStateHash;
	int manAttackAStateHash = Animator.StringToHash("Base Layer.Player_Man_AttackA");
	int manAttackBStateHash = Animator.StringToHash("Base Layer.Player_Man_AttackB");

	private Enemy attackedEnemy;
	private Transform damageTransform;
	GameObject blood;

	void Awake()
	{
		if (gameObject.CompareTag ("PlayerManWeapon")) {
			gufengAnim = transform.parent.GetComponent<Animator> ();
		}
	}

    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.transform.CompareTag ("Enemy")) {
			attackedEnemy = other.transform.root.GetComponent<Enemy> ();

			if (!(attackedEnemy.gameObject.CompareTag ("Rider") && gameObject.CompareTag ("PlayerGirlWeapon"))) {
				attackedEnemy.currentHealth -= damage;
				if (attackedEnemy.currentHealth > 0) {
					other.gameObject.GetComponent<Animator> ().SetTrigger ("damaged");
				}
			}

			if (gameObject.CompareTag ("PlayerManWeapon")) {
				gufengStateHash = gufengAnim.GetCurrentAnimatorStateInfo (0).fullPathHash;
				if (gufengStateHash == manAttackAStateHash) {
					blood = GM_ObjectPool.instance.GetPoolObject ("Blood_Man_A");
					damageTransform = other.gameObject.transform.FindChild ("Transform_Blood_Player_Man_A");
				} else if (gufengStateHash == manAttackBStateHash) {
					blood = GM_ObjectPool.instance.GetPoolObject ("Blood_Man_B");
					damageTransform = other.gameObject.transform.FindChild ("Transform_Blood_Player_Man_B");
				}

				if ((blood != null) && (damageTransform != null)) {
					blood.transform.position = damageTransform.position;
					blood.transform.rotation = damageTransform.rotation;
					blood.SetActive (true);
				} else {
					Debug.Log ("PlayerWeapon.cs : " + damageTransform);
					return;
				}
			}
		}
    }
}
