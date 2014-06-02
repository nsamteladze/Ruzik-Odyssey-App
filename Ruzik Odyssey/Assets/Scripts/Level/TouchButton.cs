using UnityEngine;
using System.Collections;
using RuzikOdyssey;

public abstract class TouchButton : MonoBehaviour
{
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

	}

	protected abstract void OnButtonTouch();

	public virtual void Touch()
	{
		OnButtonTouch();
	}
	
	public bool HitTest(Vector2 position)
	{
		return buttonTexture.HitTest(position);
	}
}

