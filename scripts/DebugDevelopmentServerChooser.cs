using System;
using System.Collections.Generic;
using Godot;

public partial class DebugDevelopmentServerChooser : DebugGOLineController
{
	[Export]
	private GridContainer _grid;
	[Export]
	private DebugDeveloperServerButton _choseButtonPrototype;
	[Export]
	private int _servers;

	private List<DebugDeveloperServerButton> _buttons = new List<DebugDeveloperServerButton>();

	public event Action<int> DevServerChosen;

	internal override void setup(Action<DebugGOLineController> onUpdate)
	{
		base.setup(onUpdate);
	}

	public override void _Ready()
	{
		for (int i = 2; i <= _servers; i++)
		{
			DebugDeveloperServerButton debugDeveloperServerButton = _choseButtonPrototype.Duplicate() as DebugDeveloperServerButton;
			debugDeveloperServerButton.Init(i, ChooseServer);
			debugDeveloperServerButton.Visible = true;
			_buttons.Add(debugDeveloperServerButton);
		}
		NetworkConnection.current.OnNetworkEstablished += OnNetworkEstablished;
		NetworkConnection.current.OnNetworkCutoff += OnNetworkCutoff;
		OnNetworkEstablished();
	}

	private void OnNetworkCutoff()
	{
		Select(NetworkConnection.current.CurrentConfigVersion.serverName);
	}

	private void OnNetworkEstablished()
	{
		Select(NetworkConnection.current.CurrentConfigVersion.serverName);
	}

	public void Select(string server)
	{
		foreach (DebugDeveloperServerButton button in _buttons)
		{
			button.HighLight(server);
		}
	}

	private void ChooseServer(int number)
	{
		if (this.DevServerChosen != null)
		{
			this.DevServerChosen(number);
		}
	}
}
