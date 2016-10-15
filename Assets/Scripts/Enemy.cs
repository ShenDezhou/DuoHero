using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
	public int startingHealth;
	public float speed;
	public int score;

	[HideInInspector]
	public int currentHealth;
	[HideInInspector]
	public Transform objTransform;
	[HideInInspector]
	public float initTransformX;
	[HideInInspector]
	public int bidno;
	[HideInInspector]
	public bool onlyOnce;

	GM_EnemySpawner enemySpawner;
	Quaternion[] bidrection;
	bool firstFrame;
	float pixelPerUnit, pixelWidth;
	Animator sonAnim;

	// Use this for initialization
	void Awake () {
		objTransform = transform;
		enemySpawner = GameObject.FindWithTag("GameController").GetComponent<GM_EnemySpawner>();
		if (null == enemySpawner) {
			Debug.Log ("enemy.cs null pointer");
		}

		bidrection = new Quaternion[2];
		bidrection [0] = Quaternion.Euler (0, 0, 0);
		bidrection [1] = Quaternion.Euler (0, 180, 0);
		bidno = 0;
		firstFrame = true;
		pixelPerUnit = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referencePixelsPerUnit;
		pixelWidth = GameObject.Find("Canvas").GetComponent<CanvasScaler> ().referenceResolution.x;
		sonAnim = transform.FindChild ("SpriteAnim").GetComponent<Animator>();
		onlyOnce = false;
	}

	void OnEnable()
	{
		currentHealth = startingHealth;
		objTransform = transform;
	}

	// Update is called once per frame
	void Update () {
		if(firstFrame)
		{
			firstFrame = false;
			initTransformX = transform.position.x;
			if (initTransformX < 0)
				bidno = 0;
			else
				bidno = 1;
		}

		if (transform.tag == "Rider") {
			if (Mathf.Abs (transform.position.x) - 0.01f > Mathf.Abs (initTransformX))
				turnBack ();
		} else if(transform.tag == "FlyMan"){
			if (sonAnim.GetCurrentAnimatorStateInfo (0).IsTag ("flyEnemy_Idle")) {
				if (Mathf.Abs (transform.position.x) - 0.01f > (pixelWidth / (2 * pixelPerUnit))) {
					if(((bidno == 0) && (transform.position.x > 0)) || ((bidno == 1) && (transform.position.x < 0)))
						turnBack ();
				}
			}
		}
	}

	public void disableSelf(string enemyName) {
		gameObject.SetActive (false);
		firstFrame = true;
		GM_ObjectPool.instance.RefreshPoolObject(gameObject.tag, gameObject);
		if(initTransformX < 0) {
			enemySpawner.wavesL.spawned--;
		}
		else {
			enemySpawner.wavesR.spawned--;
		}
	}

	public void turnBack(){
		bidno = 1 - bidno;
		transform.rotation = bidrection [bidno];
		transform.position = new Vector3 (Mathf.Sign (transform.position.x) * Mathf.Abs(initTransformX), transform.position.y, transform.position.z);
	}
}
