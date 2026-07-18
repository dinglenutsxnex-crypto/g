using Godot;
using System;
using System.Linq;

public partial class UnityCollisionRecognizer : Node
{
	private Area3D _collider;

	public override void _Ready()
	{
		_collider = GetChildren().OfType<Area3D>().FirstOrDefault();
		if (_collider == null)
		{
			GD.PrintErr("Area3D of wall is null");
		}
	}
}
