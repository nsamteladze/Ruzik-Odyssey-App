using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using RuzikOdyssey;
using RuzikOdyssey.Common;
using RuzikOdyssey.Domain;
using RuzikOdyssey.Level;
using RuzikOdyssey.UI.Elements;

namespace RuzikOdyssey.UI.Views
{
	public sealed class SmallItemsStoreSceneView : ExtendedMonoBehaviour
	{
		public TextAsset storeItemsConfigFile;

		public UILabel goldAmountLabel;
		public UILabel cornAmountLabel;

		public GameObject[] highlightedTabs;
		public GameObject[] tabs;

		public GameObject storeItemsScrollView;
		public GameObject storeItemTemplate;
		public UISprite currentItemImage;
		public UILabel currentItemCaption;

		private IList<StoreItemsCategory> itemsCategories;

		private int selectedTabIndex = 0;

		private void Awake()
		{
			InitializeUi();
		}

		private void Start()
		{
			LoadStoreItems();

			Log.Debug("Screen width: {0}", Screen.width);

			PopulateItemsForTab(selectedTabIndex);
		}

		private void ClearItemsScrollView()
		{
			storeItemsScrollView.DestroyAllChildren();
		}

		private void SelectItemInCurrentCategory(int itemIndex)
		{
			Log.Debug("Tab: {0}, Element: {1}", selectedTabIndex, itemIndex);

			var item = itemsCategories[selectedTabIndex].Items[itemIndex];

			currentItemImage.spriteName = item.SpriteName;

			currentItemCaption.text = item.Name;
		}

		private void PopulateItemsForTab(int tabIndex)
		{
			if (itemsCategories.Count < tabIndex + 1) return;

			GameObject previousStoreItem = null;
			for (int i = 0; i < itemsCategories[tabIndex].Items.Count; i++)
			{
				var item = itemsCategories[tabIndex].Items[i];

				var storeItem = NGUITools.AddChild(storeItemsScrollView, storeItemTemplate);
				storeItem.SingleChild().GetComponentOrThrow<UISprite>().spriteName = item.ThumbnailName;

				// Capture index for the anonimous delegate closure
				var capturedIndex = i;
				UIEventListener.Get(storeItem.GetComponentOrThrow<UIButton>().gameObject).onClick += (obj) => 
				{ 
					SelectItemInCurrentCategory(capturedIndex); 
				};
				
				var storeItemSprite = storeItem.GetComponentOrThrow<UISprite>();
				
				storeItemSprite.topAnchor.target = storeItemsScrollView.transform;
				storeItemSprite.topAnchor.absolute = 0;
				
				storeItemSprite.bottomAnchor.target = storeItemsScrollView.transform;
				storeItemSprite.topAnchor.absolute = 0;
				
				if (previousStoreItem == null) 
				{
					currentItemImage.spriteName = item.SpriteName;
					currentItemCaption.text = item.Name;
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
			
			storeItemsScrollView.GetComponentOrThrow<UIScrollView>().ResetPosition();
		}
		
		private void InitializeUi()
		{
			goldAmountLabel.text = GlobalModel.Gold.Value.ToString();
			cornAmountLabel.text = GlobalModel.Corn.Value.ToString();
		}

		private void LoadStoreItems()
		{
			itemsCategories = JsonConvert.DeserializeObject<List<StoreItemsCategory>>(storeItemsConfigFile.text);

			if (itemsCategories == null) throw new UnityException("Failed to deserialize store items from the config file.");

			Log.Info("Loaded {0} categories into the store.", itemsCategories.Count);
		}
		
		private void SubscribeToEvent()
		{
			EventsBroker.Subscribe<PropertyChangedEventArgs<int>>(Events.Global.GoldPropertyChanged, Gold_PropertyChanged);
			EventsBroker.Subscribe<PropertyChangedEventArgs<int>>(Events.Global.CornPropertyChanged, Corn_PropertyChanged);
		}
		
		private void Gold_PropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			goldAmountLabel.text = e.PropertyValue.ToString();
		}
		
		private void Corn_PropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			cornAmountLabel.text = e.PropertyValue.ToString();
		}

		public void SelectTab(ItemsStoreTab tab)
		{
			ClearItemsScrollView();

			var tabIndex = tab.tabIndex;

			// De-highlight the currently selected tab
			highlightedTabs[selectedTabIndex].SetActive(false);
			tabs[selectedTabIndex].SetActive(true);

			// Anchor the tab next to the previously highlighted to the regular version of the tab
			if (selectedTabIndex < tabs.Length - 1) 
			{
				var tabOnRight = tabs[selectedTabIndex + 1];
				var tabOnRightSprite = tabOnRight.GetComponentOrThrow<UISprite>();

				var anchorSprite = tabs[selectedTabIndex].GetComponentOrThrow<UISprite>();
				var anchorTransform = tabs[selectedTabIndex].transform;
				
				tabOnRightSprite.leftAnchor.target = anchorTransform;
				tabOnRightSprite.leftAnchor.rect = anchorSprite;
			}

			// Highlight the new tab
			highlightedTabs[tabIndex].SetActive(true);
			tabs[tabIndex].SetActive(false);

			// Anchor the next tab to the highlighted version of the tab
			if (tabIndex < tabs.Length - 1) 
			{
				var tabOnRight = tabs[tabIndex + 1];
				var tabOnRightSprite = tabOnRight.GetComponentOrThrow<UISprite>();

				var anchorSprite = highlightedTabs[tabIndex].GetComponentOrThrow<UISprite>();
				var anchorTransform = highlightedTabs[tabIndex].transform;

				tabOnRightSprite.leftAnchor.target = anchorTransform;
				tabOnRightSprite.leftAnchor.rect = anchorSprite;
			}

			selectedTabIndex = tabIndex;

			PopulateItemsForTab(selectedTabIndex);
		}

		private void DeselectAllTabs()
		{
			foreach (var tab in highlightedTabs)
			{
				tab.SetActive(false);
			}

			foreach (var tab in tabs)
			{
				tab.SetActive(true);
			}
		}
	}
}

