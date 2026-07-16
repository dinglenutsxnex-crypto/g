using System;
using Godot;
using Network.core.events;

	public class ConnectionNetworkState : TCPNetworkState
	{
		public override void TCPStart(object data) { }
		public override void TCPStop() { }

		protected void OnEnter()
	{
		base.OnEnter();
		GD.Print("ConnectionNetworkState: Connecting...");
		NetworkConnection.current.Connect();
	}

	protected void OnExit()
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
