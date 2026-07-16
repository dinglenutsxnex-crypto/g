using Godot;
using System;

public class DragArrow : Node3D
{
	public TutorialComponent dragable;
	public TutorialComponent target;

	private TweenPosition tween;
	private TweenAlpha tweenAplha;
	private UISprite sprite;

	public override void _Process(double delta)
	{
		float num = Mathf.Atan2(tween.to.x - tween.from.x, tween.to.y - tween.from.y) * 57.29578f;
		sprite.RotationDegrees = new Vector3(0f, 0f, 0f - num);
	}

	public void SetVisible(bool visible)
	{
		Visible = visible;
		if (visible)
		{
			tween.ResetToBeginning();
			tweenAplha.ResetToBeginning();
			GD.Print("SHOW");
		}
	}
}
