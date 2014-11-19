using UnityEngine;
using System;
using RuzikOdyssey.Common;
using System.Linq;
using System.Collections.Generic;

namespace RuzikOdyssey.UI
{
	public class GlobalMapView : ExtendedMonoBehaviour
	{
		public GameObject[] chapters;
		public GameObject[] currentLocationArrows;
		public GameObject[] locationLocks;
		public GameObject[] locationMedals;
		public UILabel[] locationMedalsLabels;

		private void Awake()
		{
			GlobalModel.Connect();
		}

		private void Start()
		{
			var gameProgress = GlobalModel.Progress;

			for (int i = 0; i < gameProgress.Chapters.Count; i++)
			{
				var chapter = gameProgress.Chapters[i];

				var medals = chapter.Levels.XSum(x => x.Medals);
				var maxMedals = chapter.Levels.XSum(x => x.MaxMedals);

				locationMedals[i].SetActive(!chapter.IsLocked);
				locationMedalsLabels[i].text = String.Format("{0}/{1}", medals, maxMedals);
				locationLocks[i].SetActive(chapter.IsLocked);
				currentLocationArrows[i].SetActive(gameProgress.CurrentChapterIndex == i);
			}
		}

		public void LoadChapter1MapScreen()
		{
			Application.LoadLevel("chapter1_map_screen"); 
		}

		public void LoadMainScreen()
		{
			Application.LoadLevel("main_screen");
		}
	}

	public static class LinqExtensions
	{
		public static int XSum(this IEnumerable<GameLevel> collection, Func<GameLevel, int> selector)
		{
			var result = 0;
			foreach (var item in collection) result += selector(item);
			return result;
		}
	}
}

