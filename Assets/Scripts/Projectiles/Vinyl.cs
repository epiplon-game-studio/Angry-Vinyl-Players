using UnityEngine;
using System.Collections;

public class Vinyl : MonoBehaviour 
{
	float Lifespan = 5.0f;
	[HideInInspector] public bool IsBroken = false; 
	MeshRenderer meshRenderer;

	public Material BrokenMaterial;

	void Start()
	{
		meshRenderer = GetComponent<MeshRenderer>();
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
		meshRenderer.material = BrokenMaterial;
		IsBroken = true;
	}
	
}
