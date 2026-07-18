using Godot;

namespace SF3
{
	/// <summary>
	/// Battle-scene camera controller. Stub — add actual camera logic when porting.
	/// </summary>
	public partial class BattleCamera3D : Node3D
	{
		private static BattleCamera3D _instance;
		public static BattleCamera3D Instance => _instance;

		[Export] public float DefaultFov = 60f;
		[Export] public Vector3 DefaultOffset = Vector3.Zero;

		public override void _Ready()
		{
			_instance = this;
		}

		public void MoveToDojo(Node3D anchor, float duration = 0.5f)
		{
			GD.Print("BattleCamera3D.MoveToDojo");
		}

		public void MoveToObject(Node3D target, Vector3 offset = default, float duration = 0.5f)
		{
			GD.Print($"BattleCamera3D.MoveToObject: {target?.Name}");
		}

		public void ActivateInventoryCamera3D(Vector3 offsetFromChar)
		{
			GD.Print("BattleCamera3D.ActivateInventoryCamera3D");
		}
	}
}
