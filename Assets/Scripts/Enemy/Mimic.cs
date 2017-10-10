using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using vnc.Utilities.Time;

public class Mimic : MonoBehaviour
{
	public static FPSPlayer Player;

	public LayerMask hitscan;
	public NavMeshAgent agent;
	public Animator m_anim;

	[Space, Header("Audio")]
	public AudioClip[] audioGrunts;
	public AudioClip warcry;
	AudioSource audioSource;
	CooldownEvent gruntCooldown;

	[Space]
	public BoolReactiveProperty isAlerted = new BoolReactiveProperty();
	System.IDisposable warcryObservable;
	public float speed = 3;

	[Space, Header("Health")]
	public int TotalHitPoints;
	[HideInInspector] public int HitPoints;

	readonly float[] SIGHT_ANGLES = new[] { -45f, 0f, 45f };
	RaycastHit hit;
	Vector3 sightDirection;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		if (Player == null)
			Player = FindObjectOfType<FPSPlayer>();

		HitPoints = TotalHitPoints;
		isAlerted.Subscribe(alert => OnAlerted(alert));

		warcryObservable = isAlerted.Subscribe(alert =>
		{
			if (alert)
			{
				audioSource.PlayOneShot(warcry);
				warcryObservable.Dispose();
			}
		});		

		gruntCooldown = new CooldownEvent(3f);
		gruntCooldown.Start();
	}

	private void Update()
	{
		Sight(50f);
		if (isAlerted.Value)
		{
			//var direction = (Player.transform.position - transform.position);
			//direction.y = 0;
			//var rotation = Quaternion.LookRotation(direction);
			//transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
			//agent.Move(direction.normalized * speed * Time.deltaTime);
			agent.destination = Player.transform.position;
		}

		gruntCooldown.Tick();
		if (gruntCooldown.IsReady && !isAlerted.Value)
			GruntSound();
	}

	void Sight(float rayDistance)
	{
		foreach (var angle in SIGHT_ANGLES)
		{
			sightDirection = Quaternion.Euler(0, angle, 0) * transform.forward;
			if (Physics.Raycast(transform.position, sightDirection, out hit, rayDistance, hitscan))
			{
				isAlerted.SetValueAndForceNotify(true);
				m_anim.SetTrigger("Attack");
				break;
			}
		}

	}

	void GruntSound()
	{
		if (audioGrunts.Length > 0)
		{
			var index = Random.Range(0, audioGrunts.Length);
			audioSource.PlayOneShot(audioGrunts[index]);
		}
		gruntCooldown.Reset();
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("Projectile"))
		{
			isAlerted.SetValueAndForceNotify(true);
			var vinyl = collision.transform.GetComponent<Vinyl>();
			if (HitPoints > 0 && !vinyl.IsBroken)
			{
				HitPoints--;
				if (HitPoints <= 0)
				{
					Destroy(gameObject);
				}
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isAlerted.SetValueAndForceNotify(true);
			if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
			{
				agent.Move(-5 * transform.forward);
			}
		}
	}

	void OnAlerted(bool alert)
	{
		m_anim.SetBool("Alerted", alert);
		if (alert)
			Debug.Log("Alerted!");
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -45f, 0) * transform.forward * 50);
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, 0) * transform.forward * 50);
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 45f, 0) * transform.forward * 50);
	}
}
