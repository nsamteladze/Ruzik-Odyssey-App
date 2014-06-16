using UnityEngine;
using System.Collections;
using RuzikOdyssey;
using RuzikOdyssey.Level;
using System;

public class TouchButton : MonoBehaviour, ITouchControl
{
	public string buttonName;

	private GUITexture buttonTexture;

	protected void Start()
	{
		buttonTexture = this.gameObject.guiTexture;
		if (buttonTexture == null) throw new UnityException("Failed to find touch button texture");

		Rect initialPixelInset = buttonTexture.pixelInset;
		// Scale pixel inset recrangle
		buttonTexture.pixelInset = new Rect(initialPixelInset.x * Environment.ScaleOffset.x,
		                                    initialPixelInset.y * Environment.ScaleOffset.y,
		                                    initialPixelInset.width * Environment.ScaleOffset.x,
		                                    initialPixelInset.height * Environment.ScaleOffset.y);

		RegisterEvents();
	}

	protected virtual void RegisterEvents()
	{
		EventBroker.Publish(String.Format("{0}_Touch", buttonName), ref Touch);
	}

	public event EventHandler<EventArgs> Touch;

	protected virtual void OnTouch()
	{
		Touch(this, EventArgs.Empty);
	}

	public void TriggerTouch()
	{
		OnTouch();
	}
	
	public bool HitTest(Vector2 position)
	{
		return buttonTexture.HitTest(position);
	}
}

