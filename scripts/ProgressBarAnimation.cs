using Godot;

public partial class ProgressBarAnimation : Node
{
	public delegate void AnimationEnd();

	[Export] public float duration = 1f;
	[Export] public float to;

	public AnimationEnd onAnimationEnd;

	private ProgressBar progressBar;
	private bool animationPlaying;
	private float from;
	private float timeStart;

	public bool IsPlay { get { return animationPlaying; } }

	public override void _Ready()
	{
		progressBar = GetNode<ProgressBar>(".");
		animationPlaying = false;
	}

	public void Play(float animateTo)
	{
		to = animateTo;
		Play();
	}

	public void Play()
	{
		animationPlaying = true;
		from = progressBar.Value;
		timeStart = Engine.GetProcessTime();
	}

	public void Stop(bool triggerEvent = true)
	{
		animationPlaying = false;
		if (onAnimationEnd != null && triggerEvent)
		{
			onAnimationEnd();
		}
	}

	public void Break()
	{
		progressBar.Value = to;
		Stop();
	}

	public override void _Process(double delta)
	{
		if (animationPlaying)
		{
			float t = (Engine.GetProcessTime() - timeStart) / duration;
			progressBar.Value = Mathf.Lerp(from, to, t);
			if (Mathf.Abs(progressBar.Value - to) < 0.001f)
			{
				progressBar.Value = to;
				Stop();
			}
		}
	}
}
