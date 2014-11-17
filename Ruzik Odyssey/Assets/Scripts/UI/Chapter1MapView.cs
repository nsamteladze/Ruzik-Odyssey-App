using UnityEngine;
using System;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.UI 
{
	public class Chapter1MapView : ExtendedMonoBehaviour
	{	
		public GameObject levelDescriptionPopup;

		public MapLocation[] levels;

		private void Awake()
		{
			// Instantiates GlobalModel if it doesn't exist
			GlobalModel.Connect();
		}
		
		private void Start()
		{
			var gameProgress = GlobalModel.Progress;

			var chapter = GlobalModel.Progress.Chapters[0];

			for (int i = 0; i < chapter.Levels.Count; i++)
			{
				var level = chapter.Levels[i];

				levels[i].LocationMedalsLabel.text = String.Format("{0}/{1}", level.Medals, level.MaxMedals);
				levels[i].LocationPointer.SetActive(!level.IsLocked);
				levels[i].LocationLock.SetActive(level.IsLocked);
				levels[i].CurrentLocationArrow.SetActive(gameProgress.CurrentLevelIndex == i);
			}
		}

		public void SelectLevel(int index)
		{
			if (GlobalModel.Progress.CurrentChapter.Levels[index].IsLocked) return;

			GlobalModel.CurrentLevelIndex.Value = index;
			GlobalModel.Save();

			GoToMainScreen();
		}

		public void ShowLevelDescriptionPopup()
		{
			levelDescriptionPopup.SetActive(true);
		}

		public void HideLevelDescriptionPopup()
		{
			levelDescriptionPopup.SetActive(false);
		}

		public void GoToMainScreen()
		{
			Application.LoadLevel("main_screen");
		}

		public void ChooseLevel()
		{
			Application.LoadLevel("main_screen");
		}
	}
}

