using Godot;
using System;

namespace Nekki.UI
{
    public class NekkiUISprite : TextureRect
    {
        private static Texture2D _noImageTexture;
        private string mSpriteName;

        private static Texture2D GetNoImageTexture
        {
            get
            {
                if (_noImageTexture == null)
                {
                    _noImageTexture = new Texture2D(1, 1);
                    _noImageTexture.SetPixel(0, 0, Colors.Red);
                    _noImageTexture.Apply();
                }
                return _noImageTexture;
            }
        }

        public string SpriteName
        {
            get { return mSpriteName; }
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
