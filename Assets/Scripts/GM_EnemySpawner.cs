using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
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
	// read config file for enemy spawn interval.
	JSONNode enemy_json;

	public void Run(GM_EnemySpawner es, int a, int b, int c, int d, int e)
    {
		enemiesMax [0] = a;
		enemiesMax [1] = b;
		enemiesMax [2] = c;
		enemiesMax [3] = d;
		enemiesMax [4] = e;
        es.StartCoroutine(Spawn());
    }

	public void Run(GM_EnemySpawner es, JSONNode enemies_level)
	{
		for (int i = 0; i < enemies_level.Count; i++) {
			enemiesMax [i] = Random.Range (enemies_level [i] ["amount"] [0].AsInt, enemies_level [i] ["amount"] [1].AsInt);
		}
		es.StartCoroutine(Spawn());
	}

    IEnumerator Spawn()
    {
		if (enemy_json==null)
			enemy_json = JSONData.LoadFromFile (Application.streamingAssetsPath + "/duohero.cfg");
		
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
			JSONNode node =  enemy_json [enemyPrefab [randNum].name ];
			intervalBetweenSpawns = Random.Range (node["interval"] [0].AsFloat, node["interval"] [1].AsFloat) * node["interval_ratio"].AsFloat;
//			switch (enemyPrefab[randNum].name)
//			{
//				case "Enemy_Novice": intervalBetweenSpawns = Random.Range(1, 6) * 0.5f; break;
//				case "Enemy_BigGuy": intervalBetweenSpawns = Random.Range(3, 5) * 1f; break;
//				case "Enemy_Archer": intervalBetweenSpawns = Random.Range(4, 7) * 1f; break;
//				case "Enemy_Rider" : intervalBetweenSpawns = Random.Range(15, 20) * 1f; break;
//				case "Enemy_FlyMan" : intervalBetweenSpawns = Random.Range(15, 20) * 1f; break;
//            }
            yield return new WaitForSeconds(intervalBetweenSpawns);
        }

		startTrigger = true;
    }
}

//---------------------------------------------------------------------
public class GM_EnemySpawner : MonoBehaviour
{
	public EnemyType wavesL, wavesR;
	JSONNode level_json ;
	void Awake(){
		wavesL.spawned = 0;
		wavesR.spawned = 0;
		wavesL.startTrigger = true;
		wavesR.startTrigger = true;

	}

    void Start()
    {
		if(level_json==null)
			level_json = JSONData.LoadFromFile (Application.streamingAssetsPath + "/level.cfg");
		
//		wavesL.Run(this, 5, 0, 0, 0, 0);
//		wavesR.Run(this, 3, 0, 0, 0, 0);
	}

    void Update()
    {
//		#if _A_
		// to do while K.O. a wave of enemies
		if ((wavesL.startTrigger))
		{
			wavesL.startTrigger = false;			// re-initiallize
			//next wave
//			wavesL.Run(this, Random.Range(3, 6), Random.Range(1, 4), Random.Range(0, 3), Random.Range(0, 2), 1);
			wavesL.Run(this,level_json[wavesL.spawned++]["wavesL"]);
			//停在最后一关的难度水平
			if (wavesL.spawned == level_json.Count)
				wavesL.spawned--;
		}

		// to do while K.O. a wave of enemies
		if ((wavesR.startTrigger))
		{
			wavesR.startTrigger = false;			// re-initiallize

			//next wave
//			wavesR.Run(this, Random.Range(4, 7), Random.Range(1, 4), Random.Range(1, 4), 0, 0);
			wavesR.Run(this,level_json[wavesR.spawned++]["wavesR"]);
			//停在最后一关的难度水平
			if (wavesR.spawned == level_json.Count)
				wavesR.spawned--;
		}
//		#endif
	}
}