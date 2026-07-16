using Godot;

public partial class UISpriteRadiantAnimation : Node
{
	private TextureRect _sprite;

	private bool _animationActive;

	[Export]
	private Curve _fillingCurve;

	[Export]
	private float _fillTime = 1f;

	private float _currentFillTime;

	[Export]
	private bool _normalLoop;

	[Export]
	private bool _pingPong;

	[Export]
	private bool _invertedLoop;

	[Export]
	private bool _runOnEnable;

	private int _forward = 1;

	private bool _looped;

	public override void _Ready()
	{
		_sprite = GetNode<TextureRect>(".");
	}

	public override void _Process(double delta)
	{
		if (!_animationActive)
		{
			return;
		}
		_currentFillTime += (float)delta * (float)_forward;
		if (_currentFillTime > _fillTime || _currentFillTime < 0f)
		{
			if (!_looped)
			{
				Stop();
				return;
			}
			if (_pingPong)
			{
				if (_forward == 1)
				{
					_currentFillTime = _fillTime;
					_forward = -1;
				}
				else
				{
					_currentFillTime = 0f;
					_forward = 1;
				}
			}
			else if (_normalLoop)
			{
				_currentFillTime = 0f;
				_forward = 1;
			}
			else if (_invertedLoop)
			{
				if (_forward == 1)
				{
					_currentFillTime = _fillTime;
					_forward = -1;
				}
				else
				{
					_currentFillTime = 0f;
					_forward = 1;
				}
			}
		}
		float time = _currentFillTime / _fillTime;
		if (_sprite != null && _sprite.Material is ShaderMaterial sm)
		{
			sm.SetShaderParameter("fillAmount", _fillingCurve.Sample(time));
		}
	}

	public override void _EnterTree()
	{
		if (_runOnEnable)
		{
			Play();
		}
	}

	public override void _ExitTree()
	{
		Stop();
	}

	public void Play()
	{
		if (_sprite == null)
		{
			return;
		}
		Reset();
		_looped = _normalLoop || _pingPong || _invertedLoop;
		_animationActive = true;
	}

	public void Pause()
	{
		_animationActive = false;
	}

	public void Resume()
	{
		_animationActive = true;
	}

	public void Stop()
	{
		_animationActive = false;
		Reset();
	}

	private void Reset()
	{
		_forward = 1;
		_currentFillTime = 0f;
	}
}
