using System;
using Godot;
using Network.core.events;

public class ConnectionNetworkState : TCPNetworkState
{
	protected override void OnEnter()
	{
		base.OnEnter();
		GD.Print("ConnectionNetworkState: Connecting...");
		NetworkConnection.current.Connect();
	}

	protected override void OnExit()
	{
		base.OnExit();
		GD.Print("ConnectionNetworkState: Connected");
	}

	public void OnConnectionSuccess()
	{
		Exit();
	}

	public void OnConnectionFailed(string reason)
	{
		GD.PrintErr("ConnectionNetworkState: Failed - " + reason);
		Retry();
	}

	private void Retry()
	{
		NetworkConnection.current.Connect();
	}
}
