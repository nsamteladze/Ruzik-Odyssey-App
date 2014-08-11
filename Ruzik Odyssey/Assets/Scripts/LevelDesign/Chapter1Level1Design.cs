using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace RuzikOdyssey.LevelDesign
{
	public class Chapter1Level1Design : LevelDesign
	{
		public GameObject bird;
		public GameObject scout;
		public GameObject worker;
		public GameObject soldier;
		public GameObject interceptor;
		public GameObject destroyer;

		private int counter = 0;
		private List<EnemyPackDesign> levelDesign;

		private int totalEnemyPacks;

		private void Awake()
		{
			LoadLevel();
			totalEnemyPacks = levelDesign.Count();
		}

		public override EnemyPackDesign GetNext()
		{
			var enemyPack = levelDesign[counter % totalEnemyPacks];
			counter++;

			return enemyPack;
		}

		private void LoadLevel()
		{
			levelDesign = new List<EnemyPackDesign>
			{
				// Wave 1
				new EnemyPackDesign
				{
					Enemies = new List<GameObject>
					{
						bird, bird
					},
					NextPackAppearance = 5.0f,
				},
				// Wave 2
				new EnemyPackDesign
				{
					Enemies = new List<GameObject>
					{
						bird, bird, bird
					},
					NextPackAppearance = 4.0f,
				},
				// Wave 3
				new EnemyPackDesign
				{
					Enemies = new List<GameObject>
					{
						bird, bird, bird, bird, scout
					},
					NextPackAppearance = 5.0f,
				},
				// Wave 4
				new EnemyPackDesign
				{
					Enemies = new List<GameObject>
					{
						bird, scout, scout
					},
					NextPackAppearance = 7.0f,
				},
				// Wave 5
				new EnemyPackDesign
				{
					Enemies = new List<GameObject>
					{
						scout, scout, scout, scout
					},
					NextPackAppearance = 8.0f,
				},
				// Wave 6
				new EnemyPackDesign
				{
					Enemies = new List<GameObject>
					{
						scout, scout, scout, bird, bird, bird
					},
					NextPackAppearance = 10.0f,
				},
				// Wave 7
				new EnemyPackDesign
				{
					Enemies = new List<GameObject>
					{
						scout, scout, scout, scout, scout, worker, bird, bird
					},
					NextPackAppearance = 10.0f,
				},
				// Wave 8
				new EnemyPackDesign
				{
					Enemies = new List<GameObject>
					{
						scout, scout, scout, scout, worker, worker, bird, bird
					},
					NextPackAppearance = 10.0f,
				},
				// Wave 9
				new EnemyPackDesign
				{
					Enemies = new List<GameObject>
					{
						soldier, worker, worker, worker, worker, bird, bird
					},
					NextPackAppearance = 10.0f,
				},
				// Wave 10
				new EnemyPackDesign
				{
					Enemies = new List<GameObject>
					{
						scout, scout, scout, scout, scout, scout, worker, worker, worker, worker
					},
					NextPackAppearance = 15.0f,
				},
				// Wave 11
				new EnemyPackDesign
				{
					Enemies = new List<GameObject>
					{
						scout, scout, scout, scout, scout, scout, worker, worker, worker, worker, soldier, soldier
					},
					NextPackAppearance = 20.0f,
				},
				// Wave 12
				new EnemyPackDesign
				{
					Enemies = new List<GameObject>
					{
						worker, worker, worker, worker, soldier, soldier, soldier, soldier, bird, bird
					},
					NextPackAppearance = 15.0f,
				},
				// Wave 13
				new EnemyPackDesign
				{
					Enemies = new List<GameObject>
					{
						worker, worker, worker, worker, soldier, soldier, soldier, soldier, scout, scout, bird, bird
					},
					NextPackAppearance = 20.0f,
				},
				// Wave 14
				new EnemyPackDesign
				{
					Enemies = new List<GameObject>
					{
						worker, worker, worker, worker, soldier, soldier, soldier, soldier, scout, scout, scout, scout
					},
					NextPackAppearance = 20.0f,
				},
				// Wave 15
				new EnemyPackDesign
				{
					Enemies = new List<GameObject>
					{
						worker, worker, worker, worker, soldier, soldier, soldier, soldier, interceptor
					},
					NextPackAppearance = 25.0f,
				},
				// Wave 16
				new EnemyPackDesign
				{
					Enemies = new List<GameObject>
					{
						worker, worker, worker, worker, soldier, soldier, soldier, soldier, interceptor
					},
					NextPackAppearance = 25.0f,
				},
				// Wave 17
				new EnemyPackDesign
				{
					Enemies = new List<GameObject>
					{
						worker, worker, worker, soldier, soldier, soldier, soldier, interceptor, destroyer
					},
					NextPackAppearance = 20.0f,
				},
			};
		}
	}

	public abstract class LevelDesign : MonoBehaviour
	{
		public abstract EnemyPackDesign GetNext();
	}

	public class EnemyPackDesign
	{
		public List<GameObject> Enemies { get; set; }
		public float NextPackAppearance { get; set; }
	}
}
