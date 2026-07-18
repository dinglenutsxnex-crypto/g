using System;
using Godot;

public partial class TutorialPanelScaler : Node
{
	[Export]
	private ImageWrapper[] _targets;

	[Export]
	private TextWrapper _textLabel;

	[Export]
	private Vector2 _offset;

	public override void _Ready()
	{
		TextWrapper textLabel = _textLabel;
		textLabel.onTextChage = (Action)Delegate.Combine(textLabel.onTextChage, new Action(DoScale));
		DoScale();
	}

	public override void _ExitTree()
	{
		TextWrapper textLabel = _textLabel;
		textLabel.onTextChage = (Action)Delegate.Remove(textLabel.onTextChage, new Action(DoScale));
	}

	private void DoScale()
	{
		float preferredSize = _textLabel.Size.X;
		float preferredSize2 = _textLabel.Size.Y;
		ImageWrapper[] targets = _targets;
		foreach (ImageWrapper imageWrapper in targets)
		{
			imageWrapper.Size = new Vector2(preferredSize / (float)_targets.Length + _offset.X, preferredSize2 + _offset.Y);
		}
	}
}
