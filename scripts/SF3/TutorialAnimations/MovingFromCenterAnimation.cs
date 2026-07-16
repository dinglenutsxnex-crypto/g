using Godot;
using System;

namespace SF3.TutorialAnimations
{
	public class MovingFromCenterAnimation : Animation
	{
		private const float DURATION = 0.8f;
		private const float MOVE_DELAY = 0.5f;

		public MovingFromCenterAnimation(Control owner, Vector2 offset)
			: base(owner, offset)
		{
		}

		public override void InAnimation()
		{
			Owner.Modulate = new Color(Owner.Modulate, 0f);
			SetStartPosition();
			Tween tween = CreateTween();
			tween.TweenProperty(Owner, "modulate:a", 1f, 0.8f);
			tween.TweenInterval(0.5f);
			tween.TweenProperty(Owner, "position:y", Offset.y, 0.8f).SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.InOut);
			tween.Play();
		}

		public override void OutAnimation(Action callback)
		{
			Tween tween = CreateTween();
			tween.TweenProperty(Owner, "modulate:a", 0f, 0.8f);
			tween.Finished += () => callback();
		}

		private void SetStartPosition()
		{
			Control component = NekkiCanvasRoot.instance.Canvas;
			float num = (component.Size.y - Owner.Size.y) / 2f;
			Owner.Position = new Vector2(Owner.Position.x, 0f - num);
		}
	}
}
