using UnityEngine;

namespace RuzikOdyssey.Level
{
	public class PickUpsSpawnController : ExtendedMonoBehaviour
	{
		public GameObject healthPickUp;
		public GameObject secondaryWeaponPickUp;
		
		public int spawnInterval = 60;
		public int spawnDelay = 15;
		
		private void Start ()
		{
			InvokeRepeating("Spawn", spawnDelay, 1);
			healthPickUp.rigidbody2D.velocity = Environment.ForegroundSpeed;
			secondaryWeaponPickUp.rigidbody2D.velocity = Environment.ForegroundSpeed;
		}
		
		private void Spawn()
		{
			var dice = Random.Range(0, spawnInterval);

			if (dice > 1) return;

			var pickUpTypeDice = Random.Range(0, 10);

			if (pickUpTypeDice < 5) InstantiateWithinWarzone(healthPickUp);
			else InstantiateWithinWarzone(secondaryWeaponPickUp);
		}

		private void InstantiateWithinWarzone(GameObject gameObject)
		{
			Instantiate(
				gameObject, 
				new Vector2(Game.WarzoneBounds.Right() + gameObject.RendererSize().x / 2,
			            	Random.Range(Game.WarzoneBounds.Top() - gameObject.RendererSize().y / 2,
			       				   	     Game.WarzoneBounds.Bottom() + gameObject.RendererSize().y / 2
			       				  		)
			            	),
				transform.rotation);
		}
	}
}

