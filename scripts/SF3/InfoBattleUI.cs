using System;
using Godot;

public partial class InfoBattleUI : Node
{
	[Export]
	public Label infoLabel;

	[Export]
	public Label messageLabel;

	[Export]
	public Button okButton;

	[Export]
	public Node infoPanel;

	[Export]
	public Node messagePanel;

	public void ShowInfo(string text)
	{
		GD.Print("STUB: InfoBattleUI.ShowInfo: " + text);
	}

	public void ShowMessage(string text)
	{
		GD.Print("STUB: InfoBattleUI.ShowMessage: " + text);
	}

	public void Hide()
	{
		Visible = false;
	}
}
