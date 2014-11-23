using UnityEngine;
using System;
using RuzikOdyssey.Level;
using RuzikOdyssey.Common;
using RuzikOdyssey.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace RuzikOdyssey
{
	public static class GameProgressExtensions
	{
		public static GameChapter GetCurrentChapter(this GameProgress progress)
		{
			return progress.CurrentChapterIndex < progress.Chapters.Count 
				? progress.Chapters[progress.CurrentChapterIndex] 
				: null; 
		}

		public static GameLevel GetCurrentLevel(this GameProgress progress)
		{
			var currentChapter = progress.GetCurrentChapter();
			
			if (currentChapter == null) return null;
			if (progress.CurrentLevelIndex >= currentChapter.Levels.Count) return null;

			return currentChapter.Levels[progress.CurrentLevelIndex]; 
		}
	}

	public sealed class GameProgress
	{
		public int Gold { get; set; }
		public int Corn { get; set; }
		public int Gas { get; set; }

		public int CurrentChapterIndex { get; set; }
		public int CurrentLevelIndex { get; set; }

		public int CurrentLevelDifficulty { get; set; }

		public IList<GameChapter> Chapters { get; set; }

		public override string ToString ()
		{
			return string.Format ("[GameProgress: Gold={0}, Corn={1}, Gas={2}, CurrentChapterIndex={3}, CurrentLevelIndex={4}, CurrentLevelDifficulty={5}, Chapters={6}]", Gold, Corn, Gas, CurrentChapterIndex, CurrentLevelIndex, CurrentLevelDifficulty, Chapters);
		}
	}

	public sealed class GameChapter
	{
		public int Number { get; set; }
		public string Name { get; set; }
		public bool IsLocked { get; set; }

		public IList<GameLevel> Levels { get; set; }
	}

	public sealed class GameLevel
	{
		public int Number { get; set; }
		public string Name { get; set; }
		public bool IsLocked { get; set; }
		public int Medals { get; set; }
		public int MaxMedals { get; set; }
	}

	public sealed class GameModel
	{
		private GameContext context;

		public Property<int> Gold 
		{ 
			get; 
			set; 
		}
		public Property<int> Corn { get; set; }
		public Property<int> Gas { get; set; }

		public Property<int> CurrentLevelIndex { get; set; }
		public Property<int> CurrentLevelDifficulty { get; set; }

		public GameProgress Progress { get; set; } 

		public LevelDesign CurrentLevelDesign
		{
			get
			{
				return Content.Value
					.Chapters[Progress.CurrentChapterIndex]
					.Levels[Progress.CurrentLevelIndex]
					.Design;
			}
		}

		public Lazy<GameContent> Content { get; private set; }

		private static readonly GameModel instance = new GameModel();
		public static GameModel Instance { get { return instance; } }
			
		static GameModel() {}
		private GameModel() 
		{
			context = new GameContext();

			Initialize();
			Load();
		}

		public void Save()
		{
			Progress.Gold = Gold.Value;
			Progress.Corn = Corn.Value;
			Progress.Gas = Gas.Value;
			Progress.CurrentLevelIndex = CurrentLevelIndex.Value;
			Progress.CurrentLevelDifficulty = CurrentLevelDifficulty.Value;

			context.SaveEntity<GameProgress>(Progress);
		}

		private void Initialize()
		{
			Gold = new Property<int>(Properties.Global.Gold);
			Corn = new Property<int>(Properties.Global.Corn);
			Gas = new Property<int>(Properties.Global.Gas);

			CurrentLevelIndex = new Property<int>(Properties.Global.CurrentLevelIndex);
			CurrentLevelDifficulty = new Property<int>(Properties.Global.CurrentLevelDifficulty);

			Content = new Lazy<GameContent>(() => context.LoadGameContent());
		}

		private void Load()
		{
			this.Progress = context.LoadEntity<GameProgress>();

			if (Progress == null)
			{
				Log.Error("Failed to load Game Progress into game model");
				return;
			}

			Gold.Value = Progress.Gold;
			Corn.Value = Progress.Corn;
			Gas.Value = Progress.Gas;
			CurrentLevelIndex.Value = Progress.CurrentLevelIndex;
			CurrentLevelDifficulty.Value = Progress.CurrentLevelDifficulty;
		}

		public void Connect() 
		{ 
			/* REMARK
			 * This method is not doing anything.
			 * It is used just to properly instantiate GameModel from the 
			 * first loaded MonoBehaviour component.
			 */
		}
	}




}