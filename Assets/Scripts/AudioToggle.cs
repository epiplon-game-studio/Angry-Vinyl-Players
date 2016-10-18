using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AudioToggle : MonoBehaviour
{
	public AudioSource source;
	public Text message;
	
	public void Toggle()
	{
		if (source.isPlaying)
		{
			source.Pause();
			message.text = "Music Resume";
		}
		else
		{
			source.UnPause();
			message.text = "Music Mute";
		}
	}
}
