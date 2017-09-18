using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using vnc.Core;
using vnc.Tools;

public class PlayerHUD : Manager<PlayerHUD>
{
	[HideInInspector]
	public FPSPlayer Player;

	public Slider EnemyHealth;

	public Text BulletCounter;
	public Text LifeCounter;
	public Image GameOverPanel;
	public Image HurtDisplay;

	float hurtColorFade;

	void Start()
	{
		EnemyHealth.maxValue = 3;
		EnemyHealth.value = 0;

		//EventManager.StartListening(GameEvents.GameRestart, GameRestart);
		//EventManager.StartListening(GameEvents.PlayerHurt, PlayerWasHit);
		//EventManager.StartListening(GameEvents.PlayerDied, PlayerDied);
	}

	private void GameRestart()
	{
		GameOverPanel.gameObject.SetActive(false);
	}

	void Update ()
	{
		//BulletCounter.text = string.Format("x{0}", Player.VinylBullets);
		//LifeCounter.text = string.Format("x{0}", Player.Life);
		//HurtDisplay.color = new Color(1, 0, 0, hurtColorFade);
		//if (hurtColorFade > 0)
		//	hurtColorFade -= Time.deltaTime;
	}

	void PlayerWasHit()
	{
		hurtColorFade = 0.5f;
	}

	private void PlayerDied()
	{
		GameOverPanel.gameObject.SetActive(true);
	}

	public void SetEnemyHealth(EnemyBasic enemy)
	{
		if (enemy == null)
		{
			EnemyHealth.maxValue = 3;
			EnemyHealth.value = 0;
			return;
		}

		EnemyHealth.maxValue = enemy.TotalHitPoints;
		EnemyHealth.value = enemy.HitPoints;
	}
}
