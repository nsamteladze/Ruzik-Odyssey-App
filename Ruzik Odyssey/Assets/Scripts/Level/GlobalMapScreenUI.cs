using UnityEngine;
using System;

public class GlobalMapScreenUI : MonoBehaviour
{
	public GameObject chapterDescriptionPopup;

	public void LoadChapter1MapScreen()
	{
		Application.LoadLevel("chapter1_map_screen"); 
	}

	public void LoadMainScreen()
	{
		Application.LoadLevel("main_screen");
	}

	public void ShowLevelDescriptionPopup()
	{
		chapterDescriptionPopup.SetActive(true);
	}

	public void HideLevelDescriptionPopup()
	{
		chapterDescriptionPopup.SetActive(false);
	}
}

