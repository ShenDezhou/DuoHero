using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player_Man_Clips : MonoBehaviour
{
	Transform leftSide, rightSide;

    void Awake()
    {
		leftSide = GameObject.Find("GM").transform.FindChild ("HeroL").transform;
		rightSide = GameObject.Find("GM").transform.FindChild ("HeroR").transform;
    }

	void frame_SwitchSide()
	{
		if (transform.parent.position.Equals (leftSide.position)) {
			transform.parent.position = rightSide.position;
			transform.parent.rotation = rightSide.rotation;
		} else {
			transform.parent.position = leftSide.position;
			transform.parent.rotation = leftSide.rotation;
		}
	}

	// include "Player_Man_Attack_A", "Player_Man_Attack_B", "Player_Man_Sheath"
	void frame_PlayManSound(string audioName)
	{
		GM_Sounds.main.PlaySounds(audioName, true, 1);
	}

	void frame_GameOver()
	{
		GM.main.gameOver = true;
	}
}