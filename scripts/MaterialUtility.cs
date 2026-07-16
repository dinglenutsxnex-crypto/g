using System;
using Godot;

public static class MaterialUtility
{
	public enum RenderType
	{
		Opaque = 0,
		Transparent = 1,
		Overlay = 2,
		Glow = 3
	}

	public enum Queue
	{
		Geometry = 0,
		Transparent = 1,
		Overlay = 2
	}

	public enum FogType
	{
		None = 0,
		Linear = 1,
		LinearWithYBias = 2
	}

	public enum CharacterSkinType
	{
		Weapon = 0,
		Armor = 1,
		Head = 2,
		Trail = 3
	}

	public enum MaterialBlendType
	{
		None = 0,
		AlphaBlending = 1,
		Additive = 2,
		Multiplicative = 3
	}

	public static class EditorUtility
	{
		public const string useSFKeyword = "SF";

		public const string useFogKeyword = "USE_FOG";

		public const string useFogbyYKeyword = "USE_FOG_BY_Y";

		public const string cull = "_Cull";

		public const string useSkinColorKeyword = "USE_SKIN_COLOR";

		public const string ignoreLightKeyword = "IGNORE_LIGHT";

		public const string materialType = "_MaterialType";

		public const string fogStart = "_FogStart";

		public const string fogEnd = "_FogEnd";

		public const string metalPower = "_MetalPower";

		public const string rimMaterialPower = "_RimMaterialPower";

		public const string maskSize = "_MaskSize";

		public const string glowEffectUseMaskBKeyword = "GLOWEFFECT_USE_MASK_B";

		public const string glowEffectUseMaskRKeyword = "GLOWEFFECT_USE_MASK_R";

		public const string glowColor = "_GlowColor";

		public const string glowTexture = "_GlowTex";

		public const string useGlowTextureKeyword = "GLOWEFFECT_USE_GLOWTEX";

		public const string alphaTexture = "_AlphaTex";

		public const string maskStartColor = "_MaskStartColor";

		public const string maskEndColor = "_MaskEndColor";

		public const string useAlphaFromLightmap = "ALPHA_FROM_LM";

		public const string useMatcapLocal = "USE_MATCAP_LOCAL";

		public const string blendByTexture = "BLEND_BY_TEXTURE";

		public const string useDetail = "USE_DETAIL";

		public const string ignoreFog = "IGNORE_FOG_DF";

		public const string useVertexAnimation = "USE_VERTEX_ANIM";

		public const string lightmapDefault = "_LightMapDefault";

		public const string lightmapShadowForm = "_LightMapShadowForm";

		public const string decalTexture = "_DecalTex";

		public const string offset = "_Offset";

		public const string vertexAnimationFactorX = "_VertexAnimationFactorX";

		public const string vertexAnimationFactorY = "_VertexAnimationFactorY";

		public const string preRenderKeyword = "PRE_RENDER";

		public const string animateUV = "ANIMATE_UV";

		public const string scrollX = "_ScrollX";

		public const string scrollY = "_ScrollY";

		public const string speedMultiplier = "_SpeedMultipler";

		public const string sineAmplX = "_SineAmplX";

		public const string sineAmplY = "_SineAmplY";

		public const string sineFrequencyX = "_SineFreqX";

		public const string sineFrequencyY = "_SineFreqY";

		public const string color = "_Color";

		public const string shadowFormProperty = "_ShadowFormEnabled";

		public const string useSkinColor = "_UseSkinColor";
	}

	public static class CharacterMaterial
	{
		public static void SetMaterialKeywords(Material material, CharacterSkinType type)
		{
			SetKeywordByProperty(material, "_ShadowFormEnabled", "SF");
			SetKeywordByProperty(material, "_IgnoreLight", "IGNORE_LIGHT");
			SetFogType(material, (FogType)GetInt(material, "_FogType"));
			SetKeywordsByType(material, type);
		}

