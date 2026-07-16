using System;
using Godot;

public class TooltipManager : Node
{
	private static TooltipManager _instance;

	[Export]
	public Label tooltipLabel;

	[Export]
	public Node tooltipPanel;

	public static TooltipManager Instance
	{
		get { return _instance; }
	}

	public override void _Ready()
	{
		base._Ready();
		_instance = this;
		GD.Print("STUB: TooltipManager._Ready");
	}

	public void ShowTooltip(string text, Vector2 position)
	{
		GD.Print("STUB: TooltipManager.ShowTooltip: " + text);
		if (tooltipLabel != null) tooltipLabel.Text = text;
		if (tooltipPanel != null) tooltipPanel.Visible = true;
	}

	public void HideTooltip()
	{
		if (tooltipPanel != null) tooltipPanel.Visible = false;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_instance == this) _instance = null;
	}
}
