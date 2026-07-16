using Godot;
using System;

public partial class RewardCoin : Node
{
	[Export]
	private Label _name;

	[Export]
	private Label _coinsValue;

	[Export]
	private Label _bonusValue;

	private Tween _tween;
	private Vector2 _targetPosition;
	private float _targetAlpha = 1f;

	public Action OnFinish { get; set; }

	public override void _Ready()
	{
		if (_name == null) GD.PrintErr("Missing _name label.");
		if (_coinsValue == null) GD.PrintErr("Missing _coinsValue label.");
		if (_bonusValue == null) GD.PrintErr("Missing _bonusValue label.");
	}

	public void SetCoins(string name, string value, int count)
	{
		_name.Text = $"{Localization.Get(name)} x{count}";
		_coinsValue.Text = value;
		SetAnimation();
	}

	public void Set(string name, string coinsValue, string bonusValue)
	{
		_name.Text = Localization.Get(name);
		_coinsValue.Text = coinsValue;
		_bonusValue.Text = bonusValue;
		SetAnimation();
	}

	private void SetAnimation()
	{
		if (this is Control c)
		{
			_targetPosition = new Vector2(0f, c.Position.Y);
			_targetAlpha = 1f;
		}
	}

	public void Animate()
	{
		if (this is Control c)
		{
			_tween = GetTree().CreateTween();
			_tween.TweenProperty(c, "position", _targetPosition, 0.3f);
			_tween.TweenProperty(c, "modulate:a", _targetAlpha, 0.3f);
			_tween.Finished += () => OnFinish?.Invoke();
		}
	}

	public void BreakAnimate()
	{
		_tween?.Kill();
		if (this is Control c)
		{
			c.Position = _targetPosition;
			c.Modulate = new Color(c.Modulate, 1f);
		}
	}
}
