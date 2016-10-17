using UnityEngine;
using System.Collections;

public class ButtonClick : MonoBehaviour 
{
	public bool StartGame = false;

	void OnClick()
	{
		StartGame = true;
		Debug.Log("Start game");
	}

}
