using EZEffects;
using System;
using UniRx;
using UnityEngine;
using vnc.Tools.FirstPerson;
using vnc.Utilities.Time;

public class FPSPlayer : FirstPersonController
{
	public static FPSPlayer Instance { get; private set; }

	[Header("Gun")]
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
	[HideInInspector] public bool Alive { get { return Life > 0; } }

	private CooldownEvent cooldown;
	private CharacterController cController;
	private float gunTargetAngle = 0;
	private float gunActualAngle = 0;
	private Vector3 gunCameraStartPos;
	private float bobAngle = 0;


	public EnemyBasic currentEnemy = null;
	private RaycastHit enemyHit;
	private int enemyLayer;

	public override void OnStart()
	{
		Instance = this;
		cController = GetComponent<CharacterController>();
		cooldown = new CooldownEvent(ShootingCooldown);
		cooldown.Start();
		gunCameraStartPos = GunCamera.localPosition;

		enemyLayer = LayerMask.GetMask("Enemy");

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
		}

		HeadBob();
		SmoothRotation();

		if (Physics.Raycast(GunBarrel.position, GunBarrel.forward, out enemyHit, 100, enemyLayer))
		{
			currentEnemy = enemyHit.transform.GetComponent<EnemyBasic>();
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

			GunCamera.localPosition = new Vector3(bobOscillate - 1.66f, Math.Abs(bobOscillate) + 1.6f, -2.3f);
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

	private void OnGUI()
	{
		if (Debug.isDebugBuild)
		{
			GUI.Box(new Rect(0, 0, 200, 50), "Gun Angle: " + gunTargetAngle);
		}
	}
}
