using System;
using Godot;

public partial class NekkiUIDragScaleWidget : Node
{
	[Export]
	public Control targetControl;

	[Export]
	public bool enableDrag = true;

	[Export]
	public bool enableScale = true;

	public override void _Ready()
	{
		GD.Print("STUB: NekkiUIDragScaleWidget._Ready");
	}

	public void SetTarget(Control control)
	{
		targetControl = control;
	}
}
