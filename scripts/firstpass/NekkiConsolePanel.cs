using System;
using Godot;

/// <summary>
/// In-game debug console panel — stub implementation.
/// </summary>
public partial class NekkiConsolePanel : Control
{
	private static NekkiConsolePanel _instance;
	public static NekkiConsolePanel Instance => _instance;

	public event Action<bool> OnConsoleActive;

	public override void _Ready()
	{
		_instance = this;
	}

	public void Log(string text)
	{
		GD.Print($"[Console] {text}");
	}

	public void Clear()
	{
		GD.Print("[Console] Cleared");
	}

	public void SetActive(bool active)
	{
		Visible = active;
		OnConsoleActive?.Invoke(active);
	}
}
