using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class Walk : MonoBehaviour {

	CharacterController Controller;
	Camera Eyes;
	Vector3 MoveDirection = Vector3.zero;
	RaycastHit hit;
	CursorLockMode lockMode;

	Transform DirectionSphere;
	bool Alive 
	{ 
		get { return Life > 0; }
	}
	public Transform vinyl;
	public RectTransform BulletIndicator;
	public RectTransform Hand;
	public RectTransform LifeIndicator;
	public Image HurtDisplay;
	Text textComponent;
	Text lifeComponent;

	AudioSource Audio;
	public AudioClip PickupClip;
	public AudioClip HurtClip;
	public AudioClip DeadClip;

	public float Life;
	public float Speed;
	public float MouseSpeed;
	public int VinylBullets = 3;

	float _hurtDelay = 0;

	// Use this for initialization
	void Start () {
		Controller = GetComponent<CharacterController>() as CharacterController;
		Eyes = GetComponentInChildren<Camera>() as Camera;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		var list = GetComponentsInChildren<Transform>();
		DirectionSphere = list.Single(t => t.tag.Equals("Debug"));

		textComponent = BulletIndicator.GetComponent<Text>();
		lifeComponent = LifeIndicator.GetComponent<Text>();

		Audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKey("escape"))
			Exit();				

		lifeComponent.text = string.Format("x{0}", Life);
		if (Life <= 0)
			return;

		HurtDisplay.color = new Color(1, 0, 0, _hurtDelay / 5);
		// Update Vinyl counter
		textComponent.text = string.Format("x{0}", VinylBullets);

		if (Input.GetMouseButtonDown(0) && VinylBullets > 0)
		{
			var instance = (Transform)Instantiate(vinyl, DirectionSphere.position, transform.rotation);
			var rigidbody = instance.GetComponent<Rigidbody>();
			var force = Eyes.transform.forward * 200 * Speed;
			rigidbody.AddForce(force);
			VinylBullets--;
		}
		_hurtDelay -= Time.deltaTime;
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.transform.CompareTag("Pickable"))
		{
			var pickable = collider.GetComponent<Pickable>();
			VinylBullets += pickable.Quantity;

			Audio.PlayOneShot(PickupClip);
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
					Die();
				}
				else
				{
					if(_hurtDelay < 0)
					{
						Audio.PlayOneShot(HurtClip);
						_hurtDelay = 1.5f;
					}
				}
			}

		}
	}

	private void Die()
	{
		Audio.PlayOneShot(DeadClip);
		Destroy(Hand.gameObject);
		Controller.Move(Vector3.down * 20);
	}

	void Exit()
	{
		Cursor.lockState = CursorLockMode.None;
		Application.Quit();
	}
}
