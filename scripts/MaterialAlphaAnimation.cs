using Godot;

public partial class MaterialAlphaAnimation : Node3D
{
	public enum AnimationStyle
	{
		Loop,
		PingPong
	}

	[Export]
	public float animationTime;
	[Export]
	public Curve animationCurve;
	private float timer;
	[Export]
	public string materialColorProperty;
	private Material material;
	private float alphaValue;
	private Color matColor;
	[Export]
	public AnimationStyle animationStyle;
	private bool backAnimation;

	public override void _Ready()
	{
		timer = 0f;
		backAnimation = false;
		MeshInstance3D mesh = GetNodeOrNull<MeshInstance3D>(".");
		if (mesh != null)
		{
			material = mesh.MaterialOverride;
		}
		if (materialColorProperty.Length > 0)
		{
			materialColorProperty = materialColorProperty.Trim();
			if (materialColorProperty[0] != '_')
			{
				materialColorProperty = "_" + materialColorProperty;
			}
		}
	}

	public override void _Process(double delta)
	{
		timer += GameTimeController.deltaTime;
		alphaValue = animationCurve.Sample((!backAnimation) ? (timer / animationTime) : (1f - timer / animationTime));
		if (material != null)
		{
			Color c = (Color)material.GetShaderParameter(materialColorProperty);
			c.A = alphaValue;
			material.SetShaderParameter(materialColorProperty, c);
		}
		if (timer >= animationTime)
		{
			if (animationStyle == AnimationStyle.PingPong)
			{
				backAnimation = !backAnimation;
			}
			else
			{
				backAnimation = false;
			}
			timer = 0f;
		}
	}
}
