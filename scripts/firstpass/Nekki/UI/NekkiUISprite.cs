using Godot;
using System;

namespace Nekki.UI
{
	public partial class NekkiUISprite : TextureRect
	{
		private static ImageTexture _noImageTexture;
		private string mSpriteName;

		private static ImageTexture GetNoImageTexture
		{
			get
			{
				if (_noImageTexture == null)
				{
					Image img = Image.CreateEmpty(1, 1, false, Image.Format.Rgba8);
					img.Fill(Colors.Red);
					_noImageTexture = ImageTexture.CreateFromImage(img);
				}
				return _noImageTexture;
			}
		}

		public float Size_X => Size.X;
		public float Size_Y => Size.Y;

		public string SpriteName
		{
			get => mSpriteName;
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					Texture = GetNoImageTexture;
					mSpriteName = string.Empty;
				}
				else if (mSpriteName != value)
				{
					mSpriteName = value;
				}
			}
		}

		public void SetGrayscale(bool value) { }

		internal void Init()
		{
			if (Texture == null)
				Texture = GetNoImageTexture;
		}
	}
}
