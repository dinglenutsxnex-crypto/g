using Godot;
using System;
using SF3.GameModels;

public partial class ModelFinder : Node
{
	private Model _modelOfObject;

	public Model ModelOfObject
	{
		get
		{
			return _modelOfObject ?? (_modelOfObject = GetNodeInParent<Model>());
		}
	}

	public override void _Ready()
	{
		_modelOfObject = GetNodeInParent<Model>();
		if (_modelOfObject == null)
		{
			GD.PrintErr("Model of object not found.");
		}
	}
}
