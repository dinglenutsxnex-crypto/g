using System;
using Godot;

public partial class ModelShadow : Node
{
	[Export]
	public bool castShadow = true;

	[Export]
	public bool receiveShadow = true;

	public override void _Ready()
	{
		GD.Print("STUB: ModelShadow._Ready");
	}

	public void SetCastShadow(bool enabled)
	{
		castShadow = enabled;
	}

	public void SetReceiveShadow(bool enabled)
	{
		receiveShadow = enabled;
	}
}
