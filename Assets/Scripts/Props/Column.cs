using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : Destructible
{
	public Animator animator;

	public override void OnDestruct()
	{
		animator.SetTrigger("Break");
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == destructibleLayer)
		{
			Debug.Log("Collision object: " + collision.gameObject.name);
			var destructible = collision.gameObject.GetComponent<Destructible>();
			if (destructible != null)
			{
				destructible.OnDestruct();
			}
		}
	}
}
