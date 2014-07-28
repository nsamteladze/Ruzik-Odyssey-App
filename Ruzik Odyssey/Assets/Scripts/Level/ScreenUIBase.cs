using UnityEngine;
using System;

public abstract class ScreenUIBase : MonoBehaviour
{
	protected Vector2 scaleOffset = Vector2.one;
	protected float scale = 1.0f;
	
	private void Awake()
	{
		scaleOffset.x = Screen.width / Environment.DesignWidth;
		scaleOffset.y = Screen.height / Environment.DesignHeight;
		scale = Mathf.Max(scaleOffset.x, scaleOffset.y); 

		Initialize();
	}
	
	private void OnGUI()
	{
		InitializeUI();
	}

	protected abstract void InitializeUI();

	protected virtual void Initialize()
	{
	}
}

