using System;
using Godot;

public partial class EditorVertex : Node
{
	[Export]
	public Vector3 position;

	[Export]
	public Color color = Colors.White;

	[Export]
	public float radius = 0.1f;

	private MeshInstance3D _meshInstance;

	public override void _Ready()
	{
		GD.Print("STUB: EditorVertex._Ready");
	}

	public void SetPosition(Vector3 pos)
	{
		position = pos;
		if (_meshInstance != null)
			_meshInstance.Position = pos;
	}

	public void SetColor(Color c)
	{
		color = c;
	}

	public void Select()
	{
		GD.Print("STUB: EditorVertex.Select");
	}

	public void Deselect()
	{
		GD.Print("STUB: EditorVertex.Deselect");
	}
}
