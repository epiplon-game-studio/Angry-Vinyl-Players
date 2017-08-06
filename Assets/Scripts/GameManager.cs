using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour
{
	bool GameStarted = false;
	public bool IsRunning = true;
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
		EventManager.StartListening(EventManager.Events.GamePause, PauseGame);
	}

	void Update()
	{
		if (CrossPlatformInputManager.GetButtonDown("Cancel"))
		{
			if (IsRunning)
			{
				EventManager.TriggerEvent(EventManager.Events.GamePause);
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
		}
	}

	public void StartOrResumeGame()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		if (!GameStarted)
		{
			GameStarted = true;
			Destroy(Camera.main.gameObject);

			_playerInstance = PlayerCanvas.Player = Instantiate(Player);
			PlayerCanvas.gameObject.SetActive(true);
			SpawnVinylPlayer();
            EventManager.TriggerEvent(EventManager.Events.GameStarted);
        }
        else
		{
			EventManager.TriggerEvent(EventManager.Events.GameResume);
			IsRunning = true;
		}
	}

	private void PauseGame()
	{
		IsRunning = false;
		MenuCanvas.gameObject.SetActive(true);
	}

	public void ExitGame()
	{
		try
		{
			Application.Quit();
		}
		catch (System.Exception)
		{
		}
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
