using Godot;

public partial class BoosterpackSlash : Control
{
	private const float AppearFadeDuration = 0.2f;
	private const float DisappearFadeDuration = 1.2f;
	private const float AfterExplosionFadeDuration = 0.1f;
	private const float TimeToShowSwipeTrail = 0.4f;

	[Export]
	private TextureRect swipeTrail;

	[Export]
	private TextureRect fier;

	[Export]
	private TextureRect fierTop;

	public void SetFierPositionAndSize(Vector2 cutInPosition, Vector2 cutOutPosition)
	{
		Vector2 dir = cutOutPosition - cutInPosition;
		fier.Position = new Vector2(cutInPosition.X + dir.X / 2f, cutInPosition.Y + dir.Y / 2f);
		Vector2 a = GetParentControl().ToLocal(cutInPosition);
		Vector2 b = GetParentControl().ToLocal(cutOutPosition);
		float dist = a.DistanceTo(b);
		fier.Size = new Vector2(dist, fier.Size.Y);
		fierTop.Size = fier.Size;
	}

	public void PlaySwipeTrailAnimation()
	{
		swipeTrail.Modulate = new Color(swipeTrail.Modulate, 0f);
		Tween t = GetTree().CreateTween();
		t.TweenProperty(swipeTrail, "modulate:a", 1f, 0.2f).SetEase(Tween.EaseType.In);
		t.TweenInterval(0.4f);
		t.TweenProperty(swipeTrail, "modulate:a", 0f, 1.2f).SetEase(Tween.EaseType.Out);
	}

	public void PlayBeforeExplosionAnimation()
	{
		swipeTrail.Modulate = new Color(swipeTrail.Modulate, 0f);
		Tween t = GetTree().CreateTween();
		t.Parallel().TweenProperty(fier, "modulate:a", 0f, 0.1f);
		t.Parallel().TweenProperty(swipeTrail, "modulate:a", 0f, 0.1f);
	}

	private Control GetParentControl()
	{
		return GetParent() as Control;
	}
}
