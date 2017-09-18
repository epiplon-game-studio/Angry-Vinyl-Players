using UnityEngine;
using System.Collections;
using vnc.Tools;

public class Pickable : MonoBehaviour {

	public int Quantity;

	void Start()
	{
		EventManager.StartListening(GameEvents.GameRestart, () => { Destroy(gameObject); });
		Destroy(gameObject, 10.0f);
	}

	void Update () {
		transform.Rotate(0.6f, 0, 0);
	}
}
