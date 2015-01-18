using UnityEngine;
using RuzikOdyssey;
using RuzikOdyssey.Common;
using RuzikOdyssey.Domain;
using RuzikOdyssey.Level;
using RuzikOdyssey.ViewModels;
using RuzikOdyssey.UI.Elements;
using System;

namespace RuzikOdyssey.UI.Views
{
	public sealed class StoreSceneView : ExtendedMonoBehaviour
	{
		public UILabel goldAmountLabel;
		public UILabel cornAmountLabel;

		public UIScrollView storeScrollView;
		public GameObject storeItemTemplate;

		public StoreSceneViewModel viewModel;

		private void Awake()
		{
			viewModel.AvailableStoreItemsUpdated += ViewModel_AvailableStoreItemsUpdated;
		}

		private void Start()
		{
			goldAmountLabel.BindTo(viewModel.Gold);
			cornAmountLabel.BindTo(viewModel.Corn);

			PopulateStore();
		}

		private void ViewModel_AvailableStoreItemsUpdated(object sender, EventArgs e)
		{
			RefreshStoreItems();
		}

		private void ClearStoreItems()
		{
			storeScrollView.gameObject.DestroyAllChildren();
		}
		
		private void RefreshStoreItems()
		{
			ClearStoreItems();
			PopulateStore();
		}

		private void PopulateStore()
		{
			var availableItems = viewModel.AvailableStoreItems;

			UIStoreItem previousUiItem = null;
			foreach (var item in availableItems)
			{
				var uiItem = NGUITools
					.AddChild(storeScrollView.gameObject, storeItemTemplate)
					.GetComponentOrThrow<UIStoreItem>();

				uiItem.ImageSprite.spriteName = item.SpriteName;
				
				// Capture index for the anonimous delegate closure
				var capturedId = item.Id;
				UIEventListener.Get(uiItem.BuyButton.gameObject).onClick += (obj) => 
				{ 
					Log.Debug("Buy button clicked for store item id {0}.", capturedId); 
				};
				
				uiItem.ContainerSprite.topAnchor.target = storeScrollView.transform;
				uiItem.ContainerSprite.topAnchor.absolute = 0;
				
				uiItem.ContainerSprite.bottomAnchor.target = storeScrollView.transform;
				uiItem.ContainerSprite.topAnchor.absolute = 0;
				
				if (previousUiItem == null) 
				{
					// If the first store item
				}
				else
				{
					uiItem.ContainerSprite.leftAnchor.target = previousUiItem.transform;
					uiItem.ContainerSprite.leftAnchor.absolute = uiItem.GapBetweenItems;
					uiItem.ContainerSprite.leftAnchor.relative = 1f;
				}
				
				uiItem.ContainerSprite.updateAnchors = UIRect.AnchorUpdate.OnUpdate;
				
				uiItem.ContainerSprite.ResetAndUpdateAnchors();
				
				previousUiItem = uiItem;
			}
			
			storeScrollView.ResetPosition();
		}
	}
}
