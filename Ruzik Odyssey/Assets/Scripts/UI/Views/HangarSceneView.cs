using UnityEngine;
using RuzikOdyssey.Common;
using RuzikOdyssey;
using RuzikOdyssey.Level;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using RuzikOdyssey.UI.Elements;
using RuzikOdyssey.Domain.Inventory;
using System;
using RuzikOdyssey.ViewModels;

namespace RuzikOdyssey.UI.Views
{
	public sealed class HangarSceneView : ExtendedMonoBehaviour
	{
		/// <summary>
		/// The default inventory items category that is show when the scene starts.
		/// </summary>
		private const InventoryItemCategory DefaultInventoryCategory = InventoryItemCategory.Weapons;

		public HangarSceneViewModel viewModel;

		public GameObject[] purchasedItemsHighlightedTabs;
		public GameObject[] purchasedItemsTabs;
		
		public GameObject purchasedItemsScrollView;
		public GameObject equippedItemsScrollView;

		public GameObject purchasedItemTemplate;
		public GameObject equippedItemTemplate;

		public GameObject itemDescriptionPopup;
		public InventoryItemDescription itemDescription;

		//private IList<StoreItemsCategory> itemsCategories;
		
//		private int selectedTabIndex = 0;

		private ICollection<InventoryItem> purchasedItems;
		private ICollection<InventoryItem> equippedItems;
		
		private ItemsStoreTab selectedTab;

//		private Guid selectedPurchasedItemId;
//		private Guid selectedEquippedItemId;

		private Guid selectedItemId;
		private bool equippedItemSelected;
		
		private void Awake()
		{
			
		}
		
		private void Start()
		{
			viewModel.PurchasedItemsUpdated += ViewModel_PurchasedItemsUpdated;
			viewModel.EquippedItemsUpdated += ViewModel_EquippedItemsUpdated;

			purchasedItems = viewModel.PurchasedItems;
			equippedItems = viewModel.EquippedItems;

			this.ItemStateChanged += viewModel.View_ItemStateChanged;
			this.ItemUpgraded += viewModel.View_ItemUpgraded;
			
			// Show default tab
			PopulatePurchasedItemsForTab(null);
			PopulateEquippedItems();

			itemDescriptionPopup.SetActive(false);
		}

//		public event EventHandler<InventoryItemStateChangedEventArgs> ItemEquipped;
//		public event EventHandler<InventoryItemStateChangedEventArgs> ItemUnequipped;
//		public event EventHandler<InventoryItemStateChangedEventArgs> EquippedItemSold;
//		public event EventHandler<InventoryItemStateChangedEventArgs> PurhcasedItemSold;

		public event EventHandler<InventoryItemStateChangedEventArgs> ItemStateChanged;

		public void OnItemStateChanged(Guid itemId, InventoryItemState oldState, InventoryItemState newState)
		{
			if (ItemStateChanged != null) 
				ItemStateChanged(this, new InventoryItemStateChangedEventArgs
         		{
					ItemId = itemId,
					OldState = oldState,
					NewState = newState
				});
		}
		
//		public void OnItemEquipped(Guid itemId)
//		{
//			if (ItemEquipped != null) ItemEquipped(this, new InventoryItemStateChangedEventArgs(itemId));
//		}
//
//		public void OnItemUnequipped(Guid itemId)
//		{
//			if (ItemUnequipped != null) ItemUnequipped(this, new InventoryItemStateChangedEventArgs(itemId));
//		}
//
//		public void OnEquippedItemSold(Guid itemId)
//		{
//			if (EquippedItemSold != null) EquippedItemSold(this, new InventoryItemStateChangedEventArgs(itemId));
//		}
//
//		public void OnPurchasedItemSold(Guid itemId)
//		{
//			if (PurhcasedItemSold != null) PurhcasedItemSold(this, new InventoryItemStateChangedEventArgs(itemId));
//		}
		
