using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ObjectPoolType
{
    public GameObject poolObject;
	public Transform initTransform;
	[HideInInspector]
	public Stack<GameObject> poolDeactiveObjects;
	[HideInInspector]
	public int activeObjects;
}

public class GM_ObjectPool : MonoBehaviour
{
    public static GM_ObjectPool instance;
	public ObjectPoolType[] _poolTypes;
	[HideInInspector]
	public Dictionary<string, ObjectPoolType> objectPoolMap = new Dictionary<string, ObjectPoolType>();
	private int maxObj;

    void Awake()
    {
        instance = this;
		maxObj = 20;
//    }

  //  void Start()
    //{
		for (int i = 0; i < _poolTypes.Length; i++)		// different enemies
        {
			_poolTypes[i].activeObjects = 0;
			_poolTypes[i].poolDeactiveObjects = new Stack<GameObject>();
			objectPoolMap.Add (_poolTypes[i].poolObject.tag, _poolTypes [i]);
        }
    }

	public GameObject GetPoolObject(string _str)
	{
		GameObject obj;

		if (objectPoolMap [_str].poolDeactiveObjects.Count > 0) {
			obj = objectPoolMap [_str].poolDeactiveObjects.Pop ();
		} else if (objectPoolMap [_str].activeObjects < maxObj) {
			obj = (GameObject)Instantiate(objectPoolMap[_str].poolObject);
		} else{
			Debug.Log (_str + ": deactive : " + objectPoolMap [_str].poolDeactiveObjects.Count);
			Debug.Log (_str + ": lives : " + objectPoolMap [_str].activeObjects);
			return null;
		}

		obj.transform.position = objectPoolMap [_str].initTransform.position;
		obj.transform.rotation = objectPoolMap [_str].initTransform.rotation;
		objectPoolMap [_str].activeObjects++;
		return obj;
    }

	public bool RefreshPoolObject(string _str, GameObject obj)
	{
		objectPoolMap [_str].poolDeactiveObjects.Push (obj);
		objectPoolMap [_str].activeObjects--;
		return true;
	}

}