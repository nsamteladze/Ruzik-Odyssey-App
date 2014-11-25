using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RuzikOdyssey.Common;
using System.Linq;
using System;
using RuzikOdyssey.Ai;
using RuzikOdyssey.Enemies;
using Random = UnityEngine.Random;
using Newtonsoft.Json;
using RuzikOdyssey.Models;
using RuzikOdyssey.Domain;
using RuzikOdyssey.Level;
using RuzikOdyssey.Player;

namespace RuzikOdyssey.ViewModels
{
	public sealed class LevelSceneViewModel : ExtendedMonoBehaviour
	{
		public GameObject[] enemiesSource;
		public GameObject[] obstaclesSource;

		public float enemiesSpawningDelay = 1.0f;

		public int cornForCompletedLevel = 2;

		public RuzikController playerController; 

		private LevelDesign levelDesign;
		private WaveTemplatesCollectionIterator enemiesIterator;
		private WaveTemplatesCollectionIterator obstaclesIterator;

		private bool isFinalEnemiesWave = false;
		private bool isLevelFinished = false;
		private int finalEnemiesCouter = 0;

		public Property<int> Score { get; private set; }
		public Property<int> MissileAmmo { get; private set; }

		private void Awake()
		{
//			this.Score = new Property<int>(0);
//			this.MissileAmmo = new Property<int>(playerController.weaponController.missileAmmo);

			this.Score = new Property<int>();
			this.MissileAmmo = new Property<int>();

			playerController.Died += Player_Died;
			playerController.weaponController.MissileAmmoChanged += Player_MissileAmmoChanged;

			GlobalModel.Connect();
		}

		private void Start ()
		{
			this.Score.Value = 0;
			this.MissileAmmo.Value = playerController.weaponController.missileAmmo;

			LoadLevelDesign();

			StartCoroutine(SpawnLevelFromIterator(enemiesIterator, enemiesSource, (item) => 
      		{
				var alienController = item.GetComponentOrThrow<AlienController>();
				alienController.Died += Enemy_Died;
				alienController.Destroyed += Enemy_Destroyed;
			}));
			
			StartCoroutine(SpawnLevelFromIterator(obstaclesIterator, obstaclesSource));
		}

		#region DEBUG

		public event EventHandler<PropertyChangedEventArgs<int>> ScorePropertyChanged;
		public void OnScorePropertyChanged(int newValue)
		{
			Log.Debug("LevelSceneViewModel - Player_MissileAmmoChanged - Raising MissileAmmoChanged event");
			
			if (ScorePropertyChanged != null) 
				ScorePropertyChanged(this, new PropertyChangedEventArgs<int> { PropertyValue = newValue });
		}

		#endregion

		private void LoadLevelDesign()
		{
			var levelDesign = GlobalModel.CurrentLevelDesign;

			if (levelDesign == null)
			{
				Log.Error("Global Model returned NULL for current level design. Chapter: {0}, Level: {1}.",
				          GlobalModel.Progress.CurrentChapterIndex, GlobalModel.Progress.CurrentLevelIndex);
				return;
			}

			Log.Info("Loaded level design with {0} enemies waves and {1} obstacles waves.",
			         levelDesign.Enemies.Count, levelDesign.Obstacles.Count);

			enemiesIterator = new WaveTemplatesCollectionIterator(levelDesign.Enemies);

			enemiesIterator.LastItemReturned += LevelDesign_LastEnemyWaveReturned;

			obstaclesIterator = new WaveTemplatesCollectionIterator(levelDesign.Obstacles);	
		}

		private void OnApplicationQuit()
		{
			GlobalModel.Save();
		}

		private void LevelDesign_LastEnemyWaveReturned(object sender, EventArgs e)
		{
			isFinalEnemiesWave = true;

			finalEnemiesCouter = GameObject.FindGameObjectsWithTag(Tags.Enemy).Count();

			Log.Debug("finalEnemiesCouter = {0}", finalEnemiesCouter);

			if (finalEnemiesCouter == 0) OnPlayerWonLevel();
		}

