using UnityEngine;

namespace RuzikOdyssey.Level
{
	public class GameHelper : MonoBehaviour
	{
		private ScoreController scoreController;

		public static GameHelper Instance { get; private set; }
		public Bounds WarzoneBounds { get; set; }

		private void Awake()
		{
			if (Instance != null) throw new UnityException("Multiple game controllers detected");

			var scoreGameObject = GameObject.Find("Score");
			if (scoreGameObject == null) throw new UnityException("Failed to find a game object named Score");
			
			scoreController = scoreGameObject.GetComponent<ScoreController>();
			if (scoreController == null) throw new UnityException("Failed to get ScoreController component from game object Score");

			var warzoneBoundary = GameObject.Find("WarzoneBoundary");
			if (warzoneBoundary == null) throw new UnityException("Failed to find game object WarzoneBoundary"); 
			
			this.WarzoneBounds = warzoneBoundary.collider2D.bounds;

			Instance = this;
		}


		public void AddScore(int addedScore)
		{
			scoreController.AddScore(addedScore);
		}
	}
}
