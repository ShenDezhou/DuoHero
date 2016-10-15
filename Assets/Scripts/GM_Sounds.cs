using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GM_Sounds : MonoBehaviour {
	public static GM_Sounds main;
//	public AudioSource poolAudios;
	List<AudioSource> audioList = new List<AudioSource> (2);
	[HideInInspector]
	public Dictionary<string, AudioSource> audioPoolMap = new Dictionary<string, AudioSource>();

	void Awake()
	{
		main = this;
		GetComponents<AudioSource> (audioList);
		for (int i = 0; i < audioList.Count; i++)		// different enemies
		{
			audioPoolMap.Add (audioList[i].clip.name, audioList[i]);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlaySounds(string audioName, bool whetherPlay, int mode)
	{
		if (null == audioPoolMap [audioName]) {
			Debug.Log ("Failed to find Audio" + audioName);
			return;
		}
/*
		if(!whetherPlay)
			audioPoolMap ["MainBGM"].mute = false;
		else if ((audioName != "MainBGM") && (0 == mode)) {
			audioPoolMap ["MainBGM"].mute = true;
		}
*/
		if (whetherPlay) {
			if (0 == mode)
				audioPoolMap [audioName].Play ();
			else if (1 == mode)
				audioPoolMap [audioName].PlayOneShot (audioPoolMap [audioName].clip);
		} else {
			audioPoolMap [audioName].Stop ();
		}
	}

	public void StopSounds()
	{
		foreach (var audioEnumerator in GM_Sounds.main.audioPoolMap) {
			if(audioEnumerator.Value.isPlaying)
				audioPoolMap [audioEnumerator.Key].Stop ();
		}
	}
}
