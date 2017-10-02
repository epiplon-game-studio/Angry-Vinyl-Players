using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class Mimic : MonoBehaviour
{
	public static FPSPlayer Player;

	public LayerMask playerLayer;
	public NavMeshAgent agent;
	public Animator m_anim;

	[Space]
	public BoolReactiveProperty isAlerted = new BoolReactiveProperty();
	public float speed = 3;

	[Space]
	public int TotalHitPoints;
	[HideInInspector] public int HitPoints;

	void Start()
	{
		if (Player == null)
			Player = FindObjectOfType<FPSPlayer>();

		HitPoints = TotalHitPoints;
		isAlerted.Subscribe(alert => OnAlerted(alert));
	}

	private void Update()
	{
		if (isAlerted.Value)
		{
			//var direction = (Player.transform.position - transform.position);
			//direction.y = 0;
			//var rotation = Quaternion.LookRotation(direction);
			//transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
			//agent.Move(direction.normalized * speed * Time.deltaTime);
			agent.destination = Player.transform.position;
		}
		
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, 50f, playerLayer))
		{
			isAlerted.SetValueAndForceNotify(true);
			m_anim.SetTrigger("Attack");
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("Player"))
		{
			isAlerted.SetValueAndForceNotify(true);
		}
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

	void OnAlerted(bool alert)
	{
		m_anim.SetBool("Alerted", alert);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(transform.position, transform.forward);
	}
}
