using System.Collections.Generic;
using Godot;

public partial class ObjectDebug : Node
{
	private class Unit
	{
		private bool expand;
		public Node3D transform;
		public List<Node> scripts = new List<Node>();
		public List<Unit> childs = new List<Unit>();

		private static readonly Dictionary<int, string> Offsets = new Dictionary<int, string>();

		public Unit(Node3D root)
		{
			transform = root;
			scripts = new List<Node>(root.GetChildren());
			for (int i = 0; i < root.GetChildCount(); i++)
			{
				Node child = root.GetChild(i);
				if (child is Node3D child3D)
					childs.Add(new Unit(child3D));
			}
		}

		public void Draw(int iteration)
		{
			// Debug visualization - port to Godot UI as needed
		}

		public void DrawScripts()
		{
			// Debug visualization - port to Godot UI as needed
		}

		private string GetOffset(int iteration)
		{
			if (!Offsets.ContainsKey(iteration))
			{
				string text = "__";
				for (int i = 0; i < iteration; i++)
				{
					text += "_|_";
				}
				Offsets.Add(iteration, text);
			}
			return Offsets[iteration];
		}
	}

	private List<Unit> _units = new List<Unit>();
	private static Unit _current;

	private void GetUnits()
	{
		_units.Clear();
		foreach (Node node in GetTree().Root.GetChildren())
		{
			if (node is Node3D n3d && node != this)
			{
				_units.Add(new Unit(n3d));
			}
		}
	}

	public override void _Ready()
	{
		_current = null;
		GetUnits();
	}

	public override void _Process(double delta)
	{
	}
}
