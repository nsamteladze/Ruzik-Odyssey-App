using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Level
{
	public class LevelEnemiesDesign : MonoBehaviour
	{
		public GameObject scout;
		public GameObject worker;
		public GameObject soldier;
		public GameObject interceptor;
		public GameObject destroyer;

		private int nextGroupIndex = 0;
		private List<GameObjectsGroupDesign> levelDesign;

		private void Awake()
		{
			LoadLevel();
		}

		public GameObjectsGroupDesign GetNext()
		{
			Log.Debug("GetNext is called. nextGroupIndex = {0}", nextGroupIndex);

			if (nextGroupIndex >= levelDesign.Count) return null;
			
			var group = levelDesign[nextGroupIndex];
			nextGroupIndex++;

			if (nextGroupIndex == levelDesign.Count) 
			{
				Log.Debug("nextGroupIndex = {0}, levelDesign.Count ={1}. OnLastEnemyWaveReturned", nextGroupIndex, levelDesign.Count);

				OnLastEnemyWaveReturned();
			}
			
			return group;
		}

		public event EventHandler<EventArgs> LastEnemyWaveReturned;

		protected virtual void OnLastEnemyWaveReturned()
		{
			if (LastEnemyWaveReturned != null) LastEnemyWaveReturned(this, EventArgs.Empty);
		}

		private void LoadLevel()
		{
			levelDesign = new List<GameObjectsGroupDesign>
			{
				// Wave 1
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						scout
					},
					NextGroupInterval = 5.0f,
				},
				// Wave 2
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						scout, scout
					},
					NextGroupInterval = 7.0f,
				},
				// Wave 3
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						scout, scout, scout, scout
					},
					NextGroupInterval = 8.0f,
				},
				// Wave 4
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						scout, scout, scout
					},
					NextGroupInterval = 10.0f,
				},
				// Wave 5
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						scout, scout, scout, scout, scout, worker
					},
					NextGroupInterval = 10.0f,
				},
				// Wave 6
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						scout, scout, scout, scout, worker, worker
					},
					NextGroupInterval = 10.0f,
				},
				// Wave 7
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						soldier, worker, worker, worker, worker
					},
					NextGroupInterval = 10.0f,
				},
#if !UNITY_EDITOR

				// Wave 8
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						scout, scout, scout, scout, scout, scout, worker, worker, worker, worker
					},
					NextGroupInterval = 15.0f,
				},
				// Wave 9
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						scout, scout, scout, scout, scout, scout, worker, worker, worker, worker, soldier, soldier
					},
					NextGroupInterval = 20.0f,
				},
				// Wave 10
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						worker, worker, worker, worker, soldier, soldier, soldier, soldier
					},
					NextGroupInterval = 15.0f,
				},
				// Wave 11
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						worker, worker, worker, worker, soldier, soldier, soldier, soldier, scout, scout
					},
					NextGroupInterval = 20.0f,
				},
				// Wave 12
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						worker, worker, worker, worker, soldier, soldier, soldier, soldier, scout, scout, scout, scout
					},
					NextGroupInterval = 20.0f,
				},
				// Wave 13
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						worker, worker, worker, worker, soldier, soldier, soldier, soldier, interceptor
					},
					NextGroupInterval = 25.0f,
				},
				// Wave 14
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						worker, worker, worker, worker, soldier, soldier, soldier, soldier, interceptor
					},
					NextGroupInterval = 25.0f,
				},
				// Wave 15
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						worker, worker, worker, soldier, soldier, soldier, soldier, interceptor, destroyer
					},
					NextGroupInterval = 20.0f,
				},

#endif

				// Level end
				GameObjectsGroupDesign.WaitForSeconds(1),
			};
		}
	}


}
