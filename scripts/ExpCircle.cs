using System;
using Godot;

public class ExpCircle : Node
{
	[Export]
	public float fillAmount;

	[Export]
	public Color fillColor = Colors.Green;

	[Export]
	public float animationSpeed = 1.0f;

	private float _targetFill;

	public override void _Ready()
	{
		GD.Print("STUB: ExpCircle._Ready");
	}

	public void SetFill(float value)
	{
		_targetFill = Mathf.Clamp(value, 0f, 1f);
		GD.Print("STUB: ExpCircle.SetFill: " + value);
	}

	public void AnimateToFill(float value)
	{
		_targetFill = Mathf.Clamp(value, 0f, 1f);
		Tween tween = CreateTween();
		tween.TweenMethod(Callable.From((float v) => fillAmount = v), fillAmount, _targetFill, animationSpeed);
	}

	public override void _Process(double delta)
	{
	}
}
