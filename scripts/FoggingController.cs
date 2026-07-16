// ⚠️ STUB: needs full port — original used UnityEngine.UI.Image, DOTween DOFade
using Godot;
using System;

public partial class FoggingController : Control
{
	[Export]
	private TextureRect _background;
	[Export]
	private float _fadeDuration = 1f;
	[Export]
	private float _fadeDelay;
	[Export]
	private float _opacity = 0.5f;

	private static FoggingController _instance;

	public static FoggingController Instance
	{
		get
		{
			if (_instance == null)
			{
			}
			return _instance;
		}
	}

	public bool Active
	{
		get { return _background.Visible; }
	}

	public override void _Ready()
	{
		_instance = this;
		_background.Modulate = new Color(0f, 0f, 0f, 0f);
		_background.Visible = false;
	}

	public void ShowFogging()
	{
		_background.Visible = true;
	}

	public void HideFogging()
	{
		_background.Visible = false;
	}

	public void ShowFogging(float value, float duration, float delay, Action onDone = null)
	{
	}

	public void ChangeOpacity(float to)
	{
		_opacity = to;
	}
}
