using UnityEngine;
using UnityEngine.Audio;

public class MusicSnapshot : MonoBehaviour {

    public AudioMixerSnapshot GameSS;
    public AudioMixerSnapshot MenuSS;

	// Use this for initialization
	void Start () {
        EventManager.StartListening(EventManager.Events.GameStarted, GameResumed);
        EventManager.StartListening(EventManager.Events.GamePause, GamePaused);
        EventManager.StartListening(EventManager.Events.GameResume, GameResumed);
    }

    private void GamePaused()
    {
        MenuSS.TransitionTo(0);
    }

    private void GameResumed()
    {
        GameSS.TransitionTo(0);
    }
}
