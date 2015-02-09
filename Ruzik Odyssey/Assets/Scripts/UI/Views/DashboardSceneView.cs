using System;
using UnityEngine;
using GoogleMobileAds.Api;
using RuzikOdyssey;
using RuzikOdyssey.Common;
using RuzikOdyssey.Domain;
using RuzikOdyssey.Level;

namespace RuzikOdyssey.UI.Views
{
	public sealed class DashboardSceneView : ExtendedMonoBehaviour
	{	
		#if UNITY_EDITOR
		private string adUnitId = "unused";
		#elif UNITY_ANDROID
		private string adUnitId = "INSERT_ANDROID_INTERSTITIAL_AD_UNIT_ID_HERE";
		#elif UNITY_IPHONE
		private string adUnitId = "ca-app-pub-1384659154698612/7265813883";
		#else
		private string adUnitId = "unexpected_platform";
		#endif

		private InterstitialAd interstitialAd;

		public GameObject storePopup;

		public UILabel goldAmountLabel;
		public UILabel cornAmountLabel;
		public UILabel gasAmountLabel;

		public UILabel storeGoldAmountLabel;
		public UILabel storeCornAmountLabel;

		public UILabel currentLevelNameLabel;
		public UILabel currentLevelDifficultyLabel;

		public UISprite aircraftSprite;

		public GameObject videoAdButton;

		private void Awake()
		{
			interstitialAd = new InterstitialAd(adUnitId);
			
			interstitialAd.AdLoaded += InterstitialAd_Loaded;
			interstitialAd.AdFailedToLoad += InterstitialAd_FailedToLoad;
			interstitialAd.AdOpened += InterstitialAd_Opened;
			interstitialAd.AdClosing += InterstitialAd_Closing;
			interstitialAd.AdClosed += InterstitialAd_Closed;
			interstitialAd.AdLeftApplication += InterstitialAd_LeftApplication;
			
			SubscribeToEvent();

			/* TODO
			 * View should not have access to Global Model. 
			 * Create a ViewModel for this view and move Global Model access there.
			 */
			GlobalModel.Connect();
		}

		private void Start()
		{
			videoAdButton.SetActive(Vungle.isAdvertAvailable());
			Vungle.onCachedAdAvailableEvent += Vungle_OnCachedAdAvailableEventHandler;

			interstitialAd.LoadAd(CreateAdRequest());

			InitializeUI();
		}

		private void Vungle_OnCachedAdAvailableEventHandler()
		{
			Log.Debug("START - Vungle_OnCachedAdAvailableEventHandler");

			videoAdButton.SetActive(true);
		}

		private void OnDestroy()
		{
			UnsubscribeFromEvents();
		}

		private void InitializeUI()
		{
			goldAmountLabel.text = GlobalModel.Gold.Value.ToString();
			cornAmountLabel.text = GlobalModel.Corn.Value.ToString();
			gasAmountLabel.text = String.Format("{0}/10", GlobalModel.Gas.Value);

			currentLevelNameLabel.text = GlobalModel.Progress.GetCurrentLevel().Name;

			aircraftSprite.spriteName = GlobalModel.Aircraft.Value.Ui.SceneSpriteName;
		}

		private void SubscribeToEvent()
		{
			EventsBroker.Subscribe<PropertyChangedEventArgs<int>>(Events.Global.GoldPropertyChanged, Gold_PropertyChanged);

			EventsBroker.Subscribe<PropertyChangedEventArgs<int>>(Events.Global.CornPropertyChanged, Corn_PropertyChanged);

			EventsBroker.Subscribe<PropertyChangedEventArgs<int>>(Events.Global.GasPropertyChanged, Gas_PropertyChanged);

			EventsBroker.Subscribe<PropertyChangedEventArgs<int>>(
				Events.Global.CurrentLevelIndexPropertyChanged, 
				CurrentLevelIndex_PropertyChanged);

			EventsBroker.Subscribe<PropertyChangedEventArgs<int>>(
				Events.Global.CurrentLevelDifficultyPropertyChanged, 
				CurrentLevelDifficulty_PropertyChanged);
		}

		public void UnsubscribeFromEvents()
		{
			EventsBroker.Unsubscribe<PropertyChangedEventArgs<int>>(Events.Global.GoldPropertyChanged, Gold_PropertyChanged);
			
			EventsBroker.Unsubscribe<PropertyChangedEventArgs<int>>(Events.Global.CornPropertyChanged, Corn_PropertyChanged);
			
			EventsBroker.Unsubscribe<PropertyChangedEventArgs<int>>(Events.Global.GasPropertyChanged, Gas_PropertyChanged);
			
			EventsBroker.Unsubscribe<PropertyChangedEventArgs<int>>(
				Events.Global.CurrentLevelIndexPropertyChanged, 
				CurrentLevelIndex_PropertyChanged);
			
			EventsBroker.Unsubscribe<PropertyChangedEventArgs<int>>(
				Events.Global.CurrentLevelDifficultyPropertyChanged, 
				CurrentLevelDifficulty_PropertyChanged);
		}

