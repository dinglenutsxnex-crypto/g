using System;
using Godot;

public partial class MultiCardProgress : Control
{
	public Action onAnimationEnd;

	[Export]
	private ProgressBar progressBar;

	[Export]
	private ProgressBar addedProgressBar;

	private ProgressBarAnimation progressAnim;

	private Tween addedProgressAnim;

	private float savedAddedProgress;

	private bool isPlay;

	private int repeatLvelsup;

	private int _levelups;

	public float progress
	{
		get
		{
			return progressBar.Value / 100f;
		}
		protected set
		{
			progressBar.Value = value * 100f;
		}
	}

	public float addedProgress
	{
		get
		{
			return addedProgressBar.Value / 100f;
		}
		protected set
		{
			addedProgressBar.Value = value * 100f;
		}
	}

	public int levelup
	{
		get
		{
			return _levelups;
		}
		protected set
		{
			_levelups = value;
			progressBar.Visible = _levelups == 0;
		}
	}

	public override void _Ready()
	{
		progressAnim = progressBar.GetNode<ProgressBarAnimation>(".");
		ProgressBarAnimation progressBarAnimation = progressAnim;
		progressBarAnimation.onAnimationEnd += OnAnimationEnd;
	}

	public void SetProgress(float current, float added, int levelupCount)
	{
		progress = current;
		addedProgress = added;
		levelup = levelupCount;
		savedAddedProgress = addedProgress;
		if (levelup > 0)
		{
			progressBar.Visible = true;
			addedProgress = 1f;
		}
	}

	public void AnimateProgress()
	{
		isPlay = true;
		repeatLvelsup = _levelups;
		if (repeatLvelsup == 0)
		{
			progressAnim.Play(savedAddedProgress);
			return;
		}
		progressBar.Visible = true;
		addedProgress = 1f;
		progressAnim.Play(1f);
	}

	private void OnAnimationEnd()
	{
		repeatLvelsup--;
		if (repeatLvelsup == 0)
		{
			progress = 0f;
			addedProgress = savedAddedProgress;
			progressAnim.Play(addedProgress);
			return;
		}
		if (repeatLvelsup > 0)
		{
			progress = 0f;
			progressAnim.Play(1f);
			return;
		}
		if (onAnimationEnd != null)
		{
			onAnimationEnd();
		}
		isPlay = false;
	}

	public void AnimateAddedProgress()
	{
		isPlay = true;
		repeatLvelsup = _levelups;
		if (repeatLvelsup == 0)
		{
			addedProgress = savedAddedProgress;
		}
		else
		{
			progressBar.Visible = true;
			addedProgress = 1f;
		}
		if (addedProgressAnim != null)
			addedProgressAnim.Kill();
		addedProgressAnim = CreateTween();
		addedProgressAnim.TweenProperty(addedProgressBar, "value", addedProgress * 100f, 0.5f);
		addedProgressAnim.Finished += OnAddedAnimationEnd;
	}

	private void OnAddedAnimationEnd()
	{
		isPlay = false;
	}

	public void BreakAnimation()
	{
		if (isPlay)
		{
			if (progressAnim != null)
				progressAnim.Stop(false);
			isPlay = false;
		}
	}
}
