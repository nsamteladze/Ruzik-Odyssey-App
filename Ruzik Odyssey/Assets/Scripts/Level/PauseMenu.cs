using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{	
	private void OnGUI()
	{
		GUI.skin.button = Environment.DefaultButtonStyle;

		if (GUI.Button(new Rect(724 * Environment.ScaleOffset.x, 202 * Environment.ScaleOffset.y, 
		                        600 * Environment.Scale, 200 * Environment.Scale), 
		               "Resume") )
		{
			Environment.Resume();
			Destroy(gameObject.GetComponent<PauseMenu>());
		}

		if (GUI.Button(new Rect(724 * Environment.ScaleOffset.x, 452 * Environment.ScaleOffset.y, 
		                        600 * Environment.Scale, 200 * Environment.Scale), 
		               "Restart") )
		{
			Environment.Resume();
			Application.LoadLevel("default_level");  
		}
		
		if (GUI.Button(new Rect(724 * Environment.ScaleOffset.x, 702 * Environment.ScaleOffset.y, 
		                        600 * Environment.Scale, 200 * Environment.Scale), 
		               "Main menu") )
		{
			Environment.Resume();
			Application.LoadLevel("start_screen");  
		}
	}
}

