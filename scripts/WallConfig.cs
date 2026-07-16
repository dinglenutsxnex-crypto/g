using System.Linq;
using Godot;

public partial class WallConfig : Node
{
	public enum EWallType
	{
		NONE = 0,
		STICKY = 1,
		RECOIL = 2
	}

	[Export]
	public CollisionObject3D[] stickyWalls;

	[Export]
	public CollisionObject3D[] recoilWalls;

	public static WallConfig Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}

	public EWallType GetWallType(CollisionObject3D colliderToCheck)
	{
		if (stickyWalls.Any(c => c == colliderToCheck))
			return EWallType.STICKY;
		if (recoilWalls.Any(c => c == colliderToCheck))
			return EWallType.RECOIL;
		return EWallType.NONE;
	}
}
