using UnityEngine;
using System.Collections;
using System;

namespace RuzikOdyssey.Common
{
	public class EssenceController : MonoBehaviour
	{
		public float amount;
		public string gameObjectName;
		
		private float initialAmount;
		private BarController barController;
		
		private void Awake()
		{
			var bar = GameObjectExtensions.FindOrThrow(gameObjectName);
			barController = bar.GetComponentOrThrow<BarController>();
			
			initialAmount = amount;
		}
		
		public float Change(float delta)
		{
			Log.Debug("Changing amount {0} by {1}", amount, delta);
			
			amount += delta;
			
			if (amount > initialAmount) amount = initialAmount;
			if (amount < 0) amount = 0;
			
			Log.Debug("Updating bar with value {0}", amount);
			
			UpdateBar(amount);

			return amount;
		}
		
		private void UpdateBar(float currentAmount)
		{
			int level = (int)(100 * currentAmount / initialAmount);
			
			Log.Debug("Showing level {0} for value {1}", level, currentAmount);
			
			barController.ShowLevel(level);
		}
	}
}

