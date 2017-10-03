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

	[Space, Header("Music Settings")]
	public string startMood;
	public MusicMood[] musicMoods;
	public float HighFrequency;
	public float LowFrequency;

	Dictionary<string, AudioClip> musicsDic = new Dictionary<string, AudioClip>();
	float currentFrequency;


	void Start()
	{
		foreach (var mood in musicMoods)
			musicsDic.Add(mood.Name, mood.Clip);

		SetMood(startMood);
		SetMusic(PlayMusic);

		//currentFrequency = LowFrequency;
		//audioMixer.SetFloat("MusicFrequency", currentFrequency);
	}

	public void SetMood(string key)
	{
		AudioClip clip;
		if (musicsDic.TryGetValue(key, out clip))
		{
			StartCoroutine(ChangeMood(clip));
		}
	}

	public void SetMusic(bool play)
	{
		if (play)
		{
			musicSource.Play();
			StartCoroutine(SetHighFrequency());
		}
		else
		{
			musicSource.Stop();
			currentFrequency = LowFrequency;
			audioMixer.SetFloat("MusicFrequency", currentFrequency);
		}
	}

	IEnumerator SetHighFrequency()
	{
		var count = (HighFrequency - LowFrequency) / 50;
		for (int i = 0; i < count; i++)
		{
			float frequency;
			audioMixer.GetFloat("MusicFrequency", out frequency);
			frequency = Mathf.Lerp(frequency, HighFrequency, 50f);
			audioMixer.SetFloat("MusicFrequency", frequency);
			yield return null;
		}

	}

	IEnumerator ChangeMood(AudioClip clip)
	{
		for (int i = 10 - 1; i > 0; i--)
		{
			musicSource.volume = (i / 10f);
			yield return new WaitForEndOfFrame();
		}
		musicSource.Stop();
		musicSource.clip = clip;
		yield return new WaitForEndOfFrame();
		for (int j = 0; j < 10; j++)
		{
			musicSource.volume = (j / 10f);
			yield return new WaitForEndOfFrame();
		}
		musicSource.Play();
	}
}

[System.Serializable]
public class MusicMood
{
	public string Name;
	public AudioClip Clip;
}
