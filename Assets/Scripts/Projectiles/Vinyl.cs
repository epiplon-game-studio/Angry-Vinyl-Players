using UnityEngine;
using System.Collections;

public class Vinyl : MonoBehaviour 
{
	float Lifespan = 5.0f;
	MeshRenderer renderer;

	public Material BrokenMaterial;

	void Start()
	{
		renderer = GetComponent<MeshRenderer>();
	}

	void Update()
	{
		Lifespan -= Time.deltaTime;
		if (Lifespan < 0)
			Destroy(gameObject);
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("Enemy"))
		{
			Destroy(gameObject);
		}
		renderer.material = BrokenMaterial;
	}
	
}
