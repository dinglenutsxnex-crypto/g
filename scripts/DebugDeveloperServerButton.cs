// ⚠️ STUB: needs full port — original used NGUI UILabel and UIButton
using Godot;
using System;

public partial class DebugDeveloperServerButton : Node
{
	[Export]
	private Label _serverName;
	private int _serverNum;
	private Action<int> _choseServerAction;

	public void OnClick()
	{
		if (_choseServerAction != null)
		{
			_choseServerAction(_serverNum);
		}
	}

	public void Init(int serverNum, Action<int> chooseServer)
	{
		_serverNum = serverNum;
		Name = "DEV " + _serverNum.ToString("00");
		_serverName.Text = Name;
		_choseServerAction = chooseServer;
	}

	public void HighLight(string server)
	{
	}
}
