using System;
using Godot;

public partial class TooltipUnit : Control
{
	[Export]
	private Label _tooltip;

	[Export]
	private Control[] _arrows;

	[Export]
	private TextureRect _back;

	private Action _onDestroy;

	public void Init(Vector2 screenPosition, float showTime, Action<Label, TextureRect, Control> format, Action onDestroy)
	{
		_onDestroy = onDestroy;
		int num = SelectBack(screenPosition);
		_back.Visible = false;
		for (int i = 0; i < _arrows.Length; i++)
		{
			if (i == num)
				format(_tooltip, _back, _arrows[i]);
			_arrows[i].Visible = false;
		}
		HideAfterDelay(showTime);
	}

	private async void HideAfterDelay(float showTime)
	{
		await ToSignal(GetTree().CreateTimer(showTime), "timeout");
		if (IsInstanceValid(this))
			QueueFree();
	}

	private int SelectBack(Vector2 screenPosition)
	{
		Vector2 screenSize = DisplayServer.WindowGetSize();
		if (screenPosition.X >= 0f && screenPosition.X < screenSize.X * 0.3f)
		{
			if (screenPosition.Y >= 0f && screenPosition.Y < screenSize.Y * 0.3f) return 5;
			if (screenPosition.Y >= screenSize.Y * 0.3f && screenPosition.Y < screenSize.Y * 0.7f) return 3;
			return 0;
		}
		if (screenPosition.X >= screenSize.X * 0.3f && screenPosition.X < screenSize.X * 0.7f)
		{
			if (screenPosition.Y >= 0f && screenPosition.Y < screenSize.Y * 0.5f) return 6;
			return 1;
		}
		if (screenPosition.Y >= 0f && screenPosition.Y < screenSize.Y * 0.3f) return 7;
		if (screenPosition.Y >= screenSize.Y * 0.3f && screenPosition.Y < screenSize.Y * 0.7f) return 4;
		return 2;
	}

	public override void _ExitTree()
	{
		_onDestroy?.Invoke();
	}
}
