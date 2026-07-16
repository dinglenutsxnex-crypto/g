using System;
using Godot;

public class FeedbackDialogController : Node
{
	[Export]
	public LineEdit feedbackField;

	[Export]
	public Button submitButton;

	[Export]
	public Button cancelButton;

	[Export]
	public Label statusLabel;

	public override void _Ready()
	{
		GD.Print("STUB: FeedbackDialogController._Ready");
	}

	public void Show()
	{
		Visible = true;
	}

	public void Hide()
	{
		Visible = false;
	}

	public void OnSubmit()
	{
		GD.Print("STUB: FeedbackDialogController.OnSubmit");
	}

	public void OnCancel()
	{
		Hide();
	}
}
