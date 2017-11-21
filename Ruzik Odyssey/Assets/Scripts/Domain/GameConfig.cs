using System.IO;

namespace RuzikOdyssey.Domain
{
	public static class GameConfig
	{
		public const string GameContentFilePath = "LevelsContent";

        public const string ServerBaseUrl = "https://ruzik-odyssey-api.azurewebsites.net";

		public static string GameContentUrl
		{
			get { return Path.Combine(ServerBaseUrl, "LevelsContent.txt"); }
		}
	}
}