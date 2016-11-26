using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;

public class SpawnType
{
	public string tag;
	public Vector3 initPosition;
	public float speed;
	public Queue<int> enemyInterval;

	public SpawnType()
	{
		enemyInterval = new Queue<int> ();
	}
}

[System.Serializable]
public class EnemyType
{
	[HideInInspector]
	public bool isFromLeft;
	[HideInInspector]
	public int spawned;

	SpawnType[] enemyKind = new SpawnType[5];
	int[] enemyQueInt = new int[5];

	public void Run(ref Queue<SpawnType> enemyQueue)
	{
		if ((enemyQueue.Count >= 5) && (spawned == 0)) {
			for (int i = 0; i < 5; i++) {
				enemyKind [i] = enemyQueue.Dequeue ();
				enemyQueInt [i] = 0;
				spawned += enemyKind [i].enemyInterval.Count;
			}
		}

		for (int i = 0; i < 5; i++) {
			while ((enemyKind [i].enemyInterval.Count > 0) && (enemyKind [i].enemyInterval.Peek () <= enemyQueInt[i]))
			{
				enemyKind [i].enemyInterval.Dequeue ();
				enemyQueInt [i] = 0;
				GameObject en = GM_ObjectPool.instance.GetPoolObject (enemyKind[i].tag);
				if (en == null) {
					Debug.Log ("enemy null");
				}

				if (!isFromLeft) {
					en.transform.position = new Vector3(-1 * en.transform.position.x, en.transform.position.y, en.transform.position.z);
					en.transform.rotation = Quaternion.Euler(en.transform.rotation.x, 180, en.transform.position.z);
				}
				en.GetComponent<Enemy> ().speed = enemyKind [i].speed;
				en.SetActive (true);
			}

			enemyQueInt[i]++;
		}
	}
}

public class GM_EnemySpawner : MonoBehaviour
{
	[HideInInspector]
	public EnemyType wavesL, wavesR;
	FileStream stage;
	StreamReader sr;
	Queue<SpawnType> enemyQueue = new Queue<SpawnType>();

	void Awake(){
//		string temp;
//		char[] charArray = new char[]{','};
//		char[] comment = new char[]{'/', '/'};

		wavesL.spawned = 0;
		wavesR.spawned = 0;
		wavesL.isFromLeft = true;
		wavesR.isFromLeft = false;

//		if (File.Exists ("Assets/stage.txt")) {
//			stage = new FileStream ("Assets/stage.txt", FileMode.Open, FileAccess.Read);
//		} else {
//			Debug.Log ("file error!");
//		}
//
//		sr = new StreamReader (stage);
//		while ((temp = sr.ReadLine ()) != null) {
//			if (temp.StartsWith ("//"))
//				continue;
//
//			int todelete = temp.IndexOfAny (comment);
//			if(todelete > 0)
//				temp = temp.Remove (todelete);
//
//			string[] strArray = temp.Split (charArray);
//			SpawnType elem = new SpawnType();
//
//			elem.tag = strArray [0];
//			if (strArray.Length > 5) {
//				elem.initPosition = Vector3.zero;
//				elem.speed = System.Convert.ToSingle (strArray [4]);
//			}
//
//			for (int i = 5; i < strArray.Length; i++)
//				elem.enemyInterval.Enqueue (System.Convert.ToInt32 (strArray [i]));
//
//			//			Debug.Log (temp);
//			enemyQueue.Enqueue (elem);
//		}
		// using json
		JSONNode level_json = JSONData.LoadFromFile (Application.streamingAssetsPath + "/stage.json");
		for (int i = 0; i < level_json.Count; i++) {
			SpawnType elem = new SpawnType ();
			elem.tag = level_json [i] ["tag"];
			elem.initPosition = new Vector3(level_json [i] ["pos"][0].AsFloat,level_json [i] ["pos"][1].AsFloat,level_json [i] ["pos"][2].AsFloat);
			elem.speed = level_json [i] ["speed"].AsFloat;
			for (int j = 0; j < level_json [i] ["interval"].Count; j++)
				elem.enemyInterval.Enqueue (level_json [i] ["interval"] [j].AsInt);
			enemyQueue.Enqueue (elem);
		}

	}

	void FixedUpdate()
	{
		wavesL.Run (ref enemyQueue);
		wavesR.Run (ref enemyQueue);
	}

	void OnDestroy()
	{
		stage.Close ();
	}
}
