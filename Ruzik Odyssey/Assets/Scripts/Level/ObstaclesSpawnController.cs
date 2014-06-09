using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RuzikOdyssey.Level
{
	public class ObstaclesSpawnController : ExtendedMonoBehaviour
	{
		public GameObject heron;
		public GameObject mine;
		public GameObject smallForceField;
		public GameObject largeForceField;
		
		public float spawnInterval = 4f;

		private ICollection<ObstacleGroup> obstacleGroups;

		private void Start ()
		{
			obstacleGroups = new List<ObstacleGroup>()
			{
				new ObstacleGroup(heron, Game.WarzoneBounds),
				new ObstacleGroup(mine, Game.WarzoneBounds),
				new ObstacleGroup(smallForceField, Game.WarzoneBounds),
				new ObstacleGroup(largeForceField, Game.WarzoneBounds)
			};

			InvokeRepeating("Spawn", 0.0f, spawnInterval);
		}
		
		private void Spawn()
		{
			var index = Random.Range(0, obstacleGroups.Count);
			var obstacleGroup = obstacleGroups.ElementAt(index);

			foreach (var obstacle in obstacleGroup.Obstacles)
			{
				Instantiate(
					obstacle.ObstacleObject, 
		            new Vector2(Game.WarzoneBounds.Right() + obstacle.Size().x / 2, 
				            obstacle.PositionRange.GetNumberInRange()), 
					transform.rotation);
			}
		}
	}

	public class Range
	{
		public float Min { get; set; }
		public float Max { get; set; }

		public float GetNumberInRange()
		{
			return Random.Range(Min, Max);
		}
	}

	public class ExtendedMonoBehaviour : MonoBehaviour
	{
		protected GameHelper Game
		{
			get { return GameHelper.Instance; }
		}
	}

	public static class CustomExtensions
	{
		public static Vector2 RendererSize(this GameObject gameObject)
		{
			return gameObject.renderer.bounds.size;
		}

		public static float Top(this Bounds bounds)
		{
			return bounds.max.y;
		}

		public static float Bottom(this Bounds bounds)
		{
			return bounds.min.y;
		}

		public static float Left(this Bounds bounds)
		{
			return bounds.min.x;
		}

		public static float Right(this Bounds bounds)
		{
			return bounds.max.x;
		}


	}

	internal class Obstacle
	{
		public GameObject ObstacleObject { get; set; }
		public Range PositionRange { get; set; }

		public Obstacle(GameObject obstacleObject, Bounds bounds)
		{
			this.ObstacleObject = obstacleObject;
			this.PositionRange = new Range()
			{
				Min = bounds.Bottom() + ObstacleObject.RendererSize().y / 2,
				Max = bounds.Top() - ObstacleObject.RendererSize().y / 2,
			};
		}

		public Vector2 Size()
		{
			return ObstacleObject.RendererSize();
		}
	}

	internal class ObstacleGroup
	{
		public ICollection<Obstacle> Obstacles { get; set; }

		public ObstacleGroup(GameObject gameObject1, Bounds bounds)
		{
			this.Obstacles = new List<Obstacle>()
			{
				new Obstacle(gameObject1, bounds)
			};
		}
	}
}
