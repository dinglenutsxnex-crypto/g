using System;
using Godot;

namespace SF3.Utils
{
	public static class ImageExtensions
	{
		public const float UpperLimit = 1f;

		public const float LowerLimit = 0f;

		public static BehaviourTimer.SingleTimer CreateFillTimer(this ProgressBar image, int framesDuration, float fromAmount, float toAmount, Action onFinish = null)
		{
			image.Value = fromAmount * 100f;
			float delta = ((!(fromAmount > toAmount)) ? (0f - 1f / (float)framesDuration) : (1f / (float)framesDuration));
			return BehaviourTimer.CreateGameFramesTimer(framesDuration, delegate
			{
				image.Value -= delta * 100f;
			}, null, new Action<object>[1]
			{
				delegate
				{
					image.Value = toAmount * 100f;
					onFinish.InvokeSafe();
				}
			});
		}

		public static BehaviourTimer.SingleTimer CreateFillTimerAscending(this ProgressBar image, int framesDuration)
		{
			return image.CreateFillTimer(framesDuration, 0f, 1f);
		}

		public static BehaviourTimer.SingleTimer CreateFillTimerDescending(this ProgressBar image, int framesDuration)
		{
			return image.CreateFillTimer(framesDuration, 1f, 0f);
		}

		public static BehaviourTimer.SingleTimer CreateFillTimerAscending(this ProgressBar image, int framesDuration, Action onFinish)
		{
			return image.CreateFillTimer(framesDuration, 0f, 1f, onFinish);
		}

		public static BehaviourTimer.SingleTimer CreateFillTimerDescending(this ProgressBar image, int framesDuration, Action onFinish)
		{
			return image.CreateFillTimer(framesDuration, 1f, 0f, onFinish);
		}
	}
}
