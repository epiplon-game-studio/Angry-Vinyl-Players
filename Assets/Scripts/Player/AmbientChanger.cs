using UnityEngine;

public class AmbientChanger : MonoBehaviour
{
	[Header("Ambient")]
	public Color normalAmbient;
	public Color waterAmbient;
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 4)
		{
			RenderSettings.ambientSkyColor = waterAmbient;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 4)
		{
			RenderSettings.ambientSkyColor = normalAmbient;
		}
	}
}
