using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RuzikOdyssey.Common;
using System.Linq;
using System;
using RuzikOdyssey.Ai;
using RuzikOdyssey.Enemies;
using Random = UnityEngine.Random;

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

	public sealed class LevelViewModel : ExtendedMonoBehaviour
	{
		public GameObject[] obstaclesAndItemsSource;

		public float enemiesSpawningDelay = 1.0f;

		public int cornForCompletedLevel = 2;

		private LevelEnemiesDesign enemiesLevelDesign; 
		private ObstaclesAndItemsLevelDesign obstaclesItemsLevelDesign;
		private bool isFinalEnemiesWave = false;
		private bool isLevelFinished = false;
		private int finalEnemiesCouter = 0;

		private LevelModel model;

		private void Awake()
		{
			model = new LevelModel();

			obstaclesItemsLevelDesign = new ObstaclesAndItemsLevelDesign(obstaclesAndItemsSource.ToList());

			enemiesLevelDesign = this.gameObject.GetComponentOrThrow<LevelEnemiesDesign>();

			enemiesLevelDesign.LastEnemyWaveReturned += LevelDesign_LastEnemyWaveReturned;

			PublishEvents();
		}

		private void OnApplicationQuit()
		{
			Log.Debug("OnApplicationQuit");

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

		private void Start ()
		{
			StartSpawningLevelFromDesign();

			StartCoroutine(SpawnLevelDesign(obstaclesItemsLevelDesign));
		}

		private void StartSpawningLevelFromDesign()
		{
			Invoke("SpawnNextEnemyFromLevelDesign", enemiesSpawningDelay);
		}

		private void SpawnNextEnemyFromLevelDesign()
		{
			if (isFinalEnemiesWave) return;

			var enemyPack = enemiesLevelDesign.GetNext();
			
			InstantiateGroupDesign(enemyPack)
				.ForEach(item =>
          		{
					var alienController = item.GetComponentOrThrow<AlienController>();
					alienController.Died += Enemy_Died;
					alienController.Destroyed += Enemy_Destroyed;
				});
			
			Invoke("SpawnNextEnemyFromLevelDesign", enemyPack.NextGroupInterval);
		}

		private IEnumerator SpawnLevelDesign(ObstaclesAndItemsLevelDesign levelDesign)
		{
			var groupDesign = levelDesign.GetNextGroup();

			while (!isLevelFinished && groupDesign != null)
			{
				InstantiateGroupDesign(groupDesign);

				yield return new WaitForSeconds(groupDesign.NextGroupInterval);

				groupDesign = levelDesign.GetNextGroup();
			}
		}

		[Obsolete]
		private void InstantiateEnemiesGroupDesign(GameObjectsGroupDesign design)
		{
			foreach (var enemy in design.Objects)
			{
				var position = new Vector2(Game.WarzoneBounds.Right() + Random.Range(3, 20), 
				                           Random.Range(Game.WarzoneBounds.Bottom() + enemy.RendererSize().y / 2, 
				             							Game.WarzoneBounds.Top() - enemy.RendererSize().y / 2));

				var instance = Instantiate(enemy, position, transform.rotation) as GameObject;

				var alienController = instance.GetComponentOrThrow<AlienController>();

				alienController.Died += Enemy_Died;
			}
		}

		private List<GameObject> InstantiateGroupDesign(GameObjectsGroupDesign design)
		{
			return design.Objects
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
