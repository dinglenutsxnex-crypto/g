using System;
using Godot;

public partial class SimpleDialog : BaseDialog
{
	private static Action _onClose;

	private static string contentText;

	private static string titleText;

	private static string buttonText;

	public static SimpleDialog Show(string title, string content, string button, Action onClose = null)
	{
		contentText = content;
		titleText = title;
		buttonText = button;
		_onClose = onClose;

		SimpleDialog instance = new SimpleDialog();
		instance._Ready();
		return instance;
	}

	public static void Hide()
	{
		contentText = string.Empty;
		if (_onClose != null)
		{
			_onClose();
		}
	}

	public override void _Ready()
	{
		base._Ready();
		Title = !string.IsNullOrEmpty(titleText) ? titleText : "fight_end";
		CreateDialog();

		if (string.IsNullOrEmpty(contentText))
		{
			GD.PushError("SimpleDialog: contentText is empty, set dialog content manually");
		}

		if (string.IsNullOrEmpty(buttonText))
		{
			GD.PushError("SimpleDialog: buttonText is empty, set button label manually");
		}

		OnClose(Hide);
	}

	private void CreateDialog()
	{
		Visible = true;
	}
}
