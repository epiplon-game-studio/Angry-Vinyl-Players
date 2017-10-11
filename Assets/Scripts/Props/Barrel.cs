using System.Collections;
using UnityEngine;

public class Barrel : MonoBehaviour
{
	public GameObject ExplosionFX;
	public LayerMask destructibleLayer;
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Projectile"))
		{
			Explode();
		}
	}

	public void Explode()
	{
		var explosion = Instantiate(ExplosionFX, transform.position - (Vector3.up * 2), ExplosionFX.transform.rotation);
		Destroy(explosion.gameObject, 2f);
		Destroy(gameObject);

		StartCoroutine(ExplodingNearby());
	}

	IEnumerator ExplodingNearby()
	{
		yield return new WaitForFixedUpdate();
		foreach (var hit in Physics.SphereCastAll(transform.position, 150, Vector3.up, 50, destructibleLayer))
		{
			if (hit.collider.gameObject == gameObject)
				yield return null;

			var barrel = hit.transform.GetComponent<Barrel>();
			if (barrel != null)
			{
				barrel.Explode();
			}
			yield return null;
		}
	}
}
