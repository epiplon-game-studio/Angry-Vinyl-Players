using UnityEngine;
using System.Collections;

public class Pickable : MonoBehaviour {

	public int Quantity;

	// Update is called once per frame
	void Update () {
		transform.Rotate(0.6f, 0, 0);
	}
}
