using UnityEngine;
using RuzikOdyssey.Common;
using System;

namespace RuzikOdyssey.Level
{
	public sealed class LevelView : ExtendedMonoBehaviour
	{
		public GameObject playerWonLevelPopup;
		public UILabel totalLevelScore;

		public float wonLevelUIDelay = 1.0f;

		private void Awake()
		{
			SubscribeToEvents();
		}

		private void SubscribeToEvents()
		{
			EventBroker.Subscribe<PlayerWonLevelEventArgs>(Events.Level.PlayerWonLevel, Level_PlayerWonLevel);
		}

		private void Level_PlayerWonLevel(object sender, PlayerWonLevelEventArgs e)
		{
			Log.Debug("Player Won!!!!!");

			totalLevelScore.text = e.TotalLevelScore.ToString();

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
	}
}
