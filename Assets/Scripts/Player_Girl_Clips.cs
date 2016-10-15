using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player_Girl_Clips : MonoBehaviour
{
    Animator ac;
	Transform leftSide, rightSide;
	AnimatorStateInfo stateInfo;

    void Awake()
    {
        ac = GetComponent<Animator>();
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

	void frame_Combo()
    {
		ac.SetBool("attackB", GM_PlayerController.gpc.girlAttackB);
    }

	void frame_ShootNiddle()
    {
		GM_Sounds.main.PlaySounds("Player_Girl_Attack", true, 1);

        GameObject niddle = GM_ObjectPool.instance.GetPoolObject("Niddle");
        if (niddle == null) return;
        niddle.transform.position = transform.GetChild(0).position;
        niddle.transform.rotation = transform.GetChild(0).rotation;
        niddle.SetActive(true);
    }

	void frame_GameOver()
	{
		GM.main.gameOver = true;
	}
}