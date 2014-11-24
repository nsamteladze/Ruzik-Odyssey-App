using UnityEngine;
using RuzikOdyssey.Common;
using RuzikOdyssey.Common.UI;

namespace RuzikOdyssey.Level
{
	public class GameHelper : MonoBehaviour
	{
		public static GameHelper Instance { get; private set; }
		public Bounds WarzoneBounds { get; set; }

		private void Awake()
		{
			if (Instance != null) throw new UnityException("Multiple game controllers detected");

			var warzoneBoundary = GameObjectExtensions.FindOrThrow("WarzoneBoundary");

			WarzoneBounds = warzoneBoundary.collider2D.bounds;

			Instance = this;
		}
	}
}
