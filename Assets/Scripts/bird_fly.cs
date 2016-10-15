using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class bird_fly : MonoBehaviour {
	float pixelPerUnit, pixelWidth, pixelHeight;
	bool birdTrigger;
	float birdScale;
	SpriteRenderer birdSprite;
	BoxCollider2D birdCollider;

	void Awake(){
		pixelPerUnit = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referencePixelsPerUnit;
		pixelWidth = GameObject.Find("Canvas").GetComponent<CanvasScaler> ().referenceResolution.x;
		pixelHeight = GameObject.Find ("Canvas").GetComponent<CanvasScaler> ().referenceResolution.y;
		pixelWidth = pixelWidth / pixelPerUnit;
		pixelHeight = pixelHeight / pixelPerUnit;
		birdTrigger = true;
		birdSprite = GetComponent<SpriteRenderer> ();
		birdCollider = GetComponent<BoxCollider2D> ();
	}

	// Use this for initialization
	void Start () {
//		transform.position = new Vector3 (-4+Random.Range(-1.0f, 1.0f), 3+Random.Range(-1.0f, 1.0f), 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (birdTrigger) {
			birdTrigger = false;
			transform.position = new Vector3 (Random.Range(-9.0f, 9.0f), Random.Range(-5.0f, -2.0f), 0);
			birdScale = Random.Range (0.3f, 1.5f);
			transform.localScale = new Vector3(birdScale, birdScale, 1.0f);
			transform.eulerAngles = new Vector3 (0, 0, Random.Range(-50.0f, 50.0f));
			birdSprite.color = new Color (birdSprite.color.r, birdSprite.color.g, birdSprite.color.b, 0);
		}

		transform.Translate(Vector2.up * Time.deltaTime * 1.5f);

		if ((Mathf.Abs (transform.position.x) * 2 > pixelWidth) || (Mathf.Abs (transform.position.y) * 2 > pixelHeight)) {
			gameObject.SetActive (false);
			GM.main.birdno--;
			birdTrigger = true;
			GM_ObjectPool.instance.RefreshPoolObject(gameObject.tag, gameObject);
		}
	}

	void OnEnable(){
		birdCollider.enabled = true;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.name == "Envr_FG_Waterfall") {
			birdSprite.color = new Color (birdSprite.color.r, birdSprite.color.g, birdSprite.color.b, 1);
			birdCollider.enabled = false;
		}
	}
}
