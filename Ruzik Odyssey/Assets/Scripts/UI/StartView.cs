using RuzikOdyssey.Common;
using UnityEngine;

namespace RuzikOdyssey.UI
{
	public sealed class StartView : ExtendedMonoBehaviour
	{
		private const double BackgroundDetailsScreenRatioThreshold = 1.45;
		private const int VerticalScreenAdjustment = -30;

		public UIWidget screenContainer;
		public GameObject backgroundDetails;



		private void Awake()
		{
			if ((double) Screen.width / Screen.height > BackgroundDetailsScreenRatioThreshold) 
			{
				backgroundDetails.SetActive(false);

				screenContainer.topAnchor.absolute = VerticalScreenAdjustment;
				screenContainer.bottomAnchor.absolute = VerticalScreenAdjustment;

				screenContainer.ResetAndUpdateAnchors();
			}
		}

		public void GoToDashboardScreen()
		{
			Application.LoadLevel("main_screen");
		}
	}
}
