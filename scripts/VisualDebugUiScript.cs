using Godot;
using System.Collections.Generic;

public partial class VisualDebugUiScript : Node
{
	private static List<VisualDebugUiScript> _units = new List<VisualDebugUiScript>();

	[Export] public Button Activate;
	[Export] public Control CheckMark;
	[Export] public Label Label;

	private Node _behaviour;

	public static void Clear()
	{
		foreach (var unit in _units)
			unit.QueueFree();
		_units.Clear();
	}

	public void SetSctipt(Node behaviour)
	{
		_units.Add(this);
		_behaviour = behaviour;
		CheckMark.Visible = true;
		Activate.Pressed += OnActivate;
		Label.Text = behaviour.GetType().ToString();
		Visible = true;
	}

	private void OnActivate()
	{
		CheckMark.Visible = !CheckMark.Visible;
	}
}