using System;
using System.Collections.Generic;
using Godot;

public partial class DebugListContainerController : Control
{
	[Export]
	private Label titleLabel;

	[Export]
	private GridContainer linesGrid;

	[Export]
	private PackedScene debugLinePrefab;

	[Export]
	private PackedScene debugButtonPrefab;

	public void setup(string title)
	{
		titleLabel.Text = title;
	}

	public void setVisible(bool visible)
	{
		Modulate = new Color(Modulate, visible ? 1f : 0f);
	}

	public override void _Ready()
	{
	}

	public void UpdateLines()
	{
		foreach (Node child in linesGrid.GetChildren())
		{
			if (child is DebugLineController component)
			{
				component.onUpdate();
			}
		}
	}

	public DebugTextLineController AddLine(string name, Action<DebugTextLineController> onUpdate)
	{
		DebugTextLineController instance = debugLinePrefab.Instantiate<DebugTextLineController>();
		linesGrid.AddChild(instance);
		instance.setup(name, onUpdate);
		return instance;
	}

	public DebugGOLineController AddLine(PackedScene prefab, Action<DebugGOLineController> onUpdate)
	{
		DebugGOLineController instance = prefab.Instantiate<DebugGOLineController>();
		linesGrid.AddChild(instance);
		instance.setup(onUpdate);
		return instance;
	}

	public DebugButtonLineController AddButton(string text, Action onClick)
	{
		DebugButtonLineController instance = debugButtonPrefab.Instantiate<DebugButtonLineController>();
		linesGrid.AddChild(instance);
		instance.setup(text, onClick);
		return instance;
	}
}
