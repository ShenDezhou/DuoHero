using UnityEngine;
using System.Collections;

public class Enemy_Through : MonoBehaviour {

	public float distanceToAttack;

	Animator ownerAnim;
	private Enemy ownerEnemy;
	bool oncePerAttack;
	AnimatorStateInfo stateInfo;
	Transform backup;

	// Use this for initialization
	void Awake()
	{
		ownerAnim = GetComponent<Animator>();
		ownerEnemy = transform.parent.parent.GetComponent<Enemy> ();
		oncePerAttack = true;
	}

	// Update is called once per frame
	void Update () {
		stateInfo = ownerAnim.GetCurrentAnimatorStateInfo(0);
	
		if (Mathf.Abs (ownerEnemy.objTransform.position.x) < distanceToAttack) {
			if (oncePerAttack) {
				ownerAnim.SetTrigger ("attack");
				oncePerAttack = false;
			}
		} else {
			oncePerAttack = true;
		}

		if (stateInfo.IsTag ("throughEnemy_die")) {
			ownerEnemy.objTransform.Translate (ownerEnemy.speed * 0.8f * Time.deltaTime, 0, 0);
			backup.Translate (ownerEnemy.speed * -0.2f * Time.deltaTime, 0, 0);
		}
		else {
			ownerEnemy.objTransform.Translate (ownerEnemy.speed * Time.deltaTime, 0, 0);
		}

		if ((ownerEnemy.currentHealth <= 0) && (!stateInfo.IsTag ("throughEnemy_die"))) {
			ownerEnemy.objTransform.gameObject.SetActive (false);

			GM.main.scoreTotal += ownerEnemy.score;
			backup = transform.parent;
			transform.parent = ownerEnemy.objTransform;

			ownerEnemy.objTransform.gameObject.SetActive (true);

			backup.GetComponent<Animator> ().SetTrigger ("die");
			ownerAnim.SetTrigger ("die");
		}
	}

	void OnBecameVisible()
	{
		GM_Sounds.main.PlaySounds("Enemy_Rider_Run", true, 0);
	}

	void OnBecameInvisible()
	{
		GM_Sounds.main.PlaySounds("Enemy_Rider_Run", false, 0);
	}

	void frame_RemoveEnemy()
	{
		transform.parent = backup;
		GM_Sounds.main.PlaySounds("Enemy_Rider_Run", false, 0);
		ownerEnemy.disableSelf (gameObject.tag);
	}
}
