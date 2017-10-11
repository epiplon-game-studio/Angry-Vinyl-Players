using UnityEngine;

public class AmbientChanger : MonoBehaviour
{
	[Header("Ambient")]
	public Color normalAmbient;
	public Color waterAmbient;
	[Space]
	public FPSPlayer player;

	private void Update()
	{
		if (player.IsUnderwater())
		{
			RenderSettings.ambientSkyColor = waterAmbient;
			RenderSettings.fog = true;
		}
		else
		{
			RenderSettings.ambientSkyColor = normalAmbient;
			RenderSettings.fog = false;
		}
	}
}
