// STUB: needs UI rebuild — original 816 loc Unity MonoBehaviour with heavy Camera/NGUI usage
using Godot;
using System;

namespace SF3
{
	public class BattleCamera : Node
	{
		[Export]
		public float moveSmoothTime = 0.1f;

		[Export]
		public float rotatioSmoothTime = 0.1f;

		[Export]
		public float FOV = 60f;

		[Export]
		public float _maxDistance = 500f;

		[Export]
		public float moveTime = 1f;

		[Export]
		public Vector3 _defaultPosition = new Vector3(0f, 155f, -950f);

		public static BattleCamera Instance { get; private set; }

		public override void _Ready()
		{
			Instance = this;
		}
	}
}
