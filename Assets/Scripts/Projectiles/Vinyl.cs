using UnityEngine;
using System.Collections;

public class Vinyl : MonoBehaviour 
{
	float Lifespan = 5.0f;
	[HideInInspector] public bool IsBroken = false; 
	public MeshRenderer meshRenderer;
	public ParticleSystem pieces;

	public Material BrokenMaterial;

	void Start()
	{
		Destroy(gameObject, Lifespan);
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("Enemy"))
		{
			var particles = Instantiate(pieces, collision.transform.position, pieces.transform.rotation);
			Destroy(particles.gameObject, 1.1f);
			Destroy(gameObject);
		}
		else
		{
			meshRenderer.material = BrokenMaterial;
			IsBroken = true;
		}
	}
	
}
