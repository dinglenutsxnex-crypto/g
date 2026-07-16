using Godot;
using System;

public class KeyboardController : AbstractController
{
	private static KeyboardController _instance;

	public static KeyboardController Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new KeyboardController();
				_instance.Name = "keyboardController";
			}
			return _instance;
		}
	}

	public override void _Ready()
	{
		_instance = this;
	}

	public override void _Process(double delta)
	{
		if (!SystemProperties.IsMobilePlatform)
		{
			TrackKeys();
		}
	}

	public static void SetActive(bool value)
	{
		if (_instance != null)
		{
			_instance.Visible = value;
		}
	}
}
