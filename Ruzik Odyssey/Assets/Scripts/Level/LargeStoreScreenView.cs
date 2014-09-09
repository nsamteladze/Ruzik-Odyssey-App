using UnityEngine;

namespace RuzikOdyssey.UI
{
	public sealed class LargeStoreScreenView : MonoBehaviour
	{
		public void ExitStore()
		{
			Application.LoadLevel("main_screen");
		}
	}
}
