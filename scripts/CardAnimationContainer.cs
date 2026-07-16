using System;
using Godot;

public partial class CardAnimationContainer : Node3D
{
	[Serializable]
	public class Animation
	{
		public string name;
		public Curve rotationCurve;
		public Curve movementCurve;
		public float duration = 1f;
	}

	[Export]
	public Animation[] animations;

	private Tween _currentTween;
	private bool _isInit;

	private Animation GetAnimationByName(string name)
	{
		foreach (Animation anim in animations)
		{
			if (anim.name == name)
				return anim;
		}
		GD.PrintErr("Animation not found");
		return null;
	}

	public async void Move(Vector3 to, string animation)
	{
		Init();
		Animation anim = GetAnimationByName(animation);
		if (anim == null) return;

		_currentTween = CreateTween();
		_currentTween.TweenProperty(this, "position", to, anim.duration);
		_currentTween.TweenProperty(this, "rotation", Vector3.Zero, anim.duration);
		await ToSignal(_currentTween, "finished");
	}

	private void Init()
	{
		if (_isInit) return;
		_isInit = true;
	}

	public override void _Ready()
	{
		Init();
	}
}
