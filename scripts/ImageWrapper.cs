using Godot;

public partial class ImageWrapper : TextureRect
{
	public new Texture2D Texture
	{
		get
		{
			return base.Texture;
		}
		set
		{
			if (value != null)
			{
				if (base.Texture != null && !base.Texture.ResourceName.Equals(value.ResourceName))
				{
					TexturesUtils.ReleaseTexture(Texture);
				}
				base.Texture = value;
				TexturesUtils.AddTexture(value);
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (Texture != null)
		{
			TexturesUtils.AddTexture(Texture);
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (Texture != null)
		{
			TexturesUtils.ReleaseTexture(Texture);
		}
	}
}
