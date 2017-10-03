using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using vnc.Core;
using vnc.Tools;

public class GameManager : Manager<GameManager>
{
	bool GameStarted = false;
	[HideInInspector] public bool IsRunning = true;
	[HideInInspector] public int score = 0;

	[Header("References")]
	public Camera WorldCamera;

	public FPSPlayer Player;
	FPSPlayer _playerInstance;
	public EnemyBasic Enemy;
	public Text Score;
	[Space]
	public Transform PlayerSpawnPoint;

	public RespawnArea[] EnemySpawn;

	void Start ()
	{
		IsRunning = true;
		EventManager.StartListening(GameEvents.SpawnEnemy, SpawnVinylPlayer);
		EventManager.StartListening(GameEvents.GamePause, PauseGame);
		StartOrResumeGame();
	}

	void Update()
	{
		if (CrossPlatformInputManager.GetButtonDown("Cancel"))
		{
			if (IsRunning)
			{
				EventManager.TriggerEvent(GameEvents.GamePause);
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
		}
	}

	public void StartOrResumeGame()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		//WorldCamera.gameObject.SetActive(false);

		if (!GameStarted)
		{
			GameStarted = true;

			SpawnVinylPlayer();
            EventManager.TriggerEvent(GameEvents.GameStarted);
        }
        else
		{
			EventManager.TriggerEvent(GameEvents.GameResume);
			IsRunning = true;
		}
	}

	private void PauseGame()
	{
		IsRunning = false;
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
		EventManager.TriggerEvent(GameEvents.GameRestart);

		Destroy(_playerInstance.gameObject);
		SpawnVinylPlayer();

		score = 0;
		Score.text = string.Format("Score: {0}", score);
	}

	void SpawnVinylPlayer()
	{
		var playerCollider = Player.GetComponent<CapsuleCollider>();
		RespawnArea area = EnemySpawn.ElementAt(Random.Range(0, EnemySpawn.Count()));

		Instantiate(Enemy, area.RandomPosition(), Enemy.transform.rotation);
		Instantiate(Enemy, area.RandomPosition(), Enemy.transform.rotation);

		score++;
		//Score.text = string.Format("Score: {0}", score);
	}
}

public static class GameEvents
{
	public static string GameStarted = "START";
	public static string GameRestart = "RESTART";
	public static string GamePause = "PAUSE";
	public static string GameResume = "RESUME";
	public static string SpawnEnemy = "SPAWNENEMY";
	public static string PlayerHurt = "PLAYERHURT";
	public static string PlayerDied = "PLAYERDIED";
}