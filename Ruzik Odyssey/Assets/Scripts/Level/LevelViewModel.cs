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

namespace RuzikOdyssey.Level
{
	public static class RandomExtensions
	{
		public static float RandomVerticalPositionWithinBounds(this Bounds bounds, GameObject gameObject)
		{
			return Random.Range(bounds.Bottom() + gameObject.RendererSize().y / 2, 
			                    bounds.Top() - gameObject.RendererSize().y / 2);
		}
	}

	public class GameContent
	{
		public string Version { get; set; }
		public IList<ChapterContent> Chapters { get; set; }
	}

	public class ChapterContent
	{
		public int Number { get; set; }
		public IList<LevelContent> Levels { get; set; }
	}

	public class LevelContent
	{
		public int Number { get; set; }
		public LevelDesign Design { get; set; }
	}

	public sealed class LevelViewModel : ExtendedMonoBehaviour
	{
		public GameObject[] enemiesSource;
		public GameObject[] obstaclesSource;

		public float enemiesSpawningDelay = 1.0f;

		public int cornForCompletedLevel = 2;

		private LevelDesign levelDesign;
		private WaveTemplatesCollectionIterator enemiesIterator;
		private WaveTemplatesCollectionIterator obstaclesIterator;

		private LevelEnemiesDesign enemiesLevelDesign; 
		private bool isFinalEnemiesWave = false;
		private bool isLevelFinished = false;
		private int finalEnemiesCouter = 0;

		private LevelModel model;

		private void Awake()
		{
			model = new LevelModel();

			enemiesLevelDesign = this.gameObject.GetComponentOrThrow<LevelEnemiesDesign>();

			enemiesLevelDesign.LastEnemyWaveReturned += LevelDesign_LastEnemyWaveReturned;

			PublishEvents();

			GlobalModel.Connect();
		}

		private void Start ()
		{
			LoadLevelDesign();


			StartCoroutine(SpawnLevelFromIterator(enemiesIterator, enemiesSource, (item) => 
      		{
				var alienController = item.GetComponentOrThrow<AlienController>();
				alienController.Died += Enemy_Died;
				alienController.Destroyed += Enemy_Destroyed;
			}));
			
			StartCoroutine(SpawnLevelFromIterator(obstaclesIterator, obstaclesSource));
		}

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

		private void PublishEvents()
		{
			EventBroker.Publish<PlayerWonLevelEventArgs>(Events.Level.PlayerWonLevel, ref PlayerWonLevel);
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
			Log.Debug("{0} is dead", sender.ToString());

			model.Gold.Value += e.ScoreForKill;
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

		public event EventHandler<PlayerWonLevelEventArgs> PlayerWonLevel;

		private void OnPlayerWonLevel()
		{
			isLevelFinished = true;

			var e = new PlayerWonLevelEventArgs { TotalLevelScore = model.Gold.Value };
			if (PlayerWonLevel != null) PlayerWonLevel(this, e); 
		}

		public void ExitLevel()
		{
			if (isLevelFinished) 
			{
				GlobalModel.Gold.Value += model.Gold.Value;
				GlobalModel.Corn.Value += cornForCompletedLevel;

				GlobalModel.Save();
			}

			Application.LoadLevel("main_screen");
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
	}
}
