using System;
using Godot;
using SF3.Effects;

public class TutorialGUIEffect : GUIEffect
{
	[Export]
	private Node _targetUI;

	[Export]
	private Color _highlightColor = new Color(1f, 1f, 0f, 0.5f);

	private bool _isActive;

	public override void Play()
	{
		base.Play();
		_isActive = true;
		if (_targetUI != null)
		{
			if (_targetUI is Control control)
			{
				control.Modulate = _highlightColor;
			}
		}
	}

	public override void Stop()
	{
		base.Stop();
		_isActive = false;
		if (_targetUI != null)
		{
			if (_targetUI is Control control)
			{
				control.Modulate = Colors.White;
			}
		}
	}

	public void SetTarget(Node target)
	{
		_targetUI = target;
	}
}
