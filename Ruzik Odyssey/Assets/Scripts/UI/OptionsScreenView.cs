using RuzikOdyssey;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.UI
{
	public sealed class OptionsScreenView : ExtendedMonoBehaviour
	{
		public UILabel gameContentVersionLabel;

		private void Awake()
		{
			GlobalModel.Connect();
		}

		private void Start()
		{
			gameContentVersionLabel.text = GlobalModel.Content.Value.Version;
		}
	}
}