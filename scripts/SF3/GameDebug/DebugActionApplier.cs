using Godot;
using System;

namespace SF3.GameDebug
{
	public class DebugActionApplier : Node
	{
		public EDebugActions currentDebugAction;

		public override void _Ready()
		{
			if (!NekkiUtils.IsDebug)
			{
				QueueFree();
			}
			else
			{
				GetNode<UIButton>().Pressed += DebugAction;
			}
		}

		private void DebugAction()
		{
			GameDebugController.CheckDebugAction(currentDebugAction);
		}
	}
}
