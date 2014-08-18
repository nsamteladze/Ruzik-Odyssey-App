using UnityEngine;
using System;
using GoogleMobileAds.Api;
using RuzikOdyssey.Common;

public class MainScreenUI : ScreenUIBase
{	
	#if UNITY_EDITOR
	private string adUnitId = "unused";
	#elif UNITY_ANDROID
	private string adUnitId = "INSERT_ANDROID_INTERSTITIAL_AD_UNIT_ID_HERE";
	#elif UNITY_IPHONE
	private string adUnitId = "ca-app-pub-7907261852954179/3530751844";
	#else
	private string adUnitId = "unexpected_platform";
	#endif

	private InterstitialAd interstitialAd;

	protected override void Initialize()
	{
		interstitialAd = new InterstitialAd(adUnitId);
		
		interstitialAd.AdLoaded += InterstitialAd_Loaded;
		interstitialAd.AdFailedToLoad += InterstitialAd_FailedToLoad;
		interstitialAd.AdOpened += InterstitialAd_Opened;
		interstitialAd.AdClosing += InterstitialAd_Closing;
		interstitialAd.AdClosed += InterstitialAd_Closed;
		interstitialAd.AdLeftApplication += InterstitialAd_LeftApplication;
		
		interstitialAd.LoadAd(CreateAdRequest());
	}

	protected override void InitializeUI()
	{
		if (GUI.Button(new Rect(870 * scaleOffset.x, 970 * scaleOffset.y, 320 * scale, 160 * scale), 
		               "", GUIStyle.none))
		{
			Application.LoadLevel("global_map_screen"); 
		}

		if (GUI.Button(new Rect(1625 * scaleOffset.x, 970 * scaleOffset.y, 320 * scale, 160 * scale), 
		               "", GUIStyle.none))
		{
			if (interstitialAd.IsLoaded()) 
			{
				interstitialAd.Show();
			}
			else 
			{
				GameEnvironment.StartMission();
				Application.LoadLevel("default_level"); 
			}
		}
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
		Application.LoadLevel("default_level"); 
	}
	
	private void InterstitialAd_LeftApplication(object sender, EventArgs args)
	{
		Log.Debug("HandleInterstitialLeftApplication event received");
	}
}
