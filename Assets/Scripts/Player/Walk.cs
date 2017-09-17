using UnityEngine;
using System.Linq;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;
using System;

[System.Obsolete]
public class Walk : MonoBehaviour {

	FirstPersonController FirstPerson;
	Camera Eyes;
	bool GameRunning = true;
	Transform DirectionSphere;
	public bool Alive 
	{ 
		get { return Life > 0; }
	}
	public Transform vinyl;

	public AudioSource PickupAudioSource;
	public AudioSource HurtAudioSource;
	AudioSource Audio;
	public AudioClip DeadClip;
	public Animator gunAnimator;

	public float Life;
	public int VinylBullets;

	float hurtDelay = 1.0f;

	// Use this for initialization
	void Start () {
		FirstPerson = GetComponent<FirstPersonController>();
		Eyes = GetComponentInChildren<Camera>() as Camera;
		var list = GetComponentsInChildren<Transform>();
		DirectionSphere = list.Single(t => t.tag.Equals("Debug"));
		
		Audio = GetComponent<AudioSource>();

		EventManager.StartListening(EventManager.Events.PlayerDied, Died);
		EventManager.StartListening(EventManager.Events.GamePause, () => { FirstPerson.m_CanMove = GameRunning = false;  });
		EventManager.StartListening(EventManager.Events.GameResume, () => { FirstPerson.m_CanMove = GameRunning = true; });
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameRunning)
			return;

		if (Life <= 0)
		{
			FirstPerson.m_CanMove = false;
			return;
		}

		// Update Vinyl counter
		if (CrossPlatformInputManager.GetButtonDown("Fire1") && VinylBullets > 0)
		{
			Fire();
		}
		else if (CrossPlatformInputManager.GetButtonDown("Fire2") && VinylBullets >= 3)
		{
			Burst();
		}

		if (hurtDelay > -1)
			hurtDelay -= Time.deltaTime;
	}

	void Fire()
	{
		var instance = (Transform)Instantiate(vinyl, DirectionSphere.position, transform.rotation);
		var vinylRigidbody = instance.GetComponent<Rigidbody>();
		var force = Eyes.transform.forward * 500;
		vinylRigidbody.AddForce(force);
		VinylBullets--;
		gunAnimator.SetTrigger("Shoot");
	}

	void Burst()
	{
		for (int i = 0; i < 3; i++)
		{
			Fire();
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.transform.CompareTag("Pickable"))
		{
			var pickable = collider.GetComponent<Pickable>();
			VinylBullets += pickable.Quantity;

			var source = Instantiate(PickupAudioSource);
			source.Play();
			Destroy(source, 2);
			Destroy(collider.gameObject);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (Alive)
		{
			if (collision.transform.CompareTag("Enemy"))
			{
				collision.rigidbody.AddForce(Vector3.back * 70);
				Life--;
				if (!Alive)
				{
					EventManager.TriggerEvent(EventManager.Events.PlayerDied);
				}
				else
				{
					if (hurtDelay <= 0)
					{
						var source = Instantiate(HurtAudioSource);
						source.Play();
						Destroy(source, 2);
						hurtDelay = 1.0f;
					}
					EventManager.TriggerEvent(EventManager.Events.PlayerHurt);
				}
			}

		}
	}

	private void Died()
	{
		Audio.PlayOneShot(DeadClip);
		Eyes.transform.Translate(Vector3.down * 0.5f);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
}
