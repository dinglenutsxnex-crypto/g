using System;
using Godot;

public partial class DebugTextLineController : DebugLineController
{
	[Export]
	public Label nameLabel;

	[Export]
	public Label value;

	[Export]
	public Label separator;

	private Action<DebugTextLineController> updateAction;

	internal void setup(string name, Action<DebugTextLineController> onUpdate)
	{
		nameLabel.Text = name;
		updateAction = onUpdate;
	}

	internal override void onUpdate()
	{
		if (updateAction != null)
		{
			updateAction(this);
		}
	}

	internal void HideSeparator()
	{
		separator.Modulate = new Color(separator.Modulate, 0f);
	}
}
