using UnityEngine;
using RuzikOdyssey.Common;
using RuzikOdyssey.Common.UI;

namespace RuzikOdyssey.Level
{
	public class GameHelper : MonoBehaviour
	{
		private ScoreController scoreController;
		private Label missileCouterLabel;

		public static GameHelper Instance { get; private set; }
		public Bounds WarzoneBounds { get; set; }

		private void Awake()
		{
			if (Instance != null) throw new UnityException("Multiple game controllers detected");

			var scoreGameObject = GameObjectExtensions.FindOrThrow("Score");
			scoreController = scoreGameObject.GetComponentOrThrow<ScoreController>();
			var warzoneBoundary = GameObjectExtensions.FindOrThrow("WarzoneBoundary");

			var missileCounter = GameObjectExtensions.FindOrThrow("MissileCounter");
			missileCouterLabel = missileCounter.GetComponentInChildrenOrThrow<Label>();

			WarzoneBounds = warzoneBoundary.collider2D.bounds;

			Instance = this;
		}


		public void AddScore(int addedScore)
		{
			scoreController.AddScore(addedScore);
		}

		public void DisplayMissileAmmo(int ammo)
		{
			missileCouterLabel.Text = ammo.ToString();
		}
	}
}
