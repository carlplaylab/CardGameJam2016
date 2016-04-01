using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour 
{
	[Range(0.1f, 1f)]
	[SerializeField] private float volume = 0.5f;

	private static SoundManager instance;
	public static SoundManager Instance
	{
		get { return instance; }
	}

	private Dictionary<string, AudioSource> soundList;

	void Awake ()
	{
		instance = this;
		soundList = new Dictionary<string, AudioSource>();
	}

	void OnDestroy ()
	{
		instance = null;
		soundList.Clear();
		soundList = null;
	}

	public static void PlaySound(string soundName)
	{
		if(Instance == null)
			return;

		if(!Instance.soundList.ContainsKey(soundName))
		{
			GameObject newObj = new GameObject();
			newObj.name = soundName;
			newObj.transform.SetParent( Instance.transform );
			AudioSource audio = newObj.AddComponent<AudioSource>();
			audio.clip = Resources.Load("Audio/" + soundName) as AudioClip;
			Instance.soundList.Add(soundName, audio);

			audio.volume = Instance.volume;
			audio.Play();
		}
		else
		{
			AudioSource source = Instance.soundList[soundName];
			source.Play();
		}
	}



}
