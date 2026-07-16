using Godot;

public partial class UISpriteAnimationDelay : Node
{
	[Export]
	public float delay = 1f;

	private AnimatedSprite2D _anim;
	private float _finish;
	private bool _animationCompleted;

	public override void _Ready()
	{
		_anim = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
		if (_anim != null)
		{
			_anim.AnimationFinished += () => _animationCompleted = true;
		}
		_animationCompleted = false;
	}

	public override void _Process(double delta)
	{
		if (_anim != null && !_anim.IsPlaying())
		{
			float time = (float)Engine.GetProcessTime();
			if (!_animationCompleted)
			{
				_finish = time;
				_animationCompleted = true;
			}
			if (time - _finish >= delay)
			{
				_anim.Play();
				_animationCompleted = false;
			}
		}
	}
}
