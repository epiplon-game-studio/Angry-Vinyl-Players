using EZEffects;
using System;
using UniRx;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
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

	[Header("Water System")]
	public float m_SwimSpeed;
	public float aboveWaterTolerance;
	private int waterlayer;
	private bool m_SwimInput;
	private bool m_DiveInput;
	private float _waterSurfacePosY;

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

	public override void OnSwim()
	{
		RotateView();
		m_SwimInput = CrossPlatformInputManager.GetButton("Jump");
		m_DiveInput = CrossPlatformInputManager.GetButton("Dive");

		float speed;
		GetInput(out speed);
		// always move along the camera forward as it is the direction that it being aimed at
		Vector3 desiredMove = m_Camera.transform.forward * m_Input.y + transform.right * m_Input.x;
		m_MoveDir = desiredMove * speed;
		m_MoveDir.y = 0;
		
		if (m_SwimInput && m_Camera.transform.position.y < _waterSurfacePosY + aboveWaterTolerance)
		{
			m_MoveDir.y = m_SwimSpeed;
		}
		else if (m_DiveInput)
		{
			m_MoveDir.y = -m_SwimSpeed;
		}

		m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.deltaTime;
		m_CollisionFlags = cController.Move(m_MoveDir * Time.deltaTime);
		ProgressStepCycle(speed);
		UpdateCameraPosition(speed);

		m_MouseLook.UpdateCursorLock();

		OnUpdate();
	}

	private void HeadBob()
	{
		if (cController.velocity.magnitude > 0 && !m_IsSwiming)
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

	public bool IsUnderwater()
	{
		return m_Camera.gameObject.transform.position.y < (_waterSurfacePosY);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == waterlayer)
		{
			_waterSurfacePosY = other.transform.position.y;
			m_IsSwiming = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == waterlayer)
		{
			_waterSurfacePosY = other.transform.position.y;
			float fpsPosY = this.transform.position.y;
			if (fpsPosY > _waterSurfacePosY)
			{
				// ok we really left the water
				m_IsSwiming = false;
			}
		}
	}
}
