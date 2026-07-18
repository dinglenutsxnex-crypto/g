using System;
using System.Collections.Generic;
using Godot;
using Network.core.data;
using Network.core.events;
using Network.core;

public partial class NetworkConnection : Node
{
	public static NetworkConnection current;
	public string Server { get; set; }
	public int TCPPort { get; set; }
	public string CurrentConfigVersion { get; set; }
	public bool InsideDarkPocket { get; set; }
	public static string CurrentStateName { get; set; }
	public UserDataInfo UserData { get; set; }

	[Export]
	private string _defaultServer = "localhost";

	[Export]
	private int _defaultPort = 9339;

	private bool _isConnected;
	private Queue<NetworkEvent> _pendingEvents = new Queue<NetworkEvent>();

	public partial class UserDataInfo
	{
		public AuthData Auth { get; set; }
	}

	public partial class AuthData
	{
		public string Login { get; set; }
		public string Token { get; set; }
	}

	public override void _Ready()
	{
		base._Ready();
		current = this;
		Server = _defaultServer;
		TCPPort = _defaultPort;
		UserData = new UserDataInfo { Auth = new AuthData() };
		CurrentConfigVersion = "0.0.0";
	}

	public void Connect()
	{
		GD.Print("NetworkConnection.Connect: " + Server + ":" + TCPPort);
		_isConnected = true;
	}

	public void Disconnect()
	{
		GD.Print("NetworkConnection.Disconnect");
		_isConnected = false;
	}

	public bool IsNetworkEstablished()
	{
		return _isConnected;
	}

	public bool canCreatePlayer()
	{
		return _isConnected;
	}

	public static void Send(string command, object data, Action<NetworkEvent> callback)
	{
		GD.Print("NetworkConnection.Send: " + command);
		if (current != null)
		{
			NetworkEvent evt = new NetworkEvent(command, data);
			evt.callback = callback;
			evt.success = true;
			current._pendingEvents.Enqueue(evt);
		}
	}

	public override void _Process(double delta)
	{
		while (_pendingEvents.Count > 0)
		{
			NetworkEvent evt = _pendingEvents.Dequeue();
			NetworkEventManager.Instance.DispatchEvent(evt);
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (current == this)
			current = null;
	}
}
