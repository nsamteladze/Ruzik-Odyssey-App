using UnityEngine;
using System.Collections;
using RuzikOdyssey;
using RuzikOdyssey.Level;
using System;

public class TouchToggle : MonoBehaviour, ITouchControl
{
	public string toggleName = "TouchToggle";
	public bool isOn = false;
	
	private GUITexture toggleOnTexture;
	private GUITexture toggleOffTexture;

	protected void Start()
	{
		var onStateGameObject = this.gameObject.transform.FindChild("On");
		if (onStateGameObject == null) throw new UnityException("Failed to find a game object for toggle ON state");
		var offStateGameObject = this.gameObject.transform.FindChild("Off");
		if (offStateGameObject == null) throw new UnityException("Failed to find a game object for toggle OFF state");

		toggleOnTexture = onStateGameObject.guiTexture;
		if (toggleOnTexture == null) throw new UnityException("Failed to find toggle ON state texture");
		toggleOffTexture = offStateGameObject.guiTexture;
		if (toggleOffTexture == null) throw new UnityException("Failed to find toggle OFF state texture");
		
		var onStateTextureInitialPixelInset = toggleOnTexture.pixelInset;
		// Scale pixel inset recrangle
		toggleOnTexture.pixelInset = new Rect(onStateTextureInitialPixelInset.x * Environment.ScaleOffset.x,
		                                      onStateTextureInitialPixelInset.y * Environment.ScaleOffset.y,
		                                      onStateTextureInitialPixelInset.width * Environment.ScaleOffset.x,
		                                      onStateTextureInitialPixelInset.height * Environment.ScaleOffset.y);

		var offStateTextureInitialPixelInset = toggleOffTexture.pixelInset;
		// Scale pixel inset recrangle
		toggleOffTexture.pixelInset = new Rect(offStateTextureInitialPixelInset.x * Environment.ScaleOffset.x,
		                                       offStateTextureInitialPixelInset.y * Environment.ScaleOffset.y,
		                                       offStateTextureInitialPixelInset.width * Environment.ScaleOffset.x,
		                                       offStateTextureInitialPixelInset.height * Environment.ScaleOffset.y);

		Render();
		RegisterEvents();
	}
	
	protected virtual void RegisterEvents()
	{
		EventBroker.Publish(String.Format("{0}_StateChanged", toggleName), ref StateChanged);
	}
	
	public event EventHandler<ToggleStateChangedEventArgs> StateChanged;
	
	protected virtual void OnStateChanged()
	{
		isOn = !isOn;
		Render();
		StateChanged(this, new ToggleStateChangedEventArgs{ ToggleIsOn = isOn });
	}

	public void TriggerTouch()
	{
		OnStateChanged();
	}
	
	public bool HitTest(Vector2 position)
	{
		return isOn ? toggleOnTexture.HitTest(position) : toggleOffTexture.HitTest(position);
	}

	private void Render()
	{
		toggleOnTexture.enabled = isOn;
		toggleOffTexture.enabled = !isOn;
	}
}