		private static void SetKeywordsByType(Material mat, CharacterSkinType type)
		{
			bool flag = false;
			DisableAllKeywords(mat);
			switch (type)
			{
			case CharacterSkinType.Armor:
				EnableKeyword(mat, "USE_SKIN_COLOR");
				break;
			case CharacterSkinType.Weapon:
				SetEmission(mat, true);
				flag = true;
				break;
			case CharacterSkinType.Head:
				EnableKeyword(mat, "USE_SKIN_COLOR");
				EnableKeyword(mat, "USE_GRADIENT");
				SetEmission(mat, false);
				flag = true;
				break;
			case CharacterSkinType.Trail:
				SetEmission(mat, true);
				EnableKeyword(mat, "USE_PER_VERTEX_ALPHA");
				flag = true;
				break;
			}
			if (type == CharacterSkinType.Trail)
			{
				SetTransparent(mat);
			}
			else
			{
				SetOpaque(mat);
			}
			if (flag)
			{
				SetRenderType(mat, RenderType.Glow);
			}
		}

		private static void DisableAllKeywords(Material mat)
		{
			DisableKeyword(mat, "USE_SKIN_COLOR");
			DisableKeyword(mat, "USE_GRADIENT");
			DisableKeyword(mat, "USE_PER_VERTEX_ALPHA");
			DisableKeyword(mat, "GLOWEFFECT_USE_MASK_B");
			DisableKeyword(mat, "USE_B_AS_WEAPON_COLOR");
			DisableKeyword(mat, "GLOWEFFECT_USE_MASK_R");
			DisableKeyword(mat, "USE_R_AS_WEAPON_COLOR");
		}

		private static void SetTransparent(Material mat)
		{
			SetMaterialBlend(mat, MaterialBlendType.AlphaBlending);
			SetUseZBuffer(mat, false);
			SetRenderQueue(mat, 2251);
			SetRenderType(mat, RenderType.Transparent);
		}

		private static void SetOpaque(Material mat)
		{
			SetMaterialBlend(mat, MaterialBlendType.None);
			SetUseZBuffer(mat, true);
			SetRenderQueue(mat, 2000);
			SetRenderType(mat, RenderType.Opaque);
		}

		private static void SetEmission(Material mat, bool fromBlue)
		{
			if (fromBlue)
			{
				EnableKeyword(mat, "GLOWEFFECT_USE_MASK_B");
				EnableKeyword(mat, "USE_B_AS_WEAPON_COLOR");
			}
			else
			{
				EnableKeyword(mat, "GLOWEFFECT_USE_MASK_R");
				EnableKeyword(mat, "USE_R_AS_WEAPON_COLOR");
			}
		}

		private static void SetKeywordByProperty(Material material, string propertyName, string keywordName)
		{
			if (GetInt(material, propertyName) == 1)
			{
				EnableKeyword(material, keywordName);
			}
			else
			{
				DisableKeyword(material, keywordName);
			}
		}

		private static void SetFogType(Material mat, FogType type)
		{
			switch (type)
			{
			case FogType.None:
				DisableKeyword(mat, "USE_FOG");
				DisableKeyword(mat, "USE_FOG_BY_Y");
				break;
			case FogType.Linear:
				EnableKeyword(mat, "USE_FOG");
				DisableKeyword(mat, "USE_FOG_BY_Y");
				break;
			case FogType.LinearWithYBias:
				DisableKeyword(mat, "USE_FOG");
				EnableKeyword(mat, "USE_FOG_BY_Y");
				break;
			}
		}
	}

	public const string maskEnemyColor = "_MaskEnemyColor";

	public const string maskPlayerColor = "_MaskPlayerColor";

	public const string maskColor = "_MaskColor";

	public const string useAlphaKeyword = "USE_ALPHA";

	public const string useGradientKeyword = "USE_GRADIENT";

	public const string gradientStartColor = "_GradientStartColor";

	public const string gradientEndColor = "_GradientEndColor";

	public const string gradientStart = "_GradientStart";

	public const string gradientEnd = "_GradientEnd";

	public const string matCap = "_MatCap";

	public const string mask = "_Mask";

	public const string skinColor = "_SkinColor";

	public const string skinColorBase = "_SkinColorBase";

	public const string skinColorDifference = "_SkinColorDifference";

	public const string mainColor = "_MainColor";

	public const string usePerVertexAlphaKeyword = "USE_PER_VERTEX_ALPHA";

	public const string blendValue = "_BlendValue";

