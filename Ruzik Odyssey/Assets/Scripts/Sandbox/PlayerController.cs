using UnityEngine;
using System.Collections.Generic;
using RuzikOdyssey.Level;
using RuzikOdyssey;
using System;
using System.Linq;

namespace Sandbox.RuzikOdyssey.Player
{

	public class PlayerController : MonoBehaviour
	{
		private PlayerModel model; 
		private IMovementStrategy movementStrategy;

		private void Awake()
		{
			model = gameObject.GetComponentOrThrow<PlayerModel>();
			movementStrategy = new DeltaWithLimitedForceMovementStrategy(GetCurrentPosition());
		}

		private void RegisterEvents()
		{
			EventBroker.Subscribe<InputChangedEventArgs>(Events.Input.InputChanged, UserInput_InputChanged);
			EventBroker.Subscribe<EventArgs>(Events.Input.NoInput, UserInput_NoInput);
		}

		private void UserInput_InputChanged(object sender, InputChangedEventArgs e)
		{
			movementStrategy.Move(GetCurrentPosition(), e.Position, e.Delta);
		}

		private void UserInput_NoInput(object sender, EventArgs e)
		{
			movementStrategy.Stop();
		}

		private void FixedUpdate()
		{
			var force = movementStrategy.GetAppliedForce(GetCurrentPosition());

			rigidbody2D.velocity = Vector2.zero;
			rigidbody2D.AddForce(model.AccelerationMultiplier * force);
		}

		private Vector2 GetCurrentPosition()
		{
			return (Vector2)Camera.main.WorldToScreenPoint(transform.position);
		}
	}
	
}