using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyWeapon : MonoBehaviour
{
	public int damage;

	Animator acDmgImg;
	Image damageImage;
	private Player attackedPlayer;
	private Transform damageTransform;
	private Animator playerAnim;
	private GameObject blood;

	void Awake()
	{
		damageImage = GameObject.Find ("Player_Man").GetComponentInChildren<Player> ().damageImage;
		acDmgImg = damageImage.GetComponent<Animator>();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.transform.parent.gameObject.CompareTag ("Player")) {
			
			attackedPlayer = other.gameObject.GetComponent<Player> ();
			attackedPlayer.currentHealth -= damage;
//			Debug.Log ("kick!!!");
			//flash the screen when hurt
			if (attackedPlayer.currentHealth > 0)
				acDmgImg.SetTrigger("damaged");

			playerAnim = other.gameObject.GetComponent<Animator> ();
			playerAnim.SetTrigger ("damaged");

			if (gameObject.CompareTag ("NoviceWeapon")) {
				damageTransform = other.transform.FindChild ("Transform_Blood_Enemy_Novice");
				blood = GM_ObjectPool.instance.GetPoolObject ("Blood_Novice");
			}
			else if(gameObject.CompareTag ("ArcherWeapon")){
				damageTransform = other.transform.FindChild ("Transform_Blood_Enemy_Archer");
				blood = GM_ObjectPool.instance.GetPoolObject("Blood_Archer");
			}

			if (blood != null) {
				blood.transform.position = damageTransform.position;
				blood.transform.rotation = damageTransform.rotation;
				blood.SetActive (true);
			} else {
				return;
			}
		}
    }
}
