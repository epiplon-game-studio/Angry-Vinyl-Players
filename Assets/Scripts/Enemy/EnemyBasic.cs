using UniRx;
using UnityEngine;
using vnc.Tools;

public class EnemyBasic : MonoBehaviour 
{
	AudioSource Audio;
	UnityEngine.AI.NavMeshAgent agent;
	BoxCollider boxCollider;
	Rigidbody body;

	public Animator animator;
	public Transform GoldenVinyl;
	public AudioClip KilledSound;
	public AudioClip HitSound;

	public int TotalHitPoints;
	public int HitPoints;
	
	void Start()
	{
		boxCollider = GetComponent<BoxCollider>();
		Audio = GetComponent<AudioSource>();
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		agent.isStopped = false;
		body = GetComponent<Rigidbody>();
		HitPoints = TotalHitPoints;
		//HitPoints.Subscribe(h => PlayerHUD.Singleton.SetEnemyHealth(this));

		//EventManager.StartListening(GameEvents.GameRestart, Kill);
		//EventManager.StartListening(GameEvents.GamePause, () => { agent.isStopped = true; body.Sleep(); });
		//EventManager.StartListening(GameEvents.GameResume, () => { agent.isStopped = false; body.WakeUp(); });
	}

	void Update()
	{
		if (FPSPlayer.Instance != null)
		{
			agent.destination = FPSPlayer.Instance.transform.position;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("Projectile"))
		{
			var vinyl = collision.transform.GetComponent<Vinyl>();

			if (HitPoints > 0 && !vinyl.IsBroken)
			{
				Audio.PlayOneShot(HitSound);
				HitPoints--;
				if (HitPoints <= 0)
				{
					Audio.PlayOneShot(KilledSound);
					agent.destination = collision.transform.position;
					var vinylPosition = new Vector3(transform.position.x, 0.8f, transform.position.z);
					Instantiate(GoldenVinyl, vinylPosition, GoldenVinyl.rotation);
					Kill();

					EventManager.TriggerEvent(GameEvents.SpawnEnemy);
				}
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

		Destroy(gameObject);

		if (Application.isEditor)
		{
			UnityEditor.Selection.activeGameObject = gameObject;
		}
	}
}
