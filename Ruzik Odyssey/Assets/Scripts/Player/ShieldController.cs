using UnityEngine;
using System.Collections;
using System;

namespace RuzikOdyssey.Player
{
	public class ShieldController : MonoBehaviour
	{
		public float energy = 10.0f;

		private float defaultEnergy;
		private BarController energyBarController;
		private GameObject ui;

		private void Start()
		{
			ui = GameObject.Find("UI");
			if (ui == null) throw new Exception("Failed to find game object named 'ui' in the hierarchy");

			var energyBar = GameObject.Find("EnergyBar");
			if (energyBar == null) throw new UnityException("Failed to find energy bar in the hierarchy");

			energyBarController = energyBar.GetComponent<BarController>();
			if (energyBarController == null) throw new UnityException("Failed to find a energy bar controller");

			defaultEnergy = energy;
		}

		public float ShieldDamage(float damage)
		{
			if (energy <= 0) return damage;

			var energyCost = damage;
			var shieldedDamage = damage / 2;

			energy -= energyCost;

			var energyLevel = (int)(100 * energy / defaultEnergy);
			energyBarController.ShowLevel(energyLevel);

			return (damage - shieldedDamage);
		}
	}
}