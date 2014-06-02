using UnityEngine;
using System.Collections.Generic;

public static class Environment
{ 
	public const float DesignWidth = 2048f; 
	public const float DesignHeight = 1154f;
	public static Vector2 ScaleOffset;
	public static float Scale;
	public static GUISkin DefaultSkin;
	public static GUIStyle DefaultButtonStyle;

	public static bool IsPaused { get; private set; }
	public static bool IsGameOver { get; private set; }

	static Environment()
	{
		ScaleOffset.x = Screen.width / DesignWidth;
		ScaleOffset.y = Screen.height / DesignHeight;
		Scale = Mathf.Max(ScaleOffset.x, ScaleOffset.y); 
		DefaultSkin = Resources.Load("default_menu_skin") as GUISkin;
		DefaultButtonStyle = new GUIStyle(DefaultSkin.GetStyle("button"));
		DefaultButtonStyle.fontSize = (int)(Scale * DefaultButtonStyle.fontSize);

		IsPaused = false;
		IsGameOver = false;
	}

	public static void Pause()
	{
		Time.timeScale = 0;
		IsPaused = true;
	}

	public static void Resume()
	{
		Time.timeScale = 1;
		IsPaused = false;
	}

	public static void StartMission()
	{
		IsGameOver = false;
	}

	public static void GameOver()
	{
		IsGameOver = true;
	}
}

