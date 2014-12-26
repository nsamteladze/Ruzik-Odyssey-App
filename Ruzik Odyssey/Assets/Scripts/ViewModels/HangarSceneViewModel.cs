using RuzikOdyssey.Common;
using RuzikOdyssey.Domain.Inventory;
using System.Collections.Generic;
using System;
using System.Linq;

namespace RuzikOdyssey.ViewModels
{
	public sealed class HangarSceneViewModel : ViewModel
	{
		private ICollection<InventoryItem> purchasedItems = new List<InventoryItem>();
		private ICollection<InventoryItem> equippedItems = new List<InventoryItem>();

		public ICollection<InventoryItem> PurchasedItems 
		{ 
			get { return purchasedItems; } 
			private set
			{
				purchasedItems = value;
				OnPurchasedItemsUpdated();
			}
		}

		public ICollection<InventoryItem> EquippedItems
		{
			get { return equippedItems; }
			private set
			{
				equippedItems = value;
				OnEquippedItemsUpdated();
			}
		}
		
		public event EventHandler PurchasedItemsUpdated;
		public event EventHandler EquippedItemsUpdated;
		
		protected override void Start ()
		{
			base.Start();
			
			PurchasedItems = GlobalModel.Inventory.PurchasedItems;
			EquippedItems = GlobalModel.Inventory.EquippedItems;

			Log.Debug("HangarSceneViewModel - START. Purchased items: {0}, Equipped items: {1}",
			          PurchasedItems.Count, EquippedItems.Count);
		}
		
		private void OnPurchasedItemsUpdated()
		{
			if (PurchasedItemsUpdated != null) PurchasedItemsUpdated(this, EventArgs.Empty);
		}

		private void OnEquippedItemsUpdated()
		{
			if (EquippedItemsUpdated != null) EquippedItemsUpdated(this, EventArgs.Empty);
		}
		
//		public void View_ItemEquipped(object sender, InventoryItemStateChangedEventArgs e)
//		{
//			Log.Debug("BEFORE - Purchased items: {0}, Equipped items: {1}", 
//			          GlobalModel.Inventory.PurchasedItems.Count, GlobalModel.Inventory.EquippedItems.Count);
//			
//			var item = GlobalModel.Inventory.PurchasedItems.FirstOrDefault(x => x.Id == e.ItemId);
//			
//			if (item == null)
//			{
//				Log.Error("Failed to equip item with ID {0}. Failed to find item in purchased items.",
//				          e.ItemId);
//				return;
//			}
//			
//			var itemRemoved = GlobalModel.Inventory.PurchasedItems.Remove(item);
//			
//			if (!itemRemoved)
//			{
//				Log.Error("Failed to equip item with ID {0}. Failed to remove item from purchased items.",
//				          e.ItemId);
//				return;
//			}
//			
//			OnPurchasedItemsUpdated();
//			
//			GlobalModel.Inventory.EquippedItems.Add(item);
//
//			OnEquippedItemsUpdated();
//			
//			Log.Debug("AFTER - Purchased items: {0}, Equipped items: {1}", 
//			          GlobalModel.Inventory.PurchasedItems.Count, GlobalModel.Inventory.EquippedItems.Count);
//		}
//
//		public void View_ItemUnequipped(object sender, InventoryItemStateChangedEventArgs e)
//		{
//			Log.Debug("BEFORE - Purchased items: {0}, Equipped items: {1}", 
//						GlobalModel.Inventory.PurchasedItems.Count, GlobalModel.Inventory.EquippedItems.Count);
//			
//			var item = GlobalModel.Inventory.EquippedItems.FirstOrDefault(x => x.Id == e.ItemId);
//			
//			if (item == null)
//			{
//				Log.Error("Failed to unequip item with ID {0}. Failed to find item in equipped items.",
//				          e.ItemId);
//				return;
//			}
//			
//			var itemRemoved = GlobalModel.Inventory.EquippedItems.Remove(item);
//			
//			if (!itemRemoved)
//			{
//				Log.Error("Failed to unequip item with ID {0}. Failed to remove item from equipped items.",
//				          e.ItemId);
//				return;
//			}
//			
//			OnEquippedItemsUpdated();
//			
//			GlobalModel.Inventory.PurchasedItems.Add(item);
//			
//			OnPurchasedItemsUpdated();
//			
//			Log.Debug("AFTER - Purchased items: {0}, Equipped items: {1}", 
//			          GlobalModel.Inventory.PurchasedItems.Count, GlobalModel.Inventory.EquippedItems.Count);
//		}

		public void View_ItemUpgraded(object sender, InventoryItemUpgradedEventArgs e)
		{
			var item = GetInventoryItemsCollection(e.ItemState).FirstOrDefault(x => x.Id == e.ItemId);
			
			if (item == null)
			{
				Log.Error("Failed to upgrade item with ID {0}. Failed to find item in {1} items.",
				          e.ItemId, Enum.GetName(typeof(InventoryItemState), e.ItemState));
				return;
			}

			item.Level += 1;
		}

		public void View_ItemStateChanged(object sender, InventoryItemStateChangedEventArgs e)
		{
			Log.Debug("BEFORE - Purchased items: {0}, Equipped items: {1}", 
			          GlobalModel.Inventory.PurchasedItems.Count, GlobalModel.Inventory.EquippedItems.Count);

			var item = GetInventoryItemsCollection(e.OldState).FirstOrDefault(x => x.Id == e.ItemId);

			if (item == null)
			{
				Log.Error("Failed to change state for item with ID {0}. Failed to find item in {1} items.",
				          e.ItemId, Enum.GetName(typeof(InventoryItemState), e.OldState));
				return;
			}

			var itemRemoved = GetInventoryItemsCollection(e.OldState).Remove(item);

			if (!itemRemoved)
			{
				Log.Error("Failed to change state for item with ID {0}. Failed to remove item from {1} items.",
				          e.ItemId, Enum.GetName(typeof(InventoryItemState), e.OldState));
				return;
			}

			TriggerCollectionUpdatedEvent(e.OldState);

			GetInventoryItemsCollection(e.NewState).Add(item);

			TransferFunds(item, e.OldState, e.NewState);
			TriggerCollectionUpdatedEvent(e.NewState);

			Log.Debug("AFTER - Purchased items: {0}, Equipped items: {1}", 
			          GlobalModel.Inventory.PurchasedItems.Count, GlobalModel.Inventory.EquippedItems.Count);
		}

		private ICollection<InventoryItem> GetInventoryItemsCollection(InventoryItemState state)
		{
			switch (state)
			{
				case InventoryItemState.Available: return GlobalModel.Inventory.AvailableItems;
				case InventoryItemState.Purchased: return GlobalModel.Inventory.PurchasedItems;
				case InventoryItemState.Equipped: return GlobalModel.Inventory.EquippedItems;
				default: return new List<InventoryItem>();
			}
		}

		private void TriggerCollectionUpdatedEvent(InventoryItemState state)
		{
			switch (state)
			{
				case InventoryItemState.Purchased: 
					OnPurchasedItemsUpdated();
					break;
				case InventoryItemState.Equipped: 
					OnEquippedItemsUpdated();
					break;
			}
		}

		private void TransferFunds(InventoryItem item, InventoryItemState oldState, InventoryItemState newState)
		{
			if (newState == InventoryItemState.Available && oldState != InventoryItemState.Available)
			{
				GlobalModel.Gold.Value += item.Price.Gold / 2;
			}
		}
	}
}

