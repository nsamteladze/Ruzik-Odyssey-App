using RuzikOdyssey.Domain.Store;
using System.Collections.Generic;
using RuzikOdyssey.Common;
using System;

namespace RuzikOdyssey.ViewModels
{
	public class StoreSceneViewModel : ViewModel
	{
		public static StoreItemCategory StoreCategory { get; set; }

		private IList<StoreItem> availableStoreItems = new List<StoreItem>();
		public IList<StoreItem> AvailableStoreItems 
		{ 
			get { return this.availableStoreItems; } 
			private set 
			{
				this.availableStoreItems = value;
				OnAvailableItemsUpdated();
			} 
		}

		public event EventHandler AvailableStoreItemsUpdated;
		
		private void OnAvailableItemsUpdated()
		{
			if (AvailableStoreItemsUpdated != null) AvailableStoreItemsUpdated(this, EventArgs.Empty);
		}

		protected override void Start ()
		{
			base.Start();

			this.AvailableStoreItems = RetrieveAvailableStoreItems(StoreCategory);
		}

		private IList<StoreItem> RetrieveAvailableStoreItems(StoreItemCategory category)
		{
			switch (category)
			{
				case StoreItemCategory.Aircrafts: return GlobalModel.Store.Aircrafts;
				case StoreItemCategory.Gold: return GlobalModel.Store.Gold;
				case StoreItemCategory.Corn: return GlobalModel.Store.Corn;
				default:
					Log.Error("Failed to retrieve store items for category {0}.", EnumUtility.GetName(category));
					return new List<StoreItem>();
			}
		}
	}
}
