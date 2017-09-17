using UnityEngine;
using vnc.Tools.FirstPerson;
using vnc.Utilities.Time;

public class FPSPlayer : FirstPersonController
{
	[Header("Gun")]
	public Transform Gun;
	public Animator gunAnimator;
	public string gunShootParameter;
	public Transform GunBarrel;
	public float GunLeanRotation;
	public Rigidbody VinylPrefab;

	[Header("Settings")]
	public float ShootingCooldown;
	private CooldownEvent cooldown;

	private float gunTargetAngle = 0;
	private float gunActualAngle = 0;

	public override void OnStart()
	{
		cooldown = new CooldownEvent(ShootingCooldown);
		cooldown.Start();
	}

	public override void OnUpdate()
	{
		cooldown.Tick();
		if (Input.GetButtonDown("Fire1") && cooldown.IsReady)
		{
			gunAnimator.SetTrigger(gunShootParameter);
			cooldown.Reset();

			var bullet = Instantiate(VinylPrefab, GunBarrel.transform.position, GunBarrel.transform.rotation);
			bullet.AddForce(GunBarrel.forward * 15, ForceMode.Impulse);
			Destroy(bullet.gameObject, 5);
		}

		SmoothRotation();
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
