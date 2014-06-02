using UnityEngine;
using System.Collections;

namespace RuzikOdyssey.Level
{

	public class WarzoneBoundaryController : MonoBehaviour 
	{
		private void OnTriggerEnter2D(Collider2D otherCollider)
		{
			if (!otherCollider.tag.Equals("Enemy")) return;
			otherCollider.SendMessage("OnEnterWarzone");
		}

		private void OnTriggerExit2D(Collider2D otherCollider)
		{
			if (!otherCollider.tag.Equals("Enemy")) return;
			otherCollider.SendMessage("OnExitWarzone");
		}
	}
}
