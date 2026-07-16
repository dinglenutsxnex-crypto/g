using System;
using System.Collections.Generic;
using Godot;

public class SystemMessage : Node
{
	private static SystemMessage _instance;

	[Export]
	public Label titleLabel;

	[Export]
	public Label messageLabel;

	[Export]
	public Button okButton;

	[Export]
	public Button cancelButton;

	[Export]
	public Node messagePanel;

	private List<Button> _buttons = new List<Button>();

	public static SystemMessage Instance
	{
		get { return _instance; }
	}

	public override void _Ready()
	{
		base._Ready();
		_instance = this;
		GD.Print("STUB: SystemMessage._Ready");
	}

	public static SystemMessage ShowAlert(string msgAlias, string[] args, string confirmAlias)
	{
		GD.Print("STUB: SystemMessage.ShowAlert: " + msgAlias);
		if (_instance == null)
		{
			_instance = new SystemMessage();
		}
		return _instance;
	}

	public void AddButton(string text, Action callback)
	{
		GD.Print("STUB: SystemMessage.AddButton: " + text);
	}

	public void Show()
	{
		GD.Print("STUB: SystemMessage.Show");
		Visible = true;
	}

	public void Hide()
	{
		Visible = false;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_instance == this)
			_instance = null;
	}
}
