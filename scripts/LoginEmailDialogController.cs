using System;
using Godot;

public class LoginEmailDialogController : Node
{
	[Export]
	public LineEdit emailField;

	[Export]
	public LineEdit passwordField;

	[Export]
	public Button loginButton;

	[Export]
	public Button cancelButton;

	[Export]
	public Label errorLabel;

	public override void _Ready()
	{
		GD.Print("STUB: LoginEmailDialogController._Ready");
	}

	public void Show()
	{
		Visible = true;
	}

	public void Hide()
	{
		Visible = false;
	}

	public void OnLoginClicked()
	{
		GD.Print("STUB: LoginEmailDialogController.OnLoginClicked");
	}

	public void OnCancelClicked()
	{
		Hide();
	}

	public void SetError(string error)
	{
		if (errorLabel != null)
			errorLabel.Text = error;
	}
}
