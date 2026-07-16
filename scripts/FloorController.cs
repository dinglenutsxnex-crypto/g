using Godot;

public partial class FloorController : Node
{
	private static FloorController _instance;

	[Export] private Godot.Collections.Array<Node3D> _floor;

	public static FloorController Instance
	{
		get { return _instance; }
	}

	public static void SetShadows(bool triggerShadow)
	{
		if (_instance != null)
		{
			foreach (Node3D floorNode in _instance._floor)
			{
				// Godot layer management differs; set shadow casting mode instead
				floorNode.CastShadow = triggerShadow ? ShadowCasting.On : ShadowCasting.Off;
			}
		}
	}

	public bool IsFloor(Node3D go)
	{
		foreach (Node3D floorNode in _floor)
		{
			if (floorNode.Equals(go))
				return true;
		}
		return false;
	}

	public override void _Ready()
	{
		_instance = this;
		SetShadows(true);
	}
}