		public void Game_EquipItemButtonClicked()
		{
			SetItemDescriptionVisible(false);

			OnItemStateChanged(selectedItemId, 
			                   equippedItemSelected ? InventoryItemState.Equipped : InventoryItemState.Purchased,
			                   equippedItemSelected ? InventoryItemState.Purchased : InventoryItemState.Equipped);
		}

		private void ViewModel_PurchasedItemsUpdated(object sender, EventArgs e)
		{
			purchasedItems = viewModel.PurchasedItems;
			
			RefreshPurchasedItemsScrollView();
		}

		private void ViewModel_EquippedItemsUpdated(object sender, EventArgs e)
		{
			equippedItems = viewModel.EquippedItems;

			RefreshEquippedItemsScrollView();
		}
		
		private void ClearPurchasedItemsScrollView()
		{
			purchasedItemsScrollView.DestroyAllChildren();
		}

		private void ClearEquippedItemsScrollView()
		{
			equippedItemsScrollView.DestroyAllChildren();
		}
		
		private void RefreshPurchasedItemsScrollView()
		{
			ClearPurchasedItemsScrollView();
			PopulatePurchasedItemsForTab(selectedTab);
		}

		private void RefreshEquippedItemsScrollView()
		{
			ClearEquippedItemsScrollView();
			PopulateEquippedItems();
		}

		public void Game_PurchasedItemSelected(Guid itemId)
		{
			SelectItem(itemId, false);
			SetItemDescriptionVisible(true);
		}

		public void Game_EquippedItemSelected(Guid itemId)
		{
			SelectItem(itemId, true);
			SetItemDescriptionVisible(true);
		}

		private void SelectItem(Guid itemId, bool itemIsEquipped)
		{
			selectedItemId = itemId;
			equippedItemSelected = itemIsEquipped;

			var item = equippedItemSelected 
				? equippedItems.FirstOrDefault(x => x.Id == selectedItemId) 
				: purchasedItems.FirstOrDefault(x => x.Id == selectedItemId);

			if (item == null)
			{
				Log.Error("Failed to select item with ID {0}. Failed to find item in {1} items.",
				          selectedItemId, equippedItemSelected ? "equipped" : "purchased");
				return;
			}

			itemDescription.equipButtonTitle.text = equippedItemSelected ? "Unequip" : "Equip";

			itemDescription.itemName.text = item.Name;
			itemDescription.image.spriteName = item.SpriteName;

			itemDescription.rarity.text = Enum.GetName(typeof(InventoryItemCategory), item.Rarity);
			itemDescription.itemClass.text = String.Format("Class {0}", item.Class);

			itemDescription.price.gold.text = (item.Price.Gold / 2).ToString();
			itemDescription.level.text = item.Level.ToString();

			itemDescription.upgradePrice.gold.text = (1).ToString();

			switch (item.Category)
			{
				case InventoryItemCategory.Weapons:
					var weaponItem = (WeaponInventoryItem) item;

					itemDescription.feature1.featureName.text = "Fire Rate";
					itemDescription.feature1.value.text = weaponItem.FireRate.ToString();
					itemDescription.feature1.improvement.text = "+0";

					itemDescription.feature2.featureName.text = "Weight";
					itemDescription.feature2.value.text = weaponItem.Weight.ToString();
					itemDescription.feature2.improvement.text = "+0";
							
					itemDescription.feature3.gameObject.SetActive(false);
					itemDescription.feature4.gameObject.SetActive(false);
					break;
					
				case InventoryItemCategory.Engines:
					itemDescription.feature1.featureName.text = "Weight";
					itemDescription.feature1.value.text = item.Weight.ToString();
					itemDescription.feature1.improvement.text = "+0";

					itemDescription.feature2.featureName.text = "Power";
					itemDescription.feature2.value.text = String.Format("{0}{1}", item.Power >= 0 ? "+" : "-", item.Power);
					itemDescription.feature2.improvement.text = "+0";
						
					itemDescription.feature3.gameObject.SetActive(false);
					itemDescription.feature4.gameObject.SetActive(false);
					break;
					
				default:
					HideAllItemDescriptionFeatures();
					break;
			}
		}

