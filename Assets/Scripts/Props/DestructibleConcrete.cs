using UnityEngine;

public class DestructibleConcrete : MonoBehaviour
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

	public void OnDestructibleDestroy()
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
