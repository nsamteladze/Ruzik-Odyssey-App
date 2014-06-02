using UnityEngine;
using System.Collections;

public class PauseButton : TouchButton 
{
	private GameObject ui;
	
	private new void Start()
	{
		base.Start();

		ui = GameObject.Find("UI");
		if (ui == null)
			throw new UnityException("Failed to find game object named 'ui' in the hierarchy");
	}
	
	protected override void OnButtonTouch()
	{
		if (!Environment.IsPaused) 
		{
			ui.AddComponent<PauseMenu>();
			Environment.Pause();
		}
	}

}
