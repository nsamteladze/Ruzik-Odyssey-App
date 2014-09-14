using UnityEngine;
using RuzikOdyssey.Common;
using RuzikOdyssey;
using RuzikOdyssey.Level;

namespace RuzikOdyssey.UI
{
	public sealed class CornStoreView : ExtendedMonoBehaviour
	{
		public UILabel goldAmountLabel;
		public UILabel cornAmountLabel;
		
		private void Awake()
		{
			InitializeUi();
		}
		
		private void InitializeUi()
		{
			goldAmountLabel.text = GlobalModel.Gold.Value.ToString();
			cornAmountLabel.text = GlobalModel.Corn.Value.ToString();
		}
		
		private void SubscribeToEvent()
		{
			EventBroker.Subscribe<PropertyChangedEventArgs<int>>(Events.Global.GoldPropertyChanged, Gold_PropertyChanged);
			EventBroker.Subscribe<PropertyChangedEventArgs<int>>(Events.Global.CornPropertyChanged, Corn_PropertyChanged);
		}
		
		private void Gold_PropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			goldAmountLabel.text = e.PropertyValue.ToString();
		}
		
		private void Corn_PropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			cornAmountLabel.text = e.PropertyValue.ToString();
		}
		
		public void ExitStore()
		{
			Application.LoadLevel("main_screen");
		}
	}
}

