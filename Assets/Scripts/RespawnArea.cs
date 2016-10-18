using UnityEngine;

public class RespawnArea : MonoBehaviour
{
	BoxCollider box;
	public Vector3 Center { get { return box.center; } }
	bool _available = true;
	public bool IsAvailable { get { return _available; } } 
	
	void Start()
	{
		box = GetComponent<BoxCollider>();
	}
	
	public Vector3 RandomPosition()
	{
		var x_radius = box.size.x / 2;
		var z_radius = box.size.z / 2;

		var x_point = Random.Range(transform.position.x - x_radius, transform.position.x + x_radius);
		var z_point = Random.Range(transform.position.z - z_radius, transform.position.z + z_radius);

		return new Vector3(x_point, box.center.y, z_point);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.transform.CompareTag("Player"))
		{
			_available = false;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.transform.CompareTag("Player"))
		{
			_available = true;
		}
	}
}
