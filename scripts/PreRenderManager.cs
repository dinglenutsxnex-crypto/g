using System.Collections.Generic;
using Godot;

public partial class PreRenderManager : Node
{
	public static PreRenderManager Instance;

	private int _listPointer;
	private List<ImageTexture> _preRenderedTexturesPool;
	private List<ImageTexture> _preRenderedTexturesUsed;
	private Rid _preRenderShader;

	public override void _Ready()
	{
		Instance = this;
		_preRenderedTexturesPool = new List<ImageTexture>();
		_preRenderedTexturesUsed = new List<ImageTexture>();
	}

	public ImageTexture PreRenderTexture(Material mat, string textureName)
	{
		Texture2D texture = mat.GetShaderParameter(textureName).As<Texture2D>();
		if (texture == null)
		{
			GD.PushWarning("Missing texture " + textureName + " " + mat.ResourceName);
			return null;
		}
		ImageTexture rt = GetTextureFromPool((int)texture.GetWidth());
		return rt;
	}

	private ImageTexture GetTextureFromPool(int size)
	{
		ImageTexture rt = null;
		for (int i = 0; i < _preRenderedTexturesPool.Count; i++)
		{
			if (_preRenderedTexturesPool[i].GetWidth() == size)
			{
				rt = _preRenderedTexturesPool[i];
				_preRenderedTexturesPool.RemoveAt(i);
				break;
			}
		}
		if (rt == null)
		{
			Image image = Image.CreateEmpty(size, size, false, Image.Format.Rgba8);
			rt = ImageTexture.CreateFromImage(image);
		}
		_preRenderedTexturesUsed.Add(rt);
		return rt;
	}

	public void ReturnTexture(ImageTexture texture)
	{
		if (_preRenderedTexturesUsed.Contains(texture))
		{
			_preRenderedTexturesUsed.Remove(texture);
			_preRenderedTexturesPool.Add(texture);
		}
	}
}
