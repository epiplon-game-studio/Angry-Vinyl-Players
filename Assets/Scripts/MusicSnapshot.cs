using UnityEngine;
using UnityEngine.Audio;
using vnc.Tools;

public class MusicSnapshot : MonoBehaviour {

    public AudioMixerSnapshot GameSS;
    public AudioMixerSnapshot MenuSS;

	// Use this for initialization
	void Start () {
        EventManager.StartListening(GameEvents.GameStarted, GameResumed);
        EventManager.StartListening(GameEvents.GamePause, GamePaused);
        EventManager.StartListening(GameEvents.GameResume, GameResumed);
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
