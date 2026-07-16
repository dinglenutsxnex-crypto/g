using Godot;
using System;

namespace SF3.BattleUtils
{
	public class SF3BattleUtils : Node
	{
		private static SF3BattleUtils _instance;
		private SF3FPSUtil _fpsUtil;

		public static SF3BattleUtils instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new SF3BattleUtils();
					_instance.Name = "battle_utils";
				}
				return _instance;
			}
		}

		public static void Initialize()
		{
			SF3BattleUtils sF3BattleUtils = instance;
		}

		public override void _Ready()
		{
			_fpsUtil = new SF3FPSUtil();
		}

		public override void _Process(double delta)
		{
			_fpsUtil.Update();
		}

		public static float GetFPS()
		{
			return instance._fpsUtil.GetFPS();
		}

		public static int GetPing()
		{
			return NetworkConnection.current.GetPing();
		}
	}
}
