using UnityEngine;
using System.Collections;

public class Pickable : MonoBehaviour {

	public int Quantity;

	void Start()
	{
		EventManager.StartListening(EventManager.Events.GameRestart, () => { Destroy(gameObject); });
		Destroy(gameObject, 10.0f);
	}

	void Update () {
		transform.Rotate(0.6f, 0, 0);
	}
}
