using System;
using Godot;
using Network.core.events;

public class RefreshBattlesNetworkState : TCPNetworkState
{
	public override void TCPStart(object data) { }
	public override void TCPStop() { }

	public void OnEnter()
	{
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
