using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterOne : MonoBehaviour
{
	public List<Crate> crates;
	public List<Light> lights;
	public List<Mimic> alertEnemies;
	bool isStarted = false;

	public void StartEncounter()
	{
		if (!isStarted)
		{
			crates.ForEach(c => c.Crack());
			lights.ForEach(l => l.intensity = 4f);
			alertEnemies.ForEach(e => e.isAlerted.SetValueAndForceNotify(true));
			AudioManager.Singleton.SetMood("Danger");

			isStarted = true;
		}
	}
}
