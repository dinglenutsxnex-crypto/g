using Godot;
using System;
using System.Linq;

public class UnityCollisionRecognizer : Node
{
	private Collider _collider;

	public override void _Ready()
	{
		_collider = GetComponents<Collider>().FirstOrDefault((Collider c) => c.IsTrigger);
		if (_collider == null)
		{
			GD.PrintErr("Collider of wall is null");
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		ModelFinder component = other.GetNode<ModelFinder>();
		if (component != null)
		{
			component.ModelOfObject.WallHit(WallConfig.instance.GetWallType(_collider), _collider);
		}
	}
}
