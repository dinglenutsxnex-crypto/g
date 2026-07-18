using System;
using Godot;

public partial class TutorialMaskGenerator : Node
{
	[Export]
	public Color maskColor = Colors.Black;

	[Export]
	public float maskAlpha = 0.7f;

	public override void _Ready()
	{
		GD.Print("STUB: TutorialMaskGenerator._Ready");
	}

	public void ShowMask(Rect2 area)
	{
		GD.Print("STUB: TutorialMaskGenerator.ShowMask");
	}

	public void HideMask()
	{
		GD.Print("STUB: TutorialMaskGenerator.HideMask");
	}

	public void SetHolePosition(Vector2 position)
	{
		GD.Print("STUB: TutorialMaskGenerator.SetHolePosition");
	}

	public void SetHoleSize(Vector2 size)
	{
		GD.Print("STUB: TutorialMaskGenerator.SetHoleSize");
	}
}
