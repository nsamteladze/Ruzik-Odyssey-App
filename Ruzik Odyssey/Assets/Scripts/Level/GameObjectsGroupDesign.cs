using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Level
{

	public class GameObjectsGroupDesign
	{
		public List<GameObject> Objects { get; set; }
		public float NextGroupInterval { get; set; }

		public static GameObjectsGroupDesign WaitForSeconds(float nextGroupInterval)
		{
			return new GameObjectsGroupDesign
			{
				Objects = new List<GameObject>(),
				NextGroupInterval = nextGroupInterval
			};
		}
	}

	public class WaveTemplate
	{
		public IList<int> ObjectIndices { get; set; }
		public float NextWaveInterval { get; set; }

		public static WaveTemplate WaitForSeconds(float nextWaveInterval)
		{
			return new WaveTemplate
			{
				ObjectIndices = new List<int>(),
				NextWaveInterval = nextWaveInterval
			};
		}
	}
	
}
