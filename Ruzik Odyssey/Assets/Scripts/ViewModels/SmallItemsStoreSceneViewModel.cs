using RuzikOdyssey.Common;
using RuzikOdyssey.Domain.Inventory;
using System.Collections.Generic;
using System;

namespace RuzikOdyssey.ViewModels
{
	public sealed class SmallItemsStoreSceneViewModel : ViewModel
	{
		private ICollection<InventoryItem> availableItems = new List<InventoryItem>();
		public ICollection<InventoryItem> AvailableItems 
		{ 
			get { return availableItems; } 
			private set
			{
				availableItems = value;
				OnAvailableItemsUpdated();
			}
		}

		public event EventHandler AvailableItemsUpdated;

		protected override void Start ()
		{
			base.Start();

			AvailableItems = GlobalModel.Inventory.AvailableItems;
		}

		private void OnAvailableItemsUpdated()
		{
			if (AvailableItemsUpdated != null) AvailableItemsUpdated(this, EventArgs.Empty);
		}
	}
}
