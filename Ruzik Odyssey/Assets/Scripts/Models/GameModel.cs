using UnityEngine;
using System;
using RuzikOdyssey.Level;
using RuzikOdyssey.Common;
using RuzikOdyssey.Infrastructure;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using RuzikOdyssey.Domain;
using RuzikOdyssey.Domain.Inventory;

namespace RuzikOdyssey.Models
{
	public sealed class GameModel
	{
		private GameContext context;
		public bool IsInitialized { get; set; }

		public Property<int> Gold 
		{ 
			get; 
			set; 
		}
		public Property<int> Corn { get; set; }
		public Property<int> Gas { get; set; }

		public Property<int> CurrentLevelIndex { get; set; }
		public Property<int> CurrentLevelDifficulty { get; set; }

		public GameProgress Progress { get; private set; } 
		public GameContent Content { get; private set; }
		public GameInventory Inventory { get; private set; }

		public LevelDesign CurrentLevelDesign
		{
			get
			{
				return Content
					.Chapters[Progress.CurrentChapterIndex]
					.Levels[Progress.CurrentLevelIndex]
					.Design;
			}
		}

		private static readonly GameModel instance = new GameModel();
		public static GameModel Instance { get { return instance; } }
			
		static GameModel() {}
		private GameModel() 
		{
			context = new GameContext();

			Gold = new Property<int>(Properties.Global.Gold, true);
			Corn = new Property<int>(Properties.Global.Corn, true);
			Gas = new Property<int>(Properties.Global.Gas, true);
			CurrentLevelIndex = new Property<int>(Properties.Global.CurrentLevelIndex, true);
			CurrentLevelDifficulty = new Property<int>(Properties.Global.CurrentLevelDifficulty, true);

			PublishEvents();
		}

		public event EventHandler<ProgressUpdatedEventsArgs> LoadingProgressUpdated;
		public event EventHandler<EventArgs> LoadingFinished;

		private void OnLoadingProgressUpdated(string actionName, int actionProgress)
		{
			if (LoadingProgressUpdated != null) 
				LoadingProgressUpdated(this, 
				                       new ProgressUpdatedEventsArgs 
				                       { ActionName = actionName, ActionProgress = actionProgress });
		}

		private void OnLoadingFinished()
		{
			if (LoadingFinished != null) LoadingFinished(this, EventArgs.Empty);
		}

		private void PublishEvents()
		{
			EventsBroker.Publish<ProgressUpdatedEventsArgs>(Events.Global.ModelLoadingProgressUpdated, ref LoadingProgressUpdated);
			EventsBroker.Publish<EventArgs>(Events.Global.ModelLoadingFinished, ref LoadingFinished);
		}

		public IEnumerator InitializeAsync()
		{
			if (IsInitialized) 
			{
				OnLoadingFinished();
				yield break;
			}

			// Load game progress

			OnLoadingProgressUpdated("Loading game progress", 0);

			this.Progress = context.LoadEntity<GameProgress>();
			
			if (Progress == null) throw new Exception("Failed to load Game Progress into game model");

			OnLoadingProgressUpdated("Loading game progress", 95);

			Gold.Value = Progress.Gold;
			Corn.Value = Progress.Corn;
			Gas.Value = Progress.Gas;
			CurrentLevelIndex.Value = Progress.CurrentLevelIndex;
			CurrentLevelDifficulty.Value = Progress.CurrentLevelDifficulty;

			// Load game content

			OnLoadingProgressUpdated("Loading game content", 0);

			var iterator = context.LoadGameContentAsync(
				(content) => this.Content = content,
				(x) => OnLoadingProgressUpdated("Loading game content", x));

			while (iterator.MoveNext()) yield return null;

			// Load inventory

			OnLoadingProgressUpdated("Loading inventory", 0);
			
			this.Inventory = context.LoadEntity<GameInventory>(new InventoryItemConverter());
			
			if (this.Inventory == null) throw new Exception("Failed to load inventory into game model");

			OnLoadingProgressUpdated("Loading inventory", 95);

			Log.Info("Fisnihed initializing game model");

			IsInitialized = true;
			OnLoadingFinished();
		}

		public void Connect() 
		{ 
			/* REMARK
			 * This method is not doing anything.
			 * It is used just to properly instantiate GameModel from the 
			 * first loaded MonoBehaviour component.
			 */
			var iterator = InitializeAsync();
			while (iterator.MoveNext()) continue;
		}

		public void Save()
		{
			Progress.Gold = Gold.Value;
			Progress.Corn = Corn.Value;
			Progress.Gas = Gas.Value;
			Progress.CurrentLevelIndex = CurrentLevelIndex.Value;
			Progress.CurrentLevelDifficulty = CurrentLevelDifficulty.Value;
			
			context.SaveEntity<GameProgress>(this.Progress);
			context.SaveEntity<GameInventory>(this.Inventory);
		}
	}




}