		private void HideAllItemDescriptionFeatures()
		{
			itemDescription.feature1.gameObject.SetActive(false);
			itemDescription.feature2.gameObject.SetActive(false);
			itemDescription.feature3.gameObject.SetActive(false);
			itemDescription.feature4.gameObject.SetActive(false);
		}

		public void Game_CloseItemDescriptionButtonClicked()
		{
			SetItemDescriptionVisible(false);
		}

		public void Game_SellItemButtonClicked()
		{
			SetItemDescriptionVisible(false);

			OnItemStateChanged(selectedItemId, 
			                   equippedItemSelected ? InventoryItemState.Equipped : InventoryItemState.Purchased,
			                   InventoryItemState.Available);
		}

		public void Game_UpgradeItemButtonClicked()
		{
			OnItemUpgraded(selectedItemId, equippedItemSelected ? InventoryItemState.Equipped : InventoryItemState.Purchased);
		}

		private void OnItemUpgraded(Guid id, InventoryItemState state)
		{
			if (ItemUpgraded != null) ItemUpgraded(this, new InventoryItemUpgradedEventArgs(id, state));
		}

		public event EventHandler<InventoryItemUpgradedEventArgs> ItemUpgraded;

		private void SetItemDescriptionVisible(bool isVisible)
		{
			itemDescriptionPopup.SetActive(isVisible);
			purchasedItemsScrollView.SetActive(!isVisible);
			equippedItemsScrollView.SetActive(!isVisible);
		}

		private void PopulateEquippedItems()
		{
			GameObject previousEquippedItem = null;
			foreach (var item in equippedItems)
			{
				var equippedItem = NGUITools.AddChild(equippedItemsScrollView, equippedItemTemplate);
				var largeInventoryItem = equippedItem.GetComponentOrThrow<LargeInventoryItem>();
				
				// Capture index for the anonimous delegate closure
				var capturedId = item.Id;
				UIEventListener.Get(equippedItem.GetComponentOrThrow<UIButton>().gameObject).onClick += (obj) => 
				{ 
					Game_EquippedItemSelected(capturedId); 
				};

				largeInventoryItem.thumbnail.spriteName = item.ThumbnailName;
				largeInventoryItem.classLabel.text = String.Format("C{0}", item.Class);
				largeInventoryItem.levelLabel.text = String.Format("L{0}", item.Level);
				largeInventoryItem.rarityLabel.text = Enum.GetName(typeof(InventoryItemCategory), item.Rarity);

				var equippedItemSprite = equippedItem.GetComponentOrThrow<UISprite>();
				
				equippedItemSprite.leftAnchor.target = equippedItemsScrollView.transform;
				equippedItemSprite.leftAnchor.absolute = 0;
				
				equippedItemSprite.rightAnchor.target = equippedItemsScrollView.transform;
				equippedItemSprite.rightAnchor.absolute = 0;
				
				if (previousEquippedItem == null) 
				{
					// Most left item in the scroll view
				}
				else
				{
					equippedItemSprite.topAnchor.target = previousEquippedItem.transform;
					equippedItemSprite.topAnchor.absolute = 0;
					equippedItemSprite.topAnchor.relative = 0;
				}
				
				equippedItemSprite.updateAnchors = UIRect.AnchorUpdate.OnUpdate;
				
				equippedItemSprite.ResetAndUpdateAnchors();
				
				previousEquippedItem = equippedItem;
			}
			
			equippedItemsScrollView.GetComponentOrThrow<UIScrollView>().ResetPosition();
		}

