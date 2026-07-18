using System;
using Godot;

public partial class DebugCurrentServerController : Node
{
	[Export]
	private Label _serverLabel;

	public void OnDevServerChosen(string serverName)
	{
		GD.Print("DebugCurrentServerController.OnDevServerChosen: " + serverName);
		if (_serverLabel != null)
			_serverLabel.Text = serverName;
	}

	public void Refresh()
	{
		GD.Print("DebugCurrentServerController.Refresh");
	}
}
