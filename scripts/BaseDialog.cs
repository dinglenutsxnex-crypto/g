using System;
using Godot;

public abstract partial class BaseDialog : Control
{
	private Action _baseDialogOnClose;

	protected Node _content;

	[Export]
	public Label TitleLabel { get; set; }

	[Export]
	public Button CloseButton { get; set; }

	public virtual string Title
	{
		get
		{
			return TitleLabel != null ? TitleLabel.Text : string.Empty;
		}
		set
		{
			if (TitleLabel != null)
				TitleLabel.Text = value;
		}
	}

	public override void _Ready()
	{
		Init();
	}

	public virtual void OnClose(Action onClose)
	{
		_baseDialogOnClose = onClose;
		if (CloseButton != null)
			CloseButton.Pressed += CloseMethod;
	}

	private void CloseMethod()
	{
		if (_baseDialogOnClose != null)
		{
			_baseDialogOnClose();
		}
	}

	protected virtual void Init()
	{
	}
}
