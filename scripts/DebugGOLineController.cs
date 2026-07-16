using System;
using Godot;

public partial class DebugGOLineController : DebugLineController
{
	private System.Action<DebugGOLineController> updateAction;

	public Node go;

	internal virtual void setup(System.Action<DebugGOLineController> onUpdate)
	{
		go = this;
		updateAction = onUpdate;
	}

	internal override void onUpdate()
	{
		if (updateAction != null)
		{
			updateAction(this);
		}
	}
}
