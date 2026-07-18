using System;
using Godot;
using Nekki;
using Nekki.UI;

public partial class FightHolderModule : NekkiUIModule
{
	[Export]
	private Node _fightContainer;

	public void MountModule(NekkiUIModule module)
	{
		module.Visible = true;
	}

	public void UnmountCurrent()
	{
	}

	public override void _Ready()
	{
		base._Ready();
	}
}
