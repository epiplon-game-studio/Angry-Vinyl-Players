using UnityEngine;

public class DestructibleConcrete : Destructible
{
	public ParticleSystem particles;

	MeshRenderer m_Renderer;
	MeshCollider m_Collider;
	bool isDestroyed = false;

	private void Start()
	{
		m_Renderer = GetComponent<MeshRenderer>();
		m_Collider = GetComponent<MeshCollider>();
	}

	public override void OnDestruct()
	{
		if (!isDestroyed)
		{
			isDestroyed = true;
			m_Renderer.enabled = false;
			m_Collider.enabled = false;
			Instantiate(particles, transform.position, particles.transform.rotation);
			Destroy(gameObject, 3f);
		}
	}
}
