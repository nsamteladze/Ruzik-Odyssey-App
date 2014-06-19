using UnityEngine;
using System.Collections;
using System;
using RuzikOdyssey.Level;

namespace RuzikOdyssey.Player
{
	public class ShieldController : MonoBehaviour
	{
		public float energy = 10.0f;

		public GameObject shieldEffect;

		private float defaultEnergy;
		private BarController energyBarController;
		private GameObject ui;
		private Shield shieldEffectBehavior;

		public Vector2 shieldAdjustment = new Vector2(1.5f, 0);

		private void Start()
		{
			ui = GameObject.Find("UI");
			if (ui == null) throw new Exception("Failed to find game object named 'ui' in the hierarchy");

			var energyBar = GameObject.Find("EnergyBar");
			if (energyBar == null) throw new UnityException("Failed to find energy bar in the hierarchy");

			energyBarController = energyBar.GetComponent<BarController>();
			if (energyBarController == null) throw new UnityException("Failed to find a energy bar controller");

			var shieldEffectGameObject = (GameObject) Instantiate(shieldEffect, 
			                                                      gameObject.transform.position + (Vector3)shieldAdjustment, 
			                                                      transform.rotation);
			shieldEffectGameObject.transform.parent = gameObject.transform;
			shieldEffectBehavior = shieldEffectGameObject.GetComponent<Shield>();
			ChangeShieldVisibility(false);

			defaultEnergy = energy;
		}

		public float ShieldDamage(float damage)
		{
			if (energy <= 0) return damage;

			shieldEffectBehavior.ShowShield();

			var energyCost = damage;
			var shieldedDamage = damage / 2;

			energy -= energyCost;

			var energyLevel = (int)(100 * energy / defaultEnergy);
			energyBarController.ShowLevel(energyLevel);

			return (damage - shieldedDamage);
		}

		public void ChangeShieldVisibility(bool isVisible)
		{
			shieldEffectBehavior.gameObject.renderer.enabled = isVisible;
		}
	}
}