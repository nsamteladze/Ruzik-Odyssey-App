using UnityEngine;
using System.Collections;

public abstract class TouchButtonBase : MonoBehaviour
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

	private void Update()
	{
		if (Input.touchCount == 0) return;

		foreach (Touch touch in Input.touches)
		{
			if (buttonTexture.HitTest(touch.position)) 
			{
				if (touch.phase == TouchPhase.Began) OnButtonTouch();
			}
		}
	}

	protected abstract void OnButtonTouch();
}

