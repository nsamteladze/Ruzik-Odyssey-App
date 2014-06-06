using UnityEngine;

namespace RuzikOdyssey.Level
{
	public class GameController : MonoBehaviour
	{
		private void Awake()
		{
			if (Instance != null) throw new UnityException("Multiple game controllers detected");
			Instance = this;
		}
		
		public static GameController Instance { get; private set; }

		private ScoreController scoreController;

		private void Start()
		{
			var scoreGameObject = GameObject.Find("Score");
			if (scoreGameObject == null) throw new UnityException("Failed to find a game object named Score");

			scoreController = scoreGameObject.GetComponent<ScoreController>();
			if (scoreController == null) throw new UnityException("Failed to get ScoreController component from game object Score");
		}

		public void AddScore(int addedScore)
		{
			scoreController.AddScore(addedScore);
		}
	}
}
