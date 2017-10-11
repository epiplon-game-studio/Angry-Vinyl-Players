using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {

	protected int destructibleLayer;

	void Awake()
	{
		destructibleLayer = LayerMask.GetMask("Destructible");
	}

	public virtual void OnDestruct() { }
}
