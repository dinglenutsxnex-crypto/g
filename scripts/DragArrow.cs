using Godot;
using System;

public partial class DragArrow : Node3D
{
	public TutorialComponent dragable;
	public TutorialComponent target;

	private Vector3 _tweenFrom;
	private Vector3 _tweenTo;
	private Tween _tween;
	private Tween _tweenAlpha;
	private TextureRect sprite;

	public override void _Process(double delta)
	{
		if (_tween == null) return;
		float num = Mathf.Atan2(_tweenTo.x - _tweenFrom.x, _tweenTo.y - _tweenFrom.y) * 57.29578f;
		sprite.RotationDegrees = new Vector3(0f, 0f, 0f - num);
	}

	public void SetVisible(bool visible)
	{
		Visible = visible;
		if (visible)
		{
			if (_tween != null) _tween.Kill();
			if (_tweenAlpha != null) _tweenAlpha.Kill();
			_tween = CreateTween();
			_tweenAlpha = CreateTween();
			_tween.TweenProperty(sprite, "position", _tweenTo, 1f);
			_tweenAlpha.TweenProperty(sprite, "modulate:a", 1f, 1f);
			GD.Print("SHOW");
		}
	}
}
