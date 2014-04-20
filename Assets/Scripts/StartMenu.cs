using UnityEngine;

/// <summary>
/// Title screen script
/// </summary>
public class StartMenu : MonoBehaviour
{
	private GUISkin skin;
	
	void Start()
	{
		// Load a skin for the buttons
		skin = Resources.Load("MainMenuSkin") as GUISkin;
	}
	
	void OnGUI()
	{
		int buttonWidth = Screen.width / 6;
		int buttonHeight = Screen.height / 8;
		
		// Set the skin to use
		GUI.skin = skin;
		
		// Draw a button to start the game
		if (GUI.Button(
			// Center in X, 2/3 of the height in Y
			new Rect(Screen.width / 1.65f - buttonWidth / 2, Screen.height / 1.6f - buttonHeight / 2, 
		         buttonWidth, buttonHeight),
			"START"
			))
		{
			// On Click, load the first level.
			Application.LoadLevel("DefaultLevel"); // "Stage1" is the scene name
		}
	}
}