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

		protected GameModel GlobalModel
		{
			get { return GameModel.Instance; }
		}

		public void LoadStartScene()
		{
			LoadScene(Scenes.Start);
		}

		public void LoadDashboardScene()
		{
			LoadScene(Scenes.Dashboard);
		}

		public void LoadOptionsScene()
		{
			LoadScene(Scenes.Options);
		}

		public void LoadScene(string sceneName)
		{
			Application.LoadLevel(sceneName);
		}
	}
}
