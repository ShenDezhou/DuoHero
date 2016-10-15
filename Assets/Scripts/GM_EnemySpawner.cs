using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//-----------------------Serializable Classes-----------------------
[System.Serializable]
public class EnemyType
{
    public GameObject[] enemyPrefab;
	public float intervalBetweenWaves;
	public bool isFromLeft;

	[HideInInspector]
	public int spawned;
	[HideInInspector]
	public bool startTrigger;

	private int[] enemiesMax = new int[5];

	float intervalBetweenSpawns;
	int randNum;

	public void Run(GM_EnemySpawner es, int a, int b, int c, int d, int e)
    {
		enemiesMax [0] = a;
		enemiesMax [1] = b;
		enemiesMax [2] = c;
		enemiesMax [3] = d;
		enemiesMax [4] = e;
        es.StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
		yield return new WaitForSeconds(intervalBetweenWaves);

		while((enemiesMax[0] > 0) || (enemiesMax[1] > 0) || (enemiesMax[2] > 0) || (enemiesMax[3] > 0) || (enemiesMax[4] > 0))
        {
			randNum = Random.Range (0, 5);
			if (enemiesMax [randNum] <= 0)
				continue;

			enemiesMax [randNum]--;

			GameObject en = GM_ObjectPool.instance.GetPoolObject(enemyPrefab[randNum].tag);
			if (en == null) {
				yield return null;
			}

			if (!isFromLeft) {
				en.transform.position = new Vector3(-1 * en.transform.position.x, en.transform.position.y, en.transform.position.z);
				en.transform.rotation = Quaternion.Euler(en.transform.rotation.x, 180, en.transform.position.z);
			}

			spawned++;
            en.SetActive(true);

			//-----------------------Settings-----------------------
			switch (enemyPrefab[randNum].name)
			{
				case "Enemy_Novice": intervalBetweenSpawns = Random.Range(1, 6) * 0.5f; break;
				case "Enemy_BigGuy": intervalBetweenSpawns = Random.Range(3, 5) * 1f; break;
				case "Enemy_Archer": intervalBetweenSpawns = Random.Range(4, 7) * 1f; break;
				case "Enemy_Rider" : intervalBetweenSpawns = Random.Range(15, 20) * 1f; break;
				case "Enemy_FlyMan" : intervalBetweenSpawns = Random.Range(15, 20) * 1f; break;
            }
            yield return new WaitForSeconds(intervalBetweenSpawns);
        }

		startTrigger = true;
    }
}

//---------------------------------------------------------------------
public class GM_EnemySpawner : MonoBehaviour
{
	public EnemyType wavesL, wavesR;

	void Awake(){
		wavesL.spawned = 0;
		wavesR.spawned = 0;
		wavesL.startTrigger = false;
		wavesR.startTrigger = false;
	}

    void Start()
    {
		wavesL.Run(this, 5, 0, 0, 0, 0);
		wavesR.Run(this, 3, 0, 0, 0, 0);
	}

    void Update()
    {
//		#if _A_
		// to do while K.O. a wave of enemies
		if ((wavesL.startTrigger) && (wavesL.spawned == 0))
		{
			wavesL.startTrigger = false;			// re-initiallize

			//next wave
			wavesL.Run(this, Random.Range(3, 6), Random.Range(1, 4), Random.Range(0, 3), Random.Range(0, 2), 1);
		}

		// to do while K.O. a wave of enemies
		if ((wavesR.startTrigger) && (wavesR.spawned == 0))
		{
			wavesR.startTrigger = false;			// re-initiallize

			//next wave
			wavesR.Run(this, Random.Range(4, 7), Random.Range(1, 4), Random.Range(1, 4), 0, 0);
		}
//		#endif
	}
}