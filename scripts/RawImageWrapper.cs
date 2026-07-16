using Godot;

public partial class RawImageWrapper : TextureRect
{
	public new Texture2D Texture
	{
		get => base.Texture;
		set
		{
			if (value != null)
			{
				if (base.Texture != null && !base.Texture.ResourceName.Equals(value.ResourceName))
				{
					TexturesUtils.ReleaseTexture(Texture);
				}
				base.Texture = value;
				TexturesUtils.AddTexture(Texture);
			}
		}
	}

	public override void _Ready()
	{
		if (Texture != null)
		{
			TexturesUtils.AddTexture(Texture);
		}
	}

	public override void _ExitTree()
	{
		if (Texture != null)
		{
			TexturesUtils.ReleaseTexture(Texture);
		}
	}
}
