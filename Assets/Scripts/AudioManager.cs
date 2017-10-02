using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using vnc.Core;

public class AudioManager : Manager<AudioManager>
{
	public bool PlayMusic = true;
	public AudioSource musicSource;

	[Space]
	public AudioMixer audioMixer;
	public string musicGroup;
	public string gunEffectGroup;
	public string enemyGroup;

	void Start()
	{
		SetMusic(PlayMusic);

		//if (!PlayMusic)
		//	audioMixer.SetFloat(musicGroup, 0.0f);
	}


	public void SetMusic(bool play)
	{
		if (play)
			musicSource.Play();
		else
			musicSource.Stop();
	}
}
