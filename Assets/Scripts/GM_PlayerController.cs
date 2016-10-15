using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GM_PlayerController : MonoBehaviour
{
    public static GM_PlayerController gpc;
	[HideInInspector]
	public bool girlAttackA, girlAttackB, manAttackA, manAttackB;

	Animator manAnim, girlAnim, chooseEnemyAnim;
    bool switchSide;
	int attackAStateHash = Animator.StringToHash("Base Layer.Player_Girl_AttackA");
	AnimatorStateInfo manStateInfo, girlStateInfo;

    //touch code
	int touchValue;
    //public Vector2 begin, deltaPos;
    //public float deltaTime;
    //bool swipeWindow, touchWindow;
    //touch code
    [HideInInspector]
    public Vector2[] deltaPos;
    public const int MAX_TOUCH = 2;
    bool swipeWindow;

    void Awake()
    {
        deltaPos = new Vector2[MAX_TOUCH];
		manAttackA = false;
		manAttackB = false;
		girlAttackA = false;
		girlAttackB = false;
        gpc = this;
		manAnim = GameObject.Find ("Player_Man").transform.FindChild("SpriteAnim").GetComponent<Animator>();
		girlAnim = GameObject.Find ("Player_Girl").transform.FindChild("SpriteAnim").GetComponent<Animator>();
//		chooseEnemyAnim = GameObject.Find ("Choose_Enemy").transform.GetComponent<Animator> ();
		Time.timeScale = 1;
    }
    void Update()
    {
		touchValue = TouchDispatch ();
		manStateInfo = manAnim.GetCurrentAnimatorStateInfo(0);
		girlStateInfo = girlAnim.GetCurrentAnimatorStateInfo(0);

//		if (Input.GetKeyDown (KeyCode.L)) {
//			chooseEnemyAnim.SetBool ("nextEnemy", true);
//		}

		if (Input.GetKeyDown (KeyCode.Q)) {
			Application.Quit ();
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			Time.timeScale = 1 - Time.timeScale;
		}

		if (Input.GetKeyDown (KeyCode.M)) {
			AudioListener.pause = !AudioListener.pause;
//			foreach (var audioEnumerator in GM.main.audioPoolMap) {
//				audioEnumerator.Value.mute = !audioEnumerator.Value.mute;
//			}
		}

        //touch code
		swipeWindow = (touchValue == 1);
        switchSide = Input.GetKeyDown(KeyCode.Space) || swipeWindow;
		if ((manStateInfo.IsTag ("player_can_switch") || manStateInfo.IsTag ("player_refresh"))
		   && (girlStateInfo.IsTag ("player_can_switch") || girlStateInfo.IsTag ("player_refresh"))) {
			manAnim.SetBool ("switchSide", switchSide);
			girlAnim.SetBool ("switchSide", switchSide);
			if (switchSide)
				GM_Sounds.main.PlaySounds("Player_Switch", true, 1);
		}

		//touch code
		// Man stand left, girl stand right
		if (manAnim.gameObject.transform.parent.position.x < 0) {
			if (Input.GetKeyDown(KeyCode.A) || (touchValue == 2) || (touchValue == 5)) {
				if(!manStateInfo.IsTag("player_can_switch"))
					if (Random.value <= 0.5f)
						manAttackA = true;
					else
						manAttackB = true;
			}

			if (Input.GetKeyDown (KeyCode.L) || (touchValue == 3) || (touchValue == 5)) {
				if (girlStateInfo.fullPathHash != attackAStateHash)
					girlAttackA = true;
				else
					girlAttackB = true;
			}
		}
		// Man stand right, girl stand left
		else {
			if (Input.GetKeyDown(KeyCode.L) || (touchValue == 3) || (touchValue == 5)) {
				if(!manStateInfo.IsTag("player_can_switch"))
					if (Random.value <= 0.5f)
						manAttackA = true;
					else
						manAttackB = true;
			}

			if (Input.GetKeyDown (KeyCode.A) || (touchValue == 2) || (touchValue == 5)) {
				if (girlStateInfo.fullPathHash != attackAStateHash)
					girlAttackA = true;
				else
					girlAttackB = true;
			}
		}

		manAnim.SetBool ("attackA", manAttackA);
		manAnim.SetBool ("attackB", manAttackB);
		manAttackA = false;
		manAttackB = false;

		girlAnim.SetBool("attackA", girlAttackA);
		girlAttackA = false;

		if (girlStateInfo.fullPathHash != attackAStateHash)
			girlAttackB = false;
	}

    //touch code
	public int TouchDispatch()
    {
        int touchResult = 0;

//		if (Input.touchCount > MAX_TOUCH)		//for test
//			return 4;
		
        for (int i = 0; i < Input.touchCount && i < MAX_TOUCH; i++)
        {
            Touch item = Input.GetTouch(i);

            switch (item.phase)
            {
                case TouchPhase.Began:
                    deltaPos[i] = Vector2.zero;
                    break;
                case TouchPhase.Canceled:
                    break;
                case TouchPhase.Ended:
                    if (Mathf.Abs(deltaPos[i].x) > Screen.width * 0.02f)
                    {
                        touchResult = 1;								// slide screen
                        deltaPos[i] = Vector2.zero;
                        break;
                    }
                    else if (Mathf.Abs(deltaPos[i].x) < Screen.width * 0.01f)
                    {
                        if (Input.GetTouch(i).position.x < Screen.width * 0.4f)
                            touchResult = touchResult + 2;				// touch left
                        else if (Input.GetTouch(i).position.x > Screen.width * 0.6f)
							touchResult = touchResult + 3;				// touch right
                    }
                    break;
                case TouchPhase.Moved:
                    deltaPos[i] += item.deltaPosition;
                    break;
                case TouchPhase.Stationary:
                    break;
                default:
                    break;
            }
        }

        return touchResult;
    }
}