		private void Enemy_Died(object sender, EnemyDiedEventArgs e)
		{
			Log.Debug("{0} is dead. Adding {1} to score.", sender.ToString(), e.ScoreForKill);

			this.Score.Value += e.ScoreForKill;

//			Log.Debug("New level score is {0}", Score.Value);
//
//			Log.Debug("Calling OnScorePropertyChanged");
//			OnScorePropertyChanged(Score.Value);
		}

		private void Enemy_Destroyed(object sender, EventArgs e)
		{
			Log.Debug("Destroyed {0}", sender.ToString());

			if (isFinalEnemiesWave)
			{
				finalEnemiesCouter--;
				
				Log.Debug("finalEnemiesCouter = {0}", finalEnemiesCouter);
				
				if (finalEnemiesCouter == 0) OnPlayerWonLevel();
			}
		}

		private void Player_Died(object sender, EventArgs e)
		{
			Log.Info("Player died!!!");

			LoadDashboardScene();
		}

		private void Player_MissileAmmoChanged(object sender, AmmoChangedEventArgs e)
		{
			this.MissileAmmo.Value = e.NewValue;
		}

		public event EventHandler<PlayerWonLevelEventArgs> PlayerWonLevel;

		private void OnPlayerWonLevel()
		{
			isLevelFinished = true;

			var e = new PlayerWonLevelEventArgs { TotalLevelScore = Score.Value };
			if (PlayerWonLevel != null) PlayerWonLevel(this, e); 
		}

		public void ExitLevel()
		{
			if (isLevelFinished) 
			{
				GlobalModel.Gold.Value += Score.Value;
				GlobalModel.Corn.Value += cornForCompletedLevel;

				GlobalModel.Save();
			}

			LoadDashboardScene();
		}

		private IEnumerator SpawnLevelFromIterator(WaveTemplatesCollectionIterator iterator, 
		                                           IList<GameObject> objectsSource,
		                                           Action<GameObject> action = null)
		{
			while (!isLevelFinished)
			{
				var waveTemplate = iterator.GetNext();
				
				if (waveTemplate == null) yield break;
				
				var waveObjects = waveTemplate.ObjectIndices
					.Select(index => index < objectsSource.Count ? objectsSource[index] : null)
					.Where(x => x != null)
					.ToList();

				InstantiateMany(waveObjects)
					.ForEach(x =>  { 
						if (action != null) action(x);
					});
				
				yield return new WaitForSeconds(waveTemplate.NextWaveInterval);
			}
		}

		private List<GameObject> InstantiateMany(IList<GameObject> objects)
		{
			return objects
				.Select(item =>
		        {
					var position = new Vector2(Game.WarzoneBounds.Right() + Random.Range(3, 20), 
					                           Game.WarzoneBounds.RandomVerticalPositionWithinBounds(item));
					return Instantiate(item, position, transform.rotation) as GameObject;
				})
				.ToList();
		}

		public void View_FireMissileButtonClicked(object sender, EventArgs e)
		{
			playerController.FireMissile();
		}

		public void View_ShieldToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
		{
			playerController.SetShieldActive(e.ToggleIsOn);
		}

		public void View_LevelExited(object sender, EventArgs e)
		{
			Log.Debug("LevelSceneViewModel - View_LevelExited");
		}

		public void View_LevelResumed(object sender, EventArgs e)
		{
			Log.Debug("LevelSceneViewModel - View_LevelResumed");
		}

		public void View_LevelRestarted(object sender, EventArgs e)
		{
			Log.Debug("LevelSceneViewModel - View_LevelRestarted");
		}

		public void View_LevelPaused(object sender, EventArgs e)
		{
			Log.Debug("LevelSceneViewModel - View_LevelPaused");
		}
	}
}
