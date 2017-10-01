using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Crate : MonoBehaviour {

	public Rigidbody Epicenter;
	public float Force = 30f;
	public float Radius = 10;
	public float Upwards = 20f;
	Rigidbody[] bodies;

	void Start () {
		bodies = GetComponentsInChildren<Rigidbody>();
	}
	
	public void Crack()
	{
		bodies.ToList().ForEach(b =>
		{
			b.isKinematic = false;
			b.AddExplosionForce(Force, Epicenter.transform.position, Radius, Upwards);
		});
	}
}
