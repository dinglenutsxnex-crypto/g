using System;
using Godot;
using Network.core.events;

public class BalancerNetworkState : TCPNetworkState
{
	private int _retryCount;
	private int _maxRetries = 3;

	protected override void OnEnter()
	{
		base.OnEnter();
		_retryCount = 0;
		RequestBalancer();
	}

	private void RequestBalancer()
	{
		GD.Print("BalancerNetworkState: Requesting balancer...");
		NetworkConnection.Send("get_balancer", null, delegate(NetworkEvent e)
		{
			if (e.success)
			{
				OnBalancerReceived(e);
			}
			else
			{
				OnBalancerFailed();
			}
		});
	}

	private void OnBalancerReceived(NetworkEvent e)
	{
		GD.Print("BalancerNetworkState: Balancer received");
		Exit();
	}

	private void OnBalancerFailed()
	{
		_retryCount++;
		if (_retryCount < _maxRetries)
		{
			GD.Print("BalancerNetworkState: Retrying... (" + _retryCount + "/" + _maxRetries + ")");
			RequestBalancer();
		}
		else
		{
			GD.PrintErr("BalancerNetworkState: Max retries reached");
			Exit();
		}
	}
}
