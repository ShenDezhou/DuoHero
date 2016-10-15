using UnityEngine;
using System.Collections;

public class RemoveSelf : MonoBehaviour
{
	GameObject obj;
    //function to remove self (e.g. niddles, arrows, blood)
	void frame_RemoveSelfFunction()
    {
		obj = transform.parent.gameObject;
		obj.SetActive(false);
		GM_ObjectPool.instance.RefreshPoolObject(obj.tag, obj);
    }
}