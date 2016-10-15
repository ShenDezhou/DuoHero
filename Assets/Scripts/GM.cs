using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GM : MonoBehaviour
{
    public static GM main;
    public Image gameOverImg;
    public Button restartGame, returnHomepage;
    [HideInInspector]
    public bool gameOver = false;
    [HideInInspector]
    public int scoreTotal;
	[HideInInspector]
	public int birdno;
	int i;
	float deltaBirdTime;

    void Awake()
    {
        main = this;
        scoreTotal = 0;
        gameOverImg.gameObject.SetActive(false);
        restartGame.gameObject.SetActive(false);
        returnHomepage.gameObject.SetActive(false);
		birdno = 0;
		deltaBirdTime = 0.1f;
    }

    void Start()
    {
		GM_Sounds.main.PlaySounds("MainBGM", true, 0);
	}

	void Update()
    {
//		mainCamera.transform.Translate (Vector3.right*Time.deltaTime);
        if (gameOver)
            Invoke("EndGame", 1f);

		if ((birdno == 0) && (deltaBirdTime < 0.5f)) {
			birdno = Random.Range (4, 10);
			deltaBirdTime = Random.Range (10.0f, 15.0f);
			for (i = 0; i < birdno; i++) {
				GameObject en = GM_ObjectPool.instance.GetPoolObject ("bird");
				if (en == null) {
					Debug.Log ("failed to enerate a bird!");
				}

				en.SetActive (true);
			}
		}

		deltaBirdTime = Mathf.Max (0.0f, (deltaBirdTime - Time.deltaTime));
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1;
    }
    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void EndGame()
    {
        gameOverImg.gameObject.SetActive(true);
        restartGame.gameObject.SetActive(true);
        returnHomepage.gameObject.SetActive(true);
//		GM_Sounds.main.PlaySounds("MainBGM", false, 0);
		GM_Sounds.main.StopSounds ();
		GM_Sounds.main.PlaySounds("GameOverBGM", true, 1);
        Time.timeScale = 0;
    }
}