using UnityEngine;
using GoogleMobileAds.Api;
using System;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Ads
{
	public sealed class AdMobAdsService : MonoBehaviour
	{
		private static readonly AdMobAdsService instance = new AdMobAdsService();
		public static AdMobAdsService Instance { get { return instance; } }

		private InterstitialAd interstitialAd;
		
		static AdMobAdsService() {}
		private AdMobAdsService() 
		{
			#if UNITY_EDITOR
			string adUnitId = "unused";
			#elif UNITY_ANDROID
			string adUnitId = "INSERT_ANDROID_INTERSTITIAL_AD_UNIT_ID_HERE";
			#elif UNITY_IPHONE
			string adUnitId = "ca-app-pub-7907261852954179/3530751844";
			#else
			string adUnitId = "unexpected_platform";
			#endif
			
			interstitialAd = new InterstitialAd(adUnitId);

			interstitialAd.AdLoaded += InterstitialAd_Loaded;
			interstitialAd.AdFailedToLoad += InterstitialAd_FailedToLoad;
			interstitialAd.AdOpened += InterstitialAd_Opened;
			interstitialAd.AdClosing += InterstitialAd_Closing;
			interstitialAd.AdClosed += InterstitialAd_Closed;
			interstitialAd.AdLeftApplication += InterstitialAd_LeftApplication;

			interstitialAd.LoadAd(CreateAdRequest());
		}

		public void RequestInterstitialAd()
		{
			interstitialAd.LoadAd(CreateAdRequest());
		}

		public bool InterstitialAdIsReady()
		{
			return interstitialAd.IsLoaded();
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
		
		public bool ShowInterstitialAd()
		{
			if (interstitialAd.IsLoaded())
			{
				interstitialAd.Show();
				return true;
			}
			else
			{
				Log.Warning("Interstitial is not ready yet.");
				return false;
			}
		}

		public void HideInterstitialAd()
		{
			interstitialAd.Destroy();
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
			Log.Debug("HandleInterstitialClosed event received");
		}
		
		private void InterstitialAd_LeftApplication(object sender, EventArgs args)
		{
			Log.Debug("HandleInterstitialLeftApplication event received");
		}

	}
}
