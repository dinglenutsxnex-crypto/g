// STUB: needs UI rebuild — original 567 loc Unity MonoBehaviour with protobuf / Network / YAML dependencies
using Godot;
using System;
using System.Collections.Generic;

namespace SF3
{
	public partial class BattlesManager : Node
	{
		public static BattlesManager instance { get; private set; }
		public static BattleType currentBattleType { get; private set; }

		[Export]
		public Texture2D battleIconTexture;

		[Export]
		public PackedScene battleInfoScene;

		public static event Action OnBattlesUpdate;

		public override void _Ready()
		{
			instance = this;
		}
	}
}
