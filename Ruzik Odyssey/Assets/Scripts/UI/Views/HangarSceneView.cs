using UnityEngine;
using RuzikOdyssey.Common;
using RuzikOdyssey;
using RuzikOdyssey.Level;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using RuzikOdyssey.UI.Elements;

namespace RuzikOdyssey.UI.Views
{
	public sealed class HangarSceneView : ExtendedMonoBehaviour
	{
		public TextAsset hangarItemsConfigFile;

		public GameObject[] highlightedTabs;
		public GameObject[] tabs;
		
		public GameObject itemsScrollView;
		public GameObject itemTemplate;
		
		private IList<StoreItemsCategory> itemsCategories;
		
		private int selectedTabIndex = 0;
		
		private void Awake()
		{
			InitializeUi();
		}
		
		private void Start()
		{
			LoadHangarItems();
			
			PopulateItemsForTab(selectedTabIndex);
		}
		
		private void ClearItemsScrollView()
		{
			itemsScrollView.DestroyAllChildren();
		}
		
		private void SelectItemInCurrentCategory(int itemIndex)
		{
		}
		
		private void PopulateItemsForTab(int tabIndex)
		{
			if (itemsCategories.Count < tabIndex + 1) return;
			
			GameObject previousStoreItem = null;
			for (int i = 0; i < itemsCategories[tabIndex].Items.Count; i++)
			{
				var item = itemsCategories[tabIndex].Items[i];
				
				var storeItem = NGUITools.AddChild(itemsScrollView, itemTemplate);
				storeItem.SingleChild().GetComponentOrThrow<UISprite>().spriteName = item.ThumbnailName;
				
				// Capture index for the anonimous delegate closure
				var capturedIndex = i;
				UIEventListener.Get(storeItem.GetComponentOrThrow<UIButton>().gameObject).onClick += (obj) => 
				{ 
					SelectItemInCurrentCategory(capturedIndex); 
				};
				
				var storeItemSprite = storeItem.GetComponentOrThrow<UISprite>();
				
				storeItemSprite.topAnchor.target = itemsScrollView.transform;
				storeItemSprite.topAnchor.absolute = 0;
				
				storeItemSprite.bottomAnchor.target = itemsScrollView.transform;
				storeItemSprite.topAnchor.absolute = 0;
				
				if (previousStoreItem != null)
				{
					storeItemSprite.leftAnchor.target = previousStoreItem.transform;
					storeItemSprite.leftAnchor.absolute = 0;
					storeItemSprite.leftAnchor.relative = 1f;
				}
				
				storeItemSprite.updateAnchors = UIRect.AnchorUpdate.OnUpdate;
				
				storeItemSprite.ResetAndUpdateAnchors();
				
				previousStoreItem = storeItem;
			}
			
			itemsScrollView.GetComponentOrThrow<UIScrollView>().ResetPosition();
		}
		
		private void InitializeUi()
		{
		}
		
		private void LoadHangarItems()
		{
			itemsCategories = JsonConvert.DeserializeObject<List<StoreItemsCategory>>(hangarItemsConfigFile.text);
			
			if (itemsCategories == null) throw new UnityException("Failed to deserialize store items from the config file.");
			
			Log.Info("Loaded {0} categories into the store.", itemsCategories.Count);
		}
		
		private void SubscribeToEvent()
		{
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

