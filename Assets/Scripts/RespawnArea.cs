using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RespawnArea : MonoBehaviour
{
	BoxCollider box;
	public Vector3 Center { get { return box.center; } }
	bool _available = true;
	public bool IsAvailable { get { return _available; } } 
	
	public Vector3 RandomPosition()
	{
		box = GetComponent<BoxCollider>();

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
