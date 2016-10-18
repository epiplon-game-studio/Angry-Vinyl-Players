using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayerHUD : MonoBehaviour
{
	[HideInInspector]
	public Walk Player;

	public Text BulletCounter;
	public Text LifeCounter;
	public Image Hand;
	Image _handCopy;
	public Image GameOverPanel;
	public Image HurtDisplay;

	float hurtColorFade;

	void Start()
	{
		EventManager.StartListening(EventManager.Events.GameRestart, GameRestart);
		EventManager.StartListening(EventManager.Events.PlayerHurt, PlayerWasHit);
		EventManager.StartListening(EventManager.Events.PlayerDied, PlayerDied);

		_handCopy = Instantiate(Hand);
	}

	private void GameRestart()
	{
		Hand = Instantiate(_handCopy);
		Hand.transform.SetParent(transform, false);
		GameOverPanel.gameObject.SetActive(false);
	}

	void Update ()
	{
		BulletCounter.text = string.Format("x{0}", Player.VinylBullets);
		LifeCounter.text = string.Format("x{0}", Player.Life);
		HurtDisplay.color = new Color(1, 0, 0, hurtColorFade);
		if (hurtColorFade > 0)
			hurtColorFade -= Time.deltaTime;
	}

	void PlayerWasHit()
	{
		hurtColorFade = 0.5f;
	}

	private void PlayerDied()
	{
		Destroy(Hand.gameObject);
		GameOverPanel.gameObject.SetActive(true);
	}
}
