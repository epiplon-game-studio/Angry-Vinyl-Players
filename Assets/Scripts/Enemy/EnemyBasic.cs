using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class EnemyBasic : MonoBehaviour 
{
	AudioSource Audio;
	bool Happy = false;
	UnityEngine.AI.NavMeshAgent agent;
	BoxCollider boxCollider;
	Rigidbody body;
	Transform Player;
	Walk _player;

	public Animator animator;
	public Transform GoldenVinyl;
	public AudioClip KilledSound;
	public AudioClip HitSound;

	public int HitPoints;
	
	void Start()
	{
		Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		boxCollider = GetComponent<BoxCollider>();
		_player = Player.GetComponent<Walk>();
		Audio = GetComponent<AudioSource>();
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		body = GetComponent<Rigidbody>();


		EventManager.StartListening(EventManager.Events.GameRestart, Kill);
		EventManager.StartListening(EventManager.Events.GamePause, () => { agent.Stop(); body.Sleep(); });
		EventManager.StartListening(EventManager.Events.GameResume, () => { agent.Resume(); body.WakeUp(); });
	}

	void Update()
	{
		if (Happy)
			return;

		if (_player != null)
		{
			if (!_player.Alive)
				agent.Stop();

			if (!Happy || _player.Alive)
				agent.destination = Player.transform.position;
		}
		else
		{
			Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
			_player = Player.GetComponent<Walk>();
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("Projectile") && !Happy )
		{
			var vinyl = collision.transform.GetComponent<Vinyl>();
			Debug.Assert(vinyl != null, "Vinyl is the only Projectile object");

			if (HitPoints > 0 && !vinyl.IsBroken)
			{
				Audio.PlayOneShot(HitSound);
				HitPoints--;
			}
			else
			{
				Happy = true;
				Audio.PlayOneShot(KilledSound);
				agent.destination = collision.transform.position;
				var vinylPosition = new Vector3(transform.position.x, 0.8f, transform.position.z);
				Instantiate(GoldenVinyl, vinylPosition, GoldenVinyl.rotation);
				Kill();

				EventManager.TriggerEvent(EventManager.Events.SpawnVinylPlayer);
			}
		}
	}


	void Kill()
	{
		Debug.Log("Killed");
		animator.SetBool("IsAlive", false);
		boxCollider.enabled = false;
		body.velocity = Vector3.zero;
		body.isKinematic = true;

		Destroy(gameObject, 5);

		if (Application.isEditor)
		{
			UnityEditor.Selection.activeGameObject = gameObject;
		}
	}
}