	public const string dissolveBlendValue = "_DissolveBlendValue";

	public const string mainTex = "_MainTex";

	public const string shadowFromTex = "_ShadowForm";

	public const string mainTexPreRender = "_MainTexPreRender";

	public const string shadowFromTexPreRender = "_ShadowformPreRender";

	public const string needsPreRenderKeyword = "NEEDS_PRERENDER";

	public const string preRenderForShadowformKeyword = "PRERENDER_FOR_SF";

	public const string useRAsWeaponColorKeyword = "USE_R_AS_WEAPON_COLOR";

	public const string useBAsWeaponColorKeyword = "USE_B_AS_WEAPON_COLOR";

	public const string useDissolveKeyword = "SF_USE_DISSOLVE";

	public const string sourceBlend = "_SrcBlend";

	public const string destinationBlend = "_DstBlend";

	public const string zTest = "_ZTest";

	private const string zWrite = "_ZWrite";

	public const string lowQuality = "LOW_Q";

	public const string mediumQuality = "MED_Q";

	public const string highQuality = "HIGH_Q";

	public const string ultraQuality = "ULTRA_Q";

	public const string fogType = "_FogType";

	public const string ignoreLightProperty = "_IgnoreLight";

	public const string effectTransition = "_EffectTransition";

	private static ShaderMaterial AsShader(Material mat)
	{
		return mat as ShaderMaterial;
	}

	private static void EnableKeyword(Material mat, string keyword)
	{
		ShaderMaterial sm = AsShader(mat);
		if (sm != null)
		{
			sm.SetShaderParameter(keyword, true);
		}
	}

	private static void DisableKeyword(Material mat, string keyword)
	{
		ShaderMaterial sm = AsShader(mat);
		if (sm != null)
		{
			sm.SetShaderParameter(keyword, false);
		}
	}

	public static int GetInt(Material mat, string name)
	{
		ShaderMaterial sm = AsShader(mat);
		if (sm != null)
		{
			Variant val = sm.GetShaderParameter(name);
			if (val.VariantType == Variant.Type.Int)
				return val.AsInt32();
			if (val.VariantType == Variant.Type.Float)
				return (int)val.AsSingle();
		}
		return 0;
	}

	public static void SetInt(Material mat, string name, int value)
	{
		ShaderMaterial sm = AsShader(mat);
		if (sm != null)
		{
			sm.SetShaderParameter(name, value);
		}
	}

	public static void SetMaterialBlend(Material mat, MaterialBlendType type)
	{
		switch (type)
		{
		case MaterialBlendType.None:
			SetInt(mat, "_SrcBlend", 1);
			SetInt(mat, "_DstBlend", 0);
			break;
		case MaterialBlendType.Additive:
			break;
		case MaterialBlendType.AlphaBlending:
			SetInt(mat, "_SrcBlend", 5);
			SetInt(mat, "_DstBlend", 10);
			break;
		case MaterialBlendType.Multiplicative:
			SetInt(mat, "_SrcBlend", 2);
			SetInt(mat, "_DstBlend", 0);
			break;
		}
	}

	public static bool GetUseZBuffer(Material mat)
	{
		return GetInt(mat, "_ZWrite") == 1;
	}

	public static void SetUseZBuffer(Material mat, bool useZBuffer)
	{
		SetInt(mat, "_ZWrite", useZBuffer ? 1 : 0);
	}

	public static void SetRenderType(Material mat, RenderType type)
	{
		if (mat != null)
			mat.SetMeta("RenderType", type.ToString());
	}

	public static RenderType GetRenderType(Material mat)
	{
		if (mat != null && mat.HasMeta("RenderType"))
		{
			string tag = mat.GetMeta("RenderType").AsString();
			return (RenderType)Enum.Parse(typeof(RenderType), tag);
		}
		return RenderType.Opaque;
	}

	public static void SetQueue(Material mat, Queue queue)
	{
		if (mat != null)
			mat.SetMeta("Queue", queue.ToString());
	}

	public static void SetRenderQueue(Material mat, int queueValue)
	{
		if (mat is BaseMaterial3D bm3d)
			bm3d.RenderPriority = queueValue;
	}
}
