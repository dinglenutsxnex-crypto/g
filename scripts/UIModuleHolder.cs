using Godot;
using System;

public abstract partial class UIModuleHolder : ExtentionBehaviour
{
	protected NekkiUIModule _nekkiUIModule;

	public override void _Ready()
	{
		_nekkiUIModule = GetNode<NekkiUIModule>();
	}

	public virtual void Initialize()
	{
	}

	public virtual void ShowModule(IntentModule intent, Action openedCallback)
	{
	}

	public virtual void HideModule(Action closedCallback)
	{
	}

	public virtual void UpdateModule(IntentModule intent)
	{
	}
}
