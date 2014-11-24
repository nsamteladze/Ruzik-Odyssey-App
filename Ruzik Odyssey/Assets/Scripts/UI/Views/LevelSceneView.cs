using UnityEngine;
using RuzikOdyssey.Common;
using System;
using RuzikOdyssey.Level;
using RuzikOdyssey.Domain;
using RuzikOdyssey.ViewModels;
using RuzikOdyssey.UI;

namespace RuzikOdyssey.Views
{
	public sealed class LevelSceneView : ExtendedMonoBehaviour
	{
		public GameObject playerWonLevelPopup;

		public float wonLevelUIDelay = 1.0f;

		public LevelSceneViewModel viewModel;

		public UIToggle shieldToggle;
		public UILabel scoreLabel;
		public UILabel missileAmmoLabel;

		public event EventHandler<EventArgs> FireMissileButtonClicked;
		public event EventHandler<ToggleStateChangedEventArgs> ShieldToggleStateChanged;

		private void Awake()
		{
			viewModel.PlayerWonLevel += ViewModel_PlayerWonLevel;

			this.FireMissileButtonClicked += viewModel.View_FireMissileButtonClicked;
			this.ShieldToggleStateChanged += viewModel.View_ShieldToggleStateChanged;
		}

		private void Start()
		{
			missileAmmoLabel.BindTo(viewModel.MissileAmmo);
			scoreLabel.BindTo(viewModel.Score);
		}

		private void ViewModel_PlayerWonLevel(object sender, PlayerWonLevelEventArgs e)
		{
			Log.Info("Player Won!!!!!");

			Invoke("ShowPlayerWonLevelPopup", wonLevelUIDelay);
		}

		public void ShowPlayerWonLevelPopup()
		{
			playerWonLevelPopup.SetActive(true);
		}

		public void HidePlayerWonLevelPopup()
		{
			playerWonLevelPopup.SetActive(false);
		}

		public void OnFireMissileButtonClicked()
		{
			if (FireMissileButtonClicked != null) FireMissileButtonClicked(this, EventArgs.Empty);
		}

		public void OnShieldToggleStateChanged()
		{
			var isOn = shieldToggle.value;

			if (ShieldToggleStateChanged != null) 
				ShieldToggleStateChanged(this, new ToggleStateChangedEventArgs { ToggleIsOn = isOn });
		}
	}
}
