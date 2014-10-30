using UnityEngine;
using System;
using RuzikOdyssey.Level;
using RuzikOdyssey.Common;
using RuzikOdyssey.Infrastructure;
using System.Collections.Generic;

namespace RuzikOdyssey
{
	public sealed class GameProgress
	{
		public IList<GameChapter> Chapters { get; private set; }
	}

	public sealed class GameChapter
	{
		public IList<GameLevel> Levels { get; private set; }

		public int Number { get; set; }
		public string Name { get; set; }
		public bool IsLocked { get; set; }
	}

	public sealed class GameLevel
	{
		public string Name { get; set; }
		public int Number { get; set; }
		public bool IsLocked { get; set; }
		public int Medals { get; set; }
		public int MaxMedals { get; set; }
	}

	public sealed class GameModel
	{
		private GameContext context;

		public Property<int> Gold { get; set; }
		public Property<int> Corn { get; set; }
		public Property<int> Gas { get; set; }

		public GameProgress GameProgress { get; set; } 

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
			var entity = new GameModelEntity
			{
				Gold = Gold.Value,
				Corn = Corn.Value,
				Gas = Gas.Value,
			};

			context.SaveGameModelEntity(entity);
		}

		private void Initialize()
		{
			Gold = new Property<int>("Global_Gold");
			Corn = new Property<int>("Global_Corn");
			Gas = new Property<int>("Global_Gas");

			Gold.PublishEvents();
			Corn.PublishEvents();
			Gas.PublishEvents();
		}

		private void Load()
		{
			Log.Debug("Loading GameModel");

			var entity = context.LoadGameModelEntity();

			if (entity == null) 
			{
				Log.Warning("Failed to load game model");
				Gold.Value = 0;
				Corn.Value = 0;
				Gas.Value = 10;

				Save();
				return;
			}

			Gold.Value = entity.Gold;
			Corn.Value = entity.Corn;
			Gas.Value = entity.Gas;
		}

		public void Connect() { }
	}




}