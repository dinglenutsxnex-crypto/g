using Godot;

namespace SF3
{
    [Tool]
    public class GlowEffectShadowform : Node3D
    {
        public enum GlowMode { Glow, AlphaGlow, SimpleGlow, SimpleAlphaGlow }
        public enum BlendMode { Additive, Multiply, Screen, Subtract }

        [Export] public Material glowMaterial;
        [Export] public bool useDissolveGlow;
        [Export] public GlowMode glowMode;
        [Export] public BlendMode blendMode;
        [Export] public int downsampleSize = 256;
        [Export] public int blurIterations = 4;
        [Export] public float blurSpread = 1f;
        [Export] public float glowStrength = 1.2f;
        [Export] public Color glowColorMultiplier = Colors.White;

        public int BlurIterations { get => blurIterations; set => blurIterations = value; }
        public float BlurSpread { get => blurSpread; set { blurSpread = value; UpdateGlowMaterial(); } }
        public float GlowStrength { get => glowStrength; set { glowStrength = value; UpdateGlowMaterial(); } }
        public Color GlowColorMultiplier { get => glowColorMultiplier; set { glowColorMultiplier = value; UpdateGlowMaterial(); } }

        public void UpdateGlowMaterial()
        {
            if (glowMaterial is ShaderMaterial sm)
            {
                sm.SetShaderParameter("blur_spread", blurSpread);
                sm.SetShaderParameter("glow_strength", glowStrength);
                sm.SetShaderParameter("glow_color_multiplier", glowColorMultiplier);
            }
        }
    }
}
