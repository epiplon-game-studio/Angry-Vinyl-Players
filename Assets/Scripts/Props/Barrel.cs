using System.Collections;
using UnityEngine;

public class Barrel : MonoBehaviour
{
	public GameObject ExplosionFX;
	public LayerMask destructibleLayer;
	public float radius;

	public AudioSource explosionSFX;

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Projectile"))
		{
			Debug.Log(name + " exploded by a projectile!");
			Explode();
		}
	}

	public void Explode()
	{
		var soundEffect = Instantiate(explosionSFX, transform.position, transform.rotation);
		Destroy(soundEffect, 6f);

		var explosion = Instantiate(ExplosionFX, transform.position - (Vector3.up * 2), ExplosionFX.transform.rotation);
		Destroy(explosion.gameObject, 2f);
		
		StartCoroutine(ExplodingNearby());
	}

	IEnumerator ExplodingNearby()
	{
		yield return new WaitForSecondsRealtime(0.2f);
		foreach (var hit in Physics.SphereCastAll(transform.position, radius, Vector3.up, 50, destructibleLayer))
		{
			if (hit.collider.gameObject == gameObject)
			{
				Debug.Log(name + " hit itself!");
				yield return null;
			}

			var barrel = hit.transform.GetComponent<Barrel>();
			if (barrel != null)
			{
				Debug.Log(string.Format("{0} exploded {1}", name, barrel.gameObject.name));
				barrel.Explode();
			}

			var destructible = hit.transform.GetComponent<DestructibleConcrete>();
			if (destructible != null)
			{
				destructible.OnDestructibleDestroy();
			}
		}
		Destroy(gameObject);
		yield return null;
	}
}
