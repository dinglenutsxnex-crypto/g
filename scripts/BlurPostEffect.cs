using Godot;

public partial class BlurPostEffect : Node
{
	[Export]
	public int blurIterations;

	[Export]
	public Material blurMaterial;

	private ImageTexture glowRenderTextureA;

	private ImageTexture glowRenderTextureB;

	private ImageTexture blurTexture;

	[Export]
	public int factor = 2;

	[Export]
	public float blurSpread = 1.25f;

	private bool _blurEnabled;

	private bool requestRenderGlow;

	public static BlurPostEffect Instance;

	public bool blurEnabled
	{
		get
		{
			return _blurEnabled;
		}
		set
		{
			if (!_blurEnabled && value)
			{
				requestRenderGlow = true;
			}
			_blurEnabled = value;
			ProcessMode = value ? ProcessModeEnum.Always : ProcessModeEnum.Disabled;
		}
	}

	// Godot equivalent of OnRenderImage — use CompositorEffect or custom shader solution
	public override void _Ready()
	{
		Instance = this;
		Vector2I screenSize = DisplayServer.WindowGetSize();
		blurTexture = ImageTexture.CreateFromImage(Image.Create(screenSize.X, screenSize.Y, false, Image.Format.Rgba8));
		blurEnabled = false;
	}
}
