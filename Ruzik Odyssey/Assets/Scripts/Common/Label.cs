using UnityEngine;
using System.Collections;

namespace RuzikOdyssey.Common.UI
{
	public class Label : MonoBehaviour
	{
		private void Awake()
		{
			if (gameObject.guiText == null) 
				throw new UnityException("Label script can be attached only to a game object with GUIText component.");
		}

		public string Text
		{
			get { return guiText.text; }
			set 
			{
				Log.Debug("Ammo is {0}", value);

				guiText.text = value; 
			}
		}
	}
}