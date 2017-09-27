using UnityEngine;

public class AmbientChanger : MonoBehaviour
{
	[Header("Ambient")]
	public Color normalAmbient;
	public Color waterAmbient;
	
	private void OnTriggerEnter(Collider other)
	{
		RenderSettings.ambientSkyColor = waterAmbient;
	}

	private void OnTriggerExit(Collider other)
	{
		RenderSettings.ambientSkyColor = normalAmbient;

	}
}
