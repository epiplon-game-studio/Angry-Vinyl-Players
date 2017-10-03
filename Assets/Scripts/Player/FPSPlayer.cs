using EZEffects;
using System;
using UniRx;
using UnityEngine;
using vnc.Utilities.Time;

[Serializable]
public class FPSPlayer : UnityStandardAssets.Characters.FirstPerson.FirstPersonController
{
	public static FPSPlayer Instance { get; private set; }

	[Header("Gun")]
	public Transform Head;
	public Transform Gun;
	public Transform GunCamera;
	public Animator gunAnimator;
	public string gunShootParameter;
	public Transform GunBarrel;
	public float GunLeanRotation;
	public Rigidbody VinylPrefab;

	[Header("Settings")]
	public float ShootingCooldown;
	public float Life;
	public int VinylBullets;
	public Vector3 GunBobbingAdjustment;
	[HideInInspector] public bool Alive { get { return Life > 0; } }

	private CooldownEvent cooldown;
	private CharacterController cController;
	private float gunTargetAngle = 0;
	private float gunActualAngle = 0;
	private Vector3 gunCameraStartPos;
	private float bobAngle = 0;

	public Mimic currentEnemy = null;
	public LayerMask enemyLayer;
	private RaycastHit enemyHit;
	private int waterlayer;

	public override void OnStart()
	{
		Instance = this;
		cController = GetComponent<CharacterController>();
		cooldown = new CooldownEvent(ShootingCooldown);
		cooldown.Start();
		gunCameraStartPos = GunCamera.localPosition;

		waterlayer = 4;
	}

	public override void OnUpdate()
	{
		cooldown.Tick();
		if (Input.GetButton("Fire1") && cooldown.IsReady)
		{
			gunAnimator.SetTrigger(gunShootParameter);
			cooldown.Reset();

			var bullet = Instantiate(VinylPrefab, GunBarrel.transform.position, GunBarrel.transform.rotation);
			bullet.AddForce(GunBarrel.forward * 15, ForceMode.Impulse);
			Destroy(bullet.gameObject, 5);

			m_MouseLook.KickCamera();
		}

		HeadBob();
		SmoothRotation();
		LeanCamera();

		if (Physics.Raycast(GunBarrel.position, GunBarrel.forward, out enemyHit, 100, enemyLayer))
		{
			currentEnemy = enemyHit.transform.GetComponent<Mimic>();
			PlayerHUD.Singleton.SetEnemyHealth(currentEnemy);
		}
		else
		{
			PlayerHUD.Singleton.SetEnemyHealth(null);
		}
	}

	private void HeadBob()
	{
		if (cController.velocity.magnitude > 0)
		{
			float bobOscillate = Mathf.Sin(bobAngle * Mathf.Deg2Rad) /2 ;
			bobAngle += (Time.deltaTime * 200);
			if (bobAngle >= 360) bobAngle = 0;

			//GunCamera.localPosition = new Vector3(bobOscillate - 1.66f, Math.Abs(bobOscillate) + 1.6f, -2.3f);
			GunCamera.localPosition = new Vector3(bobOscillate + GunBobbingAdjustment.x, Math.Abs(bobOscillate) + GunBobbingAdjustment.y, GunBobbingAdjustment.z);
		}
		else
		{
			GunCamera.localPosition = gunCameraStartPos;
		}

	}

	void SmoothRotation()
	{
		var mouseSpeed = Input.GetAxis("Mouse X");
		gunTargetAngle = Mathf.Clamp(mouseSpeed, -1, 1) * GunLeanRotation;
		gunActualAngle = Mathf.LerpAngle(gunActualAngle, gunTargetAngle, 0.1f);
		Gun.localEulerAngles = new Vector3(0, gunActualAngle, 0);
	}
	
	void LeanCamera()
	{
		float zRot = Input.GetAxis("Horizontal");
		Quaternion localRot = Head.localRotation;
		Head.localRotation = Quaternion.Euler(localRot.x, localRot.y, -zRot * 4);
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.layer == waterlayer)
		{
			m_IsSwiming = true;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == waterlayer)
		{
			m_IsSwiming = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == waterlayer)
		{
			m_IsSwiming = false;
		}
	}
}
