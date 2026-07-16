using Godot;

namespace SF3.Settings
{
	public partial class GameSettings : Node
	{
		private ItemSettings _itemSettings;
		private DojoSettings _dojoSettings;
		private ClientSettings _clientSettings;

		public static GameSettings Instance { get; private set; }

		public static ItemSettings ItemSettings
		{
			get { return Instance._itemSettings; }
		}

		public static DojoSettings DojoSettings
		{
			get { return Instance._dojoSettings; }
		}

		public static ClientSettings clientSettings
		{
			get { return Instance._clientSettings; }
		}

		public static void Initialize()
		{
			if (Instance != null)
			{
				return;
			}
			Instance = new GameSettings();
			Instance.Name = "game_settings";
			StaticObjectsManager.AddObject(Instance);
			Instance._itemSettings = GlobalLoad.GetLoadObjectInternal<ItemSettings>("GameSettings", "item_settings");
			Instance._dojoSettings = GlobalLoad.GetLoadObjectInternal<DojoSettings>("GameSettings", "dojo_settings");
			Instance._clientSettings = GlobalLoad.GetLoadObjectInternal<ClientSettings>("GameSettings", "clientSettings");
		}
	}
}
