using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Game_Start : MonoBehaviour
{
    public GameObject title;
    public Image tutsImg;
    public Button startButton, tutsButton, returnButton;

    void Awake()
    {
        title.SetActive(false);
        tutsImg.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        tutsButton.gameObject.SetActive(false);
        returnButton.gameObject.SetActive(false);
    }
    void Start()
    {
		GM_Sounds.main.PlaySounds("HomepageBGM", true, 0);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void ShowTutsPage()
    {
        tutsImg.gameObject.SetActive(true);
        returnButton.gameObject.SetActive(true);

        startButton.gameObject.SetActive(false);
        tutsButton.gameObject.SetActive(false);
    }
    public void ReturnToHomepage()
    {
        startButton.gameObject.SetActive(true);
        tutsButton.gameObject.SetActive(true);
        
        tutsImg.gameObject.SetActive(false);
        returnButton.gameObject.SetActive(false);
    }
}