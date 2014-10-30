using UnityEngine;
using System;

public class Chapter1MapView : MonoBehaviour
{	
	public GameObject levelDescriptionPopup;

	public void ShowLevelDescriptionPopup()
	{
		levelDescriptionPopup.SetActive(true);
	}

	public void HideLevelDescriptionPopup()
	{
		levelDescriptionPopup.SetActive(false);
	}

	public void ReturnToMainScreen()
	{
		Application.LoadLevel("main_screen");
	}

	public void ChooseLevel()
	{
		Application.LoadLevel("main_screen");
	}
}

