using UnityEngine;
using System.Collections;

public class Pickable : MonoBehaviour {

	public int Quantity;

	void Start()
	{
		EventManager.StartListening(EventManager.Events.GameRestart, () => { Destroy(gameObject); });
	}

	void Update () {
		transform.Rotate(0.6f, 0, 0);
	}
}
