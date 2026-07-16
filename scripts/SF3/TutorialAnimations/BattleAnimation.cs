using Godot;
using System;

namespace SF3.TutorialAnimations
{
	public class BattleAnimation : Animation
	{
		private const float DURATION = 0.25f;

		public BattleAnimation(Control owner, Vector2 offset)
			: base(owner, offset)
		{
		}

		public override void InAnimation()
		{
			SetStartPosition();
			Tween tween = CreateTween();
			tween.TweenProperty(Owner, "position:y", Offset.y, 0.25f);
		}

		public override void OutAnimation(Callable callback)
		{
			Tween tween = CreateTween();
			tween.TweenProperty(Owner, "modulate:a", 0f, 0.25f);
			tween.Finished += callback;
		}

		private void SetStartPosition()
		{
			Vector2 anchoredPosition = Owner.Position + Offset;
			if (Owner.PivotOffset.y == 1f)
			{
				anchoredPosition.y += Owner.Size.y;
			}
			else
			{
				anchoredPosition.y -= Owner.Size.y;
			}
			Owner.Position = anchoredPosition;
		}
	}
}
