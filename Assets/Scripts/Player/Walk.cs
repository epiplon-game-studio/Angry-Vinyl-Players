using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Walk : MonoBehaviour {

	CharacterController Controller;
	FirstPersonController FirstPerson;
	Camera Eyes;
	RaycastHit hit;
	CursorLockMode lockMode;

	Transform DirectionSphere;
	public bool Alive 
	{ 
		get { return Life > 0; }
	}
	public Transform vinyl;
	//public RectTransform Hand;

	public AudioSource PickupAudioSource;
	public AudioSource HurtAudioSource;
	AudioSource Audio;
	public AudioClip DeadClip;

	public float Life;
	public int VinylBullets;

	float hurtColorFade = 0;

	// Use this for initialization
	void Start () {
		Controller = GetComponent<CharacterController>();
		FirstPerson = GetComponent<FirstPersonController>();
		Eyes = GetComponentInChildren<Camera>() as Camera;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		var list = GetComponentsInChildren<Transform>();
		DirectionSphere = list.Single(t => t.tag.Equals("Debug"));
		
		Audio = GetComponent<AudioSource>();

		EventManager.StartListening(EventManager.Events.PlayerDied, Died);
	}
	
	// Update is called once per frame
	void Update () {
		if (Life <= 0)
		{
			FirstPerson.m_CanMove = false;
			return;
		}

		// Update Vinyl counter
		if (Input.GetMouseButtonDown(0) && VinylBullets > 0)
		{
			var instance = (Transform)Instantiate(vinyl, DirectionSphere.position, transform.rotation);
			var vinylRigidbody = instance.GetComponent<Rigidbody>();
			var force = Eyes.transform.forward * 500;
			vinylRigidbody.AddForce(force);
			VinylBullets--;
		}
		hurtColorFade -= Time.deltaTime;
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
					var source = Instantiate(HurtAudioSource);
					source.Play();
					Destroy(source, 2);
					EventManager.TriggerEvent(EventManager.Events.PlayerHurt);
				}
			}

		}
	}

	private void Died()
	{
		Audio.PlayOneShot(DeadClip);
		Controller.Move(Vector3.down * 20);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
}
