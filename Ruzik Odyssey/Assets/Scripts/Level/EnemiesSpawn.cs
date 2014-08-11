using UnityEngine;
using System.Collections;
using RuzikOdyssey.Common;
using RuzikOdyssey.LevelDesign;

public class EnemiesSpawn : ExtendedMonoBehaviour
{
	public GameObject[] enemies;		// Array of enemy prefabs.

	private float spawningInterval = 4f;
	private int wavesCounter = 0;
	private int enemiesLevel = 1;

	private LevelDesign levelDesign; 

	private void Awake()
	{
		levelDesign = this.gameObject.GetComponentOrThrow<LevelDesign>();
	}

	void Start ()
	{
		// InvokeRepeating("Spawn", 10.0f, spawningInterval);

		SpawnFromLevelDesign();
	}

	private void SpawnFromLevelDesign()
	{
		var enemyPack = levelDesign.GetNext();

		InstantiateEnemyPackDesign(enemyPack);

		Invoke("SpawnFromLevelDesign", enemyPack.NextPackAppearance);
	}

	private void InstantiateEnemyPackDesign(EnemyPackDesign design)
	{
		foreach (var enemy in design.Enemies)
		{
			var position = new Vector2(transform.position.x + Random.Range(3, 20), 
			                           Random.Range(Game.WarzoneBounds.Bottom() + enemy.RendererSize().y / 2, 
			             							Game.WarzoneBounds.Top() - enemy.RendererSize().y / 2));
			Instantiate(enemy, position, transform.rotation);
		}
	}

	void Spawn()
	{
		if (Environment.IsGameOver) return;

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
		Instantiate(enemies[Random.Range(0, enemies.Length)], 
		            new Vector2(transform.position.x + 1, Random.Range(-3.8f, 3.5f)), transform.rotation);
	}

	void SpawnPair()
	{
		Instantiate(enemies[Random.Range(0, enemies.Length)], 
		            new Vector2(transform.position.x + 1, Random.Range(-3.8f, 0)), transform.rotation);
		Instantiate(enemies[Random.Range(0, enemies.Length)], 
		            new Vector2(transform.position.x + 3, Random.Range(-0, 3.5f)), transform.rotation);
	}

	void SpawnDiagonalPack()
	{
		Instantiate(enemies[Random.Range(0, enemies.Length)], 
		            new Vector2(transform.position.x + 1, -3f), transform.rotation);
		Instantiate(enemies[Random.Range(0, enemies.Length)], 
		            new Vector2(transform.position.x + 2.5f, 0f), transform.rotation);
		Instantiate(enemies[Random.Range(0, enemies.Length)], 
		            new Vector2(transform.position.x + 4, 3f), transform.rotation);
	}

	void SpawnFour()
	{
		Instantiate(enemies[Random.Range(0, enemies.Length)], 
		            new Vector2(transform.position.x + 3, -2.5f), transform.rotation);
		Instantiate(enemies[Random.Range(0, enemies.Length)], 
		            new Vector2(transform.position.x + 3, 2.5f), transform.rotation);
		Instantiate(enemies[Random.Range(0, enemies.Length)], 
		            new Vector2(transform.position.x + 1, 0), transform.rotation);
		Instantiate(enemies[Random.Range(0, enemies.Length)], 
		            new Vector2(transform.position.x + 5, 0), transform.rotation);
	}
}
