using UnityEngine;
using System.Collections;

public class Request : MonoBehaviour {
	
	void Start () {
		string url = "http://139.196.202.91:8888/rest/duohero/level";
		WWW www = new WWW(url);
		StartCoroutine(WaitForRequest(www));
	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;

		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.text);
			System.IO.File.WriteAllText(Application.streamingAssetsPath + "/stage.json",www.text);
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}    
	}
}
