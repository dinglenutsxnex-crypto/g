using System.Collections.Generic;
using Godot;

public partial class HorizontalAlign : Node
{
	public enum Anchor
	{
		center = 0,
		right = 1
	}

	[Export] public Godot.Collections.Array<Node> objects;
	[Export] public Anchor alignAnchor;
	[Export] public float offsetFactor = 1f;

	private const float DEFAULT_ASPECT = 1.33f;

	public override void _Ready()
	{
		if (objects == null || objects.Count == 0)
		{
			ReadTransforms();
		}

		float aspect = (float)DisplayServer.WindowGetSize().X / (float)DisplayServer.WindowGetSize().Y / DEFAULT_ASPECT;

		foreach (Node obj in objects)
		{
			if (obj is Control control)
			{
				Vector2 pos = control.Position;
				if (alignAnchor == Anchor.center)
				{
					pos.X *= aspect;
				}
				else if (alignAnchor == Anchor.right)
				{
					pos.X += (pos.X - pos.X * aspect) * offsetFactor;
				}
				control.Position = pos;
			}
			else if (obj is Node3D n3d)
			{
				Vector3 pos = n3d.Position;
				if (alignAnchor == Anchor.center)
				{
					pos.X *= aspect;
				}
				else if (alignAnchor == Anchor.right)
				{
					pos.X += (pos.X - pos.X * aspect) * offsetFactor;
				}
				n3d.Position = pos;
			}
		}
	}

	private void ReadTransforms()
	{
		objects = new Godot.Collections.Array<Node>();
		for (int i = 0; i < GetChildCount(); i++)
		{
			Node child = GetChild(i);
			if (child != this)
			{
				objects.Add(child);
			}
		}
	}
}
