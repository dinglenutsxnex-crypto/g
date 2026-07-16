using Godot;
namespace SF3
{
	public partial class DebugInfoHider : UIModuleHolder
	{
		public enum EDebugButton
		{
			none = 1,
			playersStats = 2,
			leftTouch = 3,
			rightTouch = 4,
			enemyAI = 5
		}
		[Export]
		private Button _btPlayersStats;
		public bool playersStatsShow;
		private Node playerStats;
		[Export]
		private Button _btEnemyStats;
		public bool enemyStatsShow;
		private Node enemyStats;
		private bool _playerStatsActive;
		private bool _enemyStatsActive;
		[Export]
		private Button _btSceneDebug;
		private Node sceneDebug;
		public bool sceneDebugShow;
		private bool _sceneDebugActive;
		public static Node setSceneDebugObj
		{
			set
			{
				Instance.sceneDebug = value;
			}
		}
		public static DebugInfoHider Instance { get; private set; }
		public static Node setPlayerStatsObj
		{
			set
			{
				Instance.playerStats = value;
			}
		}
		public static Node setEnemyStatsObj
		{
			set
			{
				Instance.enemyStats = value;
			}
		}
		public bool _collisionCapsulesActive { get; private set; }
		public override void _Ready()
		{
			base.Awake();
			Instance = this;
			_collisionCapsulesActive = false;
			_playerStatsActive = false;
			_enemyStatsActive = false;
			_collisionCapsulesActive = false;
			_sceneDebugActive = false;
			InitButtons();
		}
		private void InitButtons()
		{
			if (_btPlayersStats != null)
			{
				_btPlayersStats.onClick.Add(new EventDelegate(OnPlayersStatsClick));
				if (playersStatsShow)
				{
					OnPlayersStatsClick();
				}
			}
			if (_btEnemyStats != null)
			{
				_btEnemyStats.onClick.Add(new EventDelegate(ShowHideEnemyStats));
				if (enemyStatsShow)
				{
					ShowHideEnemyStats();
				}
			}
			if (_btSceneDebug != null)
			{
				_btSceneDebug.onClick.Add(new EventDelegate(OnSceneDebugClick));
				if (sceneDebugShow)
				{
					OnSceneDebugClick();
				}
			}
		}

		private void ShowHideEnemyStats()
		{
			if (!(enemyStats == null) && enemyStats != null)
			{
				_enemyStatsActive = !_enemyStatsActive;
				enemyStats.SendMessage("ShowHide");
			}
		}
		private void OnPlayersStatsClick()
		{
			if (!(playerStats == null))
			{
				_playerStatsActive = !_playerStatsActive;
				playerStats.SendMessage("ShowHide");
			}
		}
		private void OnSceneDebugClick()
		{
			VisualDebugUI.Open();
			if (!(sceneDebug == null))
			{
				_sceneDebugActive = !_sceneDebugActive;
				sceneDebug.SendMessage("ShowHide");
			}
		}
	}
}

