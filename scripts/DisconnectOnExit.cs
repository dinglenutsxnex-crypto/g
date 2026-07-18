using Godot;
using System;

public partial class DisconnectOnExit : Node
{
	private NetworkConnection _connection;

	public static void Init(NetworkConnection connection)
	{
		DisconnectOnExit disconnectOnExit = new DisconnectOnExit();
		disconnectOnExit.Name = "_networkDisconnector";
		StaticObjectsManager.AddObject(disconnectOnExit);
		disconnectOnExit._connection = connection;
	}

	public override void _ExitTree()
	{
		try
		{
			if (NetworkConnection.HasInstance())
			{
				_connection.EnterDarkPocket();
				_connection.RestartConnection("Game Closing");
			}
		}
		catch (Exception)
		{
		}
	}
}
