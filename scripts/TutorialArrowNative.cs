using Godot;
public partial class TutorialArrowNative : TutorialPointer
{
	public float Offset;
	public float Amplitude;
	public Curve AnimCurve;
	public Color StartColor;
	public Color EndColor;
	[Export]
	private TextureRect _image;
	public void Init(float duration)
	{
		if (!_init)
		{
			base.Init();
			AddPositionTween(_image, Amplitude, duration, AnimCurve);
			AddColorTween(_image, StartColor, EndColor, duration);
		}
	}
	public void SetPosition(Control target, ArrowPosition position)
	{
		switch (position)
		{
		case ArrowPosition.Bottom:
		{
			float x = target.Position.Y - Offset;
			_image.Position = new Vector2(x, _image.Position.Y);
			break;
		}
		case ArrowPosition.Top:
		{
			float x = target.Position.Y + target.Size.Y + Offset;
			_image.Position = new Vector2(x, _image.Position.Y);
			break;
		}
		case ArrowPosition.Left:
		{
			float y = target.Position.X - Offset;
			_image.Position = new Vector2(_image.Position.X, y);
			break;
		}
		case ArrowPosition.Right:
		{
			float y = target.Position.X + target.Size.X + Offset;
			_image.Position = new Vector2(_image.Position.X, y);
			break;
		}
		}
	}
}
