using System;
using Godot;
using Network.core.events;

public class RefreshBattlesNetworkState : TCPNetworkState
{
	public override void _Ready()
	{
		base._Ready();
	}

	protected override void OnEnter()
	{
		base.OnEnter();
		SendRefreshRequest();
	}

	private void SendRefreshRequest()
	{
		NetworkConnection.Send("refresh_battles", null, delegate(NetworkEvent e)
		{
			if (e.success)
			{
				OnRefreshComplete();
			}
			else
			{
				OnRefreshFailed();
			}
		});
	}

	private void OnRefreshComplete()
	{
		Exit();
	}

	private void OnRefreshFailed()
	{
		Retry();
	}

	private void Retry()
	{
		SendRefreshRequest();
	}
}