		private void PopulatePurchasedItemsForTab(ItemsStoreTab tab)
		{
			var selectedCategory = tab != null ? tab.category : DefaultInventoryCategory;

			Log.Debug("Selected category {0}", Enum.GetName(typeof(InventoryItemCategory), selectedCategory));

			Log.Debug("Purchased items of category {0}: {1}",
			          Enum.GetName(typeof(InventoryItemCategory), selectedCategory), 
			          purchasedItems.Where(x => x.Category == selectedCategory).Count());

			GameObject previousStoreItem = null;
			foreach (var item in purchasedItems.Where(x => x.Category == selectedCategory))
			{
				var storeItem = NGUITools.AddChild(purchasedItemsScrollView, purchasedItemTemplate);
				storeItem.SingleChild().GetComponentOrThrow<UISprite>().spriteName = item.ThumbnailName;
				
				// Capture index for the anonimous delegate closure
				var capturedId = item.Id;
				UIEventListener.Get(storeItem.GetComponentOrThrow<UIButton>().gameObject).onClick += (obj) => 
				{ 
					Game_PurchasedItemSelected(capturedId); 
				};
				
				var storeItemSprite = storeItem.GetComponentOrThrow<UISprite>();
				
				storeItemSprite.topAnchor.target = purchasedItemsScrollView.transform;
				storeItemSprite.topAnchor.absolute = 0;
				
				storeItemSprite.bottomAnchor.target = purchasedItemsScrollView.transform;
				storeItemSprite.topAnchor.absolute = 0;
				
				if (previousStoreItem == null) 
				{
					// Most left item in the scroll view
				}
				else
				{
					storeItemSprite.leftAnchor.target = previousStoreItem.transform;
					storeItemSprite.leftAnchor.absolute = 0;
					storeItemSprite.leftAnchor.relative = 1f;
				}
				
				storeItemSprite.updateAnchors = UIRect.AnchorUpdate.OnUpdate;
				
				storeItemSprite.ResetAndUpdateAnchors();
				
				previousStoreItem = storeItem;
			}
			
			purchasedItemsScrollView.GetComponentOrThrow<UIScrollView>().ResetPosition();
		}
		
		public void Game_PurchasedItemsTabSelected(ItemsStoreTab tab)
		{
			ClearPurchasedItemsScrollView();
			
			var tabIndex = tab.tabIndex;
			var selectedTabIndex = selectedTab != null ? selectedTab.tabIndex : 0;
			
			// De-highlight the currently selected tab
			purchasedItemsHighlightedTabs[selectedTabIndex].SetActive(false);
			purchasedItemsTabs[selectedTabIndex].SetActive(true);
			
			// Anchor the tab next to the previously highlighted to the regular version of the tab
			if (selectedTabIndex < purchasedItemsTabs.Length - 1) 
			{
				var tabOnRight = purchasedItemsTabs[selectedTabIndex + 1];
				var tabOnRightSprite = tabOnRight.GetComponentOrThrow<UISprite>();
				
				var anchorSprite = purchasedItemsTabs[selectedTabIndex].GetComponentOrThrow<UISprite>();
				var anchorTransform = purchasedItemsTabs[selectedTabIndex].transform;
				
				tabOnRightSprite.leftAnchor.target = anchorTransform;
				tabOnRightSprite.leftAnchor.rect = anchorSprite;
			}
			
			// Highlight the new tab
			purchasedItemsHighlightedTabs[tabIndex].SetActive(true);
			purchasedItemsTabs[tabIndex].SetActive(false);
			
			// Anchor the next tab to the highlighted version of the tab
			if (tabIndex < purchasedItemsTabs.Length - 1) 
			{
				var tabOnRight = purchasedItemsTabs[tabIndex + 1];
				var tabOnRightSprite = tabOnRight.GetComponentOrThrow<UISprite>();
				
				var anchorSprite = purchasedItemsHighlightedTabs[tabIndex].GetComponentOrThrow<UISprite>();
				var anchorTransform = purchasedItemsHighlightedTabs[tabIndex].transform;
				
				tabOnRightSprite.leftAnchor.target = anchorTransform;
				tabOnRightSprite.leftAnchor.rect = anchorSprite;
			}
			
			selectedTab = tab;

			Log.Debug("Selected tab {0}", selectedTab.tabIndex);
			
			PopulatePurchasedItemsForTab(selectedTab);
		}
		
		private void DeselectAllTabs()
		{
			foreach (var tab in purchasedItemsHighlightedTabs)
			{
				tab.SetActive(false);
			}
			
			foreach (var tab in purchasedItemsTabs)
			{
				tab.SetActive(true);
			}
		}

	}
}

