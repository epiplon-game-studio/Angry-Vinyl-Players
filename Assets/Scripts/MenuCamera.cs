using UnityEngine;
using System.Collections;

public class MenuCamera : MonoBehaviour
{
	Camera menuCamera;

	void Start()
	{
		menuCamera = GetComponent<Camera>();
	}
	
	void Update ()
	{
		transform.Rotate(0, 0.6f, 0);
	}
}
