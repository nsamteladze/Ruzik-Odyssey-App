using UnityEngine;
using System.Collections;

public class GameOverMenu : MonoBehaviour
{
	private void OnGUI()
	{
		GUI.skin.button = Environment.DefaultButtonStyle;

		if (GUI.Button(new Rect(724 * Environment.ScaleOffset.x, 327 * Environment.ScaleOffset.y, 
		                        600 * Environment.Scale, 200 * Environment.Scale), 
		               "Restart"))
		{
			Environment.StartMission();
			Application.LoadLevel("default_level");  
		}

		if (GUI.Button(new Rect(724 * Environment.ScaleOffset.x, 627 * Environment.ScaleOffset.y, 
		                        600 * Environment.Scale, 200 * Environment.Scale), 
		               "Main menu"))
		{
			Application.LoadLevel("start_screen");  
		}
	}
}

