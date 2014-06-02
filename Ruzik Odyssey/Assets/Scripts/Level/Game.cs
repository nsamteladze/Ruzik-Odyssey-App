using UnityEngine;

namespace RuzikOdyssey.Level
{
	public static class Game
	{
		private static ScoreController scoreController;

		static Game()
		{
			var scoreGameObject = GameObject.Find("Score");
			if (scoreGameObject == null) throw new UnityException("Failed to find a game object named Score");

			scoreController = scoreGameObject.GetComponent<ScoreController>();
			if (scoreController == null) throw new UnityException("Failed to get ScoreController component from game object Score");
		}

		public static void AddScore(int addedScore)
		{
			scoreController.AddScore(addedScore);
		}
	}
}
