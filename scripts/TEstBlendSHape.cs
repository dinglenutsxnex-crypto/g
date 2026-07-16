using Godot;

public partial class TEstBlendSHape : Node
{
	[Export] public SkinnedMeshInstance3D skinnedMeshRenderer;
	[Export] public bool playbl;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		if (playbl)
		{
			playbl = false;
		}
	}
}