using System;
using Godot;
using Network.core.events;
using common;

public class JoinZoneNetworkState : TCPNetworkState
{
	private ulong JoinZoneTimeoutTimer;

	public override void TCPStart(object data)
	{
		NetworkConnection.current.addEventListener("join_zone", OnJoinZone);
		JoinZoneTimeoutTimer = (ulong)(NetworkConnection.Settings.JoinZoneTimeout.ToSeconds() * 1000.0);
		_ = GetTree().CreateTimer(NetworkConnection.Settings.JoinZoneTimeout.ToSeconds());
	}

	public override void TCPCleanup()
	{
		cancelJoinZoneTimeout();
		NetworkConnection.current.removeEventListener("join_zone", OnJoinZone);
	}

	public override void TCPStop()
	{
		Disconnect();
	}

	private void cancelJoinZoneTimeout()
	{
		// STUB: coroutine replaced with timer
		JoinZoneTimeoutTimer = 0;
	}

	private void OnJoinZone(NetworkEvent e)
	{
		cancelJoinZoneTimeout();
		JoinZoneEvent extensible = e.getExtensible<JoinZoneEvent>();
		GD.Print(string.Format("OnJoinZone Player Exists = {0}", extensible.PlayerExists));
		if (extensible.PlayerExists)
		{
			OnSuccess(typeof(GetPlayerNetworkState), null);
		}
		else if (!NetworkConnection.current.canCreatePlayer())
		{
			OnSuccess(typeof(WaitEndOfTutorialNetworkState), null);
		}
		else
		{
			OnSuccess(typeof(CreatePlayerNetworkState), null);
		}
	}
}
