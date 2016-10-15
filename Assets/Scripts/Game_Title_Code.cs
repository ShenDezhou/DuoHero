using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game_Title_Code : MonoBehaviour
{
    public Button startButton, tutsButton;

    void ShowStartButton()
    {
        Invoke("StartButtonAppear", 1f);
    }

    void StartButtonAppear()
    {
        startButton.gameObject.SetActive(true);
        tutsButton.gameObject.SetActive(true);
    }
}