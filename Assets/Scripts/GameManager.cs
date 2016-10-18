using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public bool IsRunning = false;
	[HideInInspector]
	public int score = 0;
	
	public Walk Player;
	Walk _playerInstance;
	public GameObject Enemy;
	public Text Score;
	public Canvas MenuCanvas;
	public PlayerHUD PlayerCanvas;

	public RespawnArea[] SpawnPoints;

	void Start ()
	{
		IsRunning = true;
		EventManager.StartListening(EventManager.Events.SpawnVinylPlayer, SpawnVinylPlayer);
	}

	public void StartGame()
	{
		IsRunning = true;
		Destroy(Camera.main.gameObject);
		MenuCanvas.gameObject.SetActive(false);

		_playerInstance = PlayerCanvas.Player = Instantiate(Player);
		PlayerCanvas.gameObject.SetActive(true);
		SpawnVinylPlayer();
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void RestartGame()
	{
		EventManager.TriggerEvent(EventManager.Events.GameRestart);

		Destroy(_playerInstance.gameObject);
		_playerInstance = PlayerCanvas.Player = Instantiate(Player);
		SpawnVinylPlayer();

		score = 0;
		Score.text = string.Format("Score: {0}", score);
	}

	void SpawnVinylPlayer()
	{
		var playerCollider = Player.GetComponent<CapsuleCollider>();
		RespawnArea area;
		do
		{
			// The enemies cannot spawn on the same area as the player
			var i = Random.Range(0, SpawnPoints.Length);
			area = SpawnPoints[i];

			//} while (area.Intersect(playerCollider.bounds));
		} while (!area.IsAvailable);

		Instantiate(Enemy, area.RandomPosition(), Enemy.transform.rotation);
		Instantiate(Enemy, area.RandomPosition(), Enemy.transform.rotation);

		score++;
		Score.text = string.Format("Score: {0}", score);
	}
}
