using UnityEngine;
using RuzikOdyssey.Level;

namespace RuzikOdyssey.Common
{
	public class ExtendedMonoBehaviour : MonoBehaviour
	{
		protected GameHelper Game
		{
			get { return GameHelper.Instance; }
		}


	}
}
