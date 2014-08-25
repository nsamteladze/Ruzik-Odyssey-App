using UnityEngine;
using System;
using GoogleMobileAds.Api;
using RuzikOdyssey.Common;

public class MainScreenUI : MonoBehaviour
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

	public GameObject storePopup;

	private void Awake()
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

	public void StartMission()
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

	public void ShowGlobalMap()
	{
		Application.LoadLevel("global_map_screen"); 
	}

	public void ShowStoreCategoriesPopup()
	{
		storePopup.SetActive(true);
	}

	public void HideStoreCategoriesPopup()
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
		Application.LoadLevel("default_level"); 
	}
	
	private void InterstitialAd_LeftApplication(object sender, EventArgs args)
	{
		Log.Debug("HandleInterstitialLeftApplication event received");
	}
}
