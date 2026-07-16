using Godot;

[Tool]
public partial class AdjustUI : Control
{
	public enum AnchorModes
	{
		None = 0,
		Pixel = 1,
		Relatively = 2
	}

	public enum ScaleModes
	{
		None = 0,
		BaseOfHeight = 1,
		BaseOnWidth = 2,
		BaseOnMinimum = 3,
		BaseOnMaximum = 4,
		Both = 5
	}

	public enum AnchorPivots
	{
		BottomLeft = 0,
		Bottom = 1,
		BottomRight = 2,
		TopLeft = 3,
		Top = 4,
		TopRight = 5,
		MiddleLeft = 6,
		Middle = 7,
		MiddleRight = 8
	}

	public AnchorModes AnchorMode;
	public AnchorPivots Pivot;
	public AnchorPivots AnchorPivot;
	public float OffsetX;
	public float OffsetY;
	public ScaleModes ScaleMode;
	public float ScaleX = 1f;
	public float ScaleY = 1f;
	public Control TransformCtrl;
	public static Canvas Canvas;
	public bool ExecuteOnUpdate;

	private static float ScreenWidth => DisplayServer.WindowGetSize().X;
	private static float ScreenHeight => DisplayServer.WindowGetSize().Y;

	public override void _Ready()
	{
		Adjust();
	}

	public override void _Process(double delta)
	{
		if (ExecuteOnUpdate || Engine.IsEditorHint())
		{
			Adjust();
		}
	}

	private void Adjust()
	{
		if (TransformCtrl == null)
			TransformCtrl = this;
		if (Canvas == null)
			Canvas = GetParent() as Canvas;

		switch (ScaleMode)
		{
			case ScaleModes.None:
				TransformCtrl.Scale = Vector2.One;
				break;
			case ScaleModes.BaseOfHeight:
			{
				float s = ScreenHeight * ScaleY;
				float scale = s / TransformCtrl.Size.Y;
				TransformCtrl.Scale = new Vector2(scale, scale);
				break;
			}
			case ScaleModes.BaseOnWidth:
			{
				float s = ScreenWidth * ScaleX;
				float scale = s / TransformCtrl.Size.X;
				TransformCtrl.Scale = new Vector2(scale, scale);
				break;
			}
			case ScaleModes.Both:
			{
				float sx = ScreenWidth * ScaleX / TransformCtrl.Size.X;
				float sy = ScreenHeight * ScaleY / TransformCtrl.Size.Y;
				TransformCtrl.Scale = new Vector2(sx, sy);
				break;
			}
			case ScaleModes.BaseOnMaximum:
			{
				float sx = ScreenWidth * ScaleX;
				float sy = ScreenHeight * ScaleY;
				if (sx > sy)
				{
					float s = sx / TransformCtrl.Size.X;
					TransformCtrl.Scale = new Vector2(s, s);
				}
				else
				{
					float s = sy / TransformCtrl.Size.Y;
					TransformCtrl.Scale = new Vector2(s, s);
				}
				break;
			}
			case ScaleModes.BaseOnMinimum:
			{
				float sx = ScreenWidth * ScaleX;
				float sy = ScreenHeight * ScaleY;
				if (sx < sy)
				{
					float s = sx / TransformCtrl.Size.X;
					TransformCtrl.Scale = new Vector2(s, s);
				}
				else
				{
					float s = sy / TransformCtrl.Size.Y;
					TransformCtrl.Scale = new Vector2(s, s);
				}
				break;
			}
		}

		switch (Pivot)
		{
			case AnchorPivots.BottomLeft: TransformCtrl.PivotOffset = new Vector2(0, TransformCtrl.Size.Y); break;
			case AnchorPivots.Bottom: TransformCtrl.PivotOffset = new Vector2(TransformCtrl.Size.X / 2, TransformCtrl.Size.Y); break;
			case AnchorPivots.BottomRight: TransformCtrl.PivotOffset = new Vector2(TransformCtrl.Size.X, TransformCtrl.Size.Y); break;
			case AnchorPivots.TopLeft: TransformCtrl.PivotOffset = Vector2.Zero; break;
			case AnchorPivots.Top: TransformCtrl.PivotOffset = new Vector2(TransformCtrl.Size.X / 2, 0); break;
			case AnchorPivots.TopRight: TransformCtrl.PivotOffset = new Vector2(TransformCtrl.Size.X, 0); break;
			case AnchorPivots.MiddleLeft: TransformCtrl.PivotOffset = new Vector2(0, TransformCtrl.Size.Y / 2); break;
			case AnchorPivots.Middle: TransformCtrl.PivotOffset = TransformCtrl.Size / 2; break;
			case AnchorPivots.MiddleRight: TransformCtrl.PivotOffset = new Vector2(TransformCtrl.Size.X, TransformCtrl.Size.Y / 2); break;
		}

		if (AnchorMode != AnchorModes.None)
		{
			switch (AnchorPivot)
			{
				case AnchorPivots.BottomLeft:
					TransformCtrl.Position = new Vector2(-0.5f * ScreenWidth, -0.5f * ScreenHeight);
					break;
				case AnchorPivots.Bottom:
					TransformCtrl.Position = new Vector2(0, -0.5f * ScreenHeight);
					break;
				case AnchorPivots.BottomRight:
					TransformCtrl.Position = new Vector2(0.5f * ScreenWidth, -0.5f * ScreenHeight);
					break;
				case AnchorPivots.TopLeft:
					TransformCtrl.Position = new Vector2(-0.5f * ScreenWidth, 0.5f * ScreenHeight);
					break;
				case AnchorPivots.Top:
					TransformCtrl.Position = new Vector2(0, 0.5f * ScreenHeight);
					break;
				case AnchorPivots.TopRight:
					TransformCtrl.Position = new Vector2(0.5f * ScreenWidth, 0.5f * ScreenHeight);
					break;
				case AnchorPivots.MiddleLeft:
					TransformCtrl.Position = new Vector2(-0.5f * ScreenWidth, 0);
					break;
				case AnchorPivots.Middle:
					TransformCtrl.Position = Vector2.Zero;
					break;
				case AnchorPivots.MiddleRight:
					TransformCtrl.Position = new Vector2(0.5f * ScreenWidth, 0);
					break;
			}
			if (AnchorMode == AnchorModes.Pixel)
				TransformCtrl.Position += new Vector2(OffsetX, OffsetY);
			else
				TransformCtrl.Position += new Vector2(TransformCtrl.Size.X * OffsetX * TransformCtrl.Scale.X, TransformCtrl.Size.Y * OffsetY * TransformCtrl.Scale.Y);
		}
	}
}
