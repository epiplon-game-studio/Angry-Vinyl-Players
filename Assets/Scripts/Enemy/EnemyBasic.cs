using UnityEngine;
using System.Collections;
using System.Linq;

public class EnemyBasic : MonoBehaviour 
{
	AudioSource Audio;
	bool Happy = false;
	NavMeshAgent agent;
	Transform Player;
	Walk _player;

	public Transform GoldenVinyl;
	public AudioClip KilledSound;
	public AudioClip HitSound;

	public int HitPoints;

	GameController controller;

	void Awake()
	{
		Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		_player = Player.GetComponent<Walk>();
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	void Start()
	{
		HitPoints = 5;
		Audio = GetComponent<AudioSource>();
		agent = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		if (!Happy || _player.Life > 0)
			agent.destination = Player.transform.position; 

	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("Projectile") && !Happy)
		{
			if (HitPoints > 0)
			{
				Audio.PlayOneShot(HitSound);
				HitPoints--;
			}
			else
			{
				Happy = true;
				controller.TotalEnemies--;
				var childRenderers = GetComponentsInChildren<MeshRenderer>();
				foreach (var item in childRenderers)
					item.material.color = Color.blue;
				
				Audio.PlayOneShot(KilledSound);
				agent.destination = collision.transform.position;
				Instantiate(GoldenVinyl, transform.position, GoldenVinyl.rotation);
				Destroy(gameObject);

				controller.Respawn();				
			}
		}
	}
}
