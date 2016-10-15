using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Score: MonoBehaviour
{
    public Sprite[] numberSprites = new Sprite[10];
    List<Image> scoreHolders = new List<Image>();

    void Awake()
    {
        for (int i = 1; i <= 5; i++)
			scoreHolders.Add(transform.FindChild("ScoreImg"+i.ToString()).GetComponent<Image>());
    }

    void Update()
    {
        DisplayScore(GM.main.scoreTotal);
    }

    void DisplayScore(int score)
    {
        int i, _start, length;
        string scoreString;

        if ((score > 99999) || (score < 0))
            return;

        scoreString = score.ToString();
        length = scoreString.Length;
        for (i = 0; i < scoreHolders.Count; i++)
            scoreHolders[i].gameObject.SetActive(false);

        _start = (scoreHolders.Count - length) >> 1;
        for (i = 0; i < length; i++)
        {
            scoreHolders[_start + i].gameObject.SetActive(true);
            scoreHolders[_start + i].sprite = numberSprites[scoreString[i] - '0'];
        }
    }
}