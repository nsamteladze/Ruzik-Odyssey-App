using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
	public int score = 0;					// The player's score.

	void Update ()
	{
		// Set the score text.
		guiText.text = "Score: " + score;

	}

}
