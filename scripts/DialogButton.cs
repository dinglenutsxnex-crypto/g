// ⚠️ STUB: needs full port — original used UnityEngine.UI.Text, Image, RectTransform
using Godot;
using System;

public partial class DialogButton : Control
{
	[Export]
	private Label _label;
	[Export]
	private TextureRect _arrow;
	[Export]
	private Button _button;
	private LocalizationText _localizationText;

	public override void _Ready()
	{
	}

	public void SetLabel(string alias, bool hasArrow = true)
	{
	}

	public void SetColor(Color? color)
	{
		if (color.HasValue)
		{
			_label.Modulate = color.Value;
			if (_arrow.Visible)
			{
				_arrow.Modulate = color.Value;
			}
		}
	}

	public void AddCallback(Action callback)
	{
		_button.Pressed += callback;
	}

	public void AddCallback(Action<string, object> callback, object arg)
	{
		_button.Pressed += () => callback(_localizationText.GetAlias(), arg);
	}
}
