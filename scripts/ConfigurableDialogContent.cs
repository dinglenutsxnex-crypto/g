using System;
using Godot;

public class ConfigurableDialogContent : Node
{
	[Export]
	public Label titleLabel;

	[Export]
	public Label messageLabel;

	[Export]
	public Button confirmButton;

	[Export]
	public Button cancelButton;

	[Export]
	public TextureRect iconTexture;

	public void Init(DialogConfig config)
	{
		GD.Print("STUB: ConfigurableDialogContent.Init");
		if (titleLabel != null && config != null)
			titleLabel.Text = config.Title;
		if (messageLabel != null && config != null)
			messageLabel.Text = config.Message;
	}

	public void Show()
	{
		Visible = true;
	}

	public void Hide()
	{
		Visible = false;
	}
}
