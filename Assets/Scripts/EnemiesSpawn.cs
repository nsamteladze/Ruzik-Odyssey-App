using UnityEngine;
using System.Collections;

public class EnemiesSpawn : MonoBehaviour
{
	public GameObject[] enemies;		// Array of enemy prefabs.

	private float spawningInterval = 5f;
	private int wavesCounter = 0;
	private int enemiesLevel = 1;

	void Start ()
	{
		InvokeRepeating("Spawn", 5f, spawningInterval);
	}

	void Spawn()
	{
		int packIndex = Random.Range (0, enemiesLevel);

		switch (packIndex)
		{
			case 0:
				SpawnSingle();
				break;
			case 1:
				SpawnPair();
				break;
			case 2:
				SpawnDiagonalPack();
				break;
			case 3:
				SpawnFour();
				break;
		}

		wavesCounter++;
		spawningInterval -= 0.1f;
		if ((wavesCounter % 3 == 0) && (enemiesLevel <= 4)) enemiesLevel++;
	}

	void SpawnSingle()
	{
		Instantiate(enemies[0], new Vector2(transform.position.x + 1, Random.Range(-3.8f, 3.5f)), transform.rotation);
	}

	void SpawnPair()
	{
		Instantiate(enemies[0], new Vector2(transform.position.x + 1, Random.Range(-3.8f, 0)), transform.rotation);
		Instantiate(enemies[0], new Vector2(transform.position.x + 3, Random.Range(-0, 3.5f)), transform.rotation);
	}

	void SpawnDiagonalPack()
	{
		Instantiate(enemies[0], new Vector2(transform.position.x + 1, -3f), transform.rotation);
		Instantiate(enemies[0], new Vector2(transform.position.x + 2.5f, 0f), transform.rotation);
		Instantiate(enemies[0], new Vector2(transform.position.x + 4, 3f), transform.rotation);
	}

	void SpawnFour()
	{
		Instantiate(enemies[0], new Vector2(transform.position.x + 3, -2.5f), transform.rotation);
		Instantiate(enemies[0], new Vector2(transform.position.x + 3, 2.5f), transform.rotation);
		Instantiate(enemies[0], new Vector2(transform.position.x + 1, 0), transform.rotation);
		Instantiate(enemies[0], new Vector2(transform.position.x + 5, 0), transform.rotation);
	}
}