		private void Gold_PropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			goldAmountLabel.text = e.PropertyValue.ToString();
		}

		private void Corn_PropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			cornAmountLabel.text = e.PropertyValue.ToString();
		}

		private void Gas_PropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			gasAmountLabel.text = String.Format("{0}/10", e.PropertyValue);
		}

		private void CurrentLevelIndex_PropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			/* REMARK
			 * GlobalModel can be null when this event handler is invoked the first time 
			 * because the event is triggered when GlobalModel construction is not done yet.
			 * 
			 * The right solution is to use event arguments instead of accessing GlobalModel directly.
			 */
			if (GlobalModel == null) return;

			var currentLevelName = GlobalModel.Progress.GetCurrentLevel().Name;
			this.currentLevelNameLabel.text = currentLevelName;
		}

		private void CurrentLevelDifficulty_PropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			switch (e.PropertyValue)
			{
				case 0: 
					currentLevelDifficultyLabel.text = "Easy";
					break;
				case 1: 
					currentLevelDifficultyLabel.text = "Medium";
					break;
				case 2: 
					currentLevelDifficultyLabel.text = "Hard";
					break;
				default:
					Log.Error("Failed to determine level difficulty for value {0}", e.PropertyValue);
					currentLevelDifficultyLabel.text = "??????";
					break;
			}
		}

		public void StartMission()
		{
			#if UNITY_EDITOR

			LoadLevelScene();

			#endif

			if (interstitialAd.IsLoaded()) 
			{
				interstitialAd.Show();
			}
			else 
			{
				GameEnvironment.StartMission();

				LoadLevelScene();
			}
		}

		public void Game_OnStoreButtonClicked()
		{
			storeGoldAmountLabel.text = GlobalModel.Gold.Value.ToString();
			storeCornAmountLabel.text = GlobalModel.Corn.Value.ToString();

			storePopup.SetActive(true);
		}

		public void IncreaseMissionDifficulty()
		{
			/* TODO
			 * This logic does not belong to the view. Move to ViewModel.
			 */
			if (GlobalModel.CurrentLevelDifficulty.Value < 2) GlobalModel.CurrentLevelDifficulty.Value++;
		}

		public void DecreaseMissionDifficulty()
		{
			/* TODO
			 * This logic does not belong to the view. Move to ViewModel.
			 */
			if (GlobalModel.CurrentLevelDifficulty.Value > 0) GlobalModel.CurrentLevelDifficulty.Value--;
		}

		public void Game_OnCloseStorePopupButtonClicked()
		{
			storePopup.SetActive(false);
		}

		private AdRequest CreateAdRequest()
		{
			return new AdRequest.Builder()
				.AddTestDevice(AdRequest.TestDeviceSimulator)
					.AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
					.AddKeyword("game")
					.SetGender(Gender.Male)
					.SetBirthday(new DateTime(1985, 1, 1))
					.TagForChildDirectedTreatment(false)
					.AddExtra("color_bg", "9B30FF")
					.Build();	
		}

		private void InterstitialAd_Loaded(object sender, EventArgs args)
		{
			Log.Debug("HandleInterstitialLoaded event received.");
		}
		
		private void InterstitialAd_FailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
			Log.Debug("HandleInterstitialFailedToLoad event received with message: " + args.Message);
		}
		
		private void InterstitialAd_Opened(object sender, EventArgs args)
		{
			Log.Debug("HandleInterstitialOpened event received");
		}
		
		private void InterstitialAd_Closing(object sender, EventArgs args)
		{
			Log.Debug("HandleInterstitialClosing event received");
		}
		
		private void InterstitialAd_Closed(object sender, EventArgs args)
		{
			GameEnvironment.StartMission();
			LoadLevelScene();
		}
		
		private void InterstitialAd_LeftApplication(object sender, EventArgs args)
		{
			Log.Debug("HandleInterstitialLeftApplication event received");
		}

		public void Game_ShowVideoAdButtonClicked()
		{
			Log.Debug("START - Game_ShowVideoAdButtonClicked");

			if (Vungle.isAdvertAvailable())
			{
				Log.Info("Playing incentivized video ad from Vungle.");
				Vungle.playAd(true, "test-tag");
			}
			else 
			{
				Log.Warning("Failed to play incentivized video ad. Ad is not available.");
			}
		}
	}
}