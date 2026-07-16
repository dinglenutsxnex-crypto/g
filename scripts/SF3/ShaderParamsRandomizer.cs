using System;
using System.Collections.Generic;
using Godot;

namespace SF3
{
	public partial class ShaderParamsRandomizer : Node3D
	{
		[Serializable]
		public partial class RandomizeIntProp
		{
			public string name;
			public int value1;
			public int value2;

			public int GetRandom()
			{
				return (int)GD.RandRange(value1, value2);
			}

			public RandomizeIntProp Clone()
			{
				return MemberwiseClone() as RandomizeIntProp;
			}
		}

		[Serializable]
		public partial class RandomizeTextureProp
		{
			public string name;
			public Vector2 tiling_value1;
			public Vector2 tiling_value2;
			public Vector2 offset_value1;
			public Vector2 offset_value2;

			public Vector2 GetRandomTiling()
			{
				Vector2 result = default(Vector2);
				result.X = GD.RandRange(tiling_value1.X, tiling_value2.X);
				result.Y = GD.RandRange(tiling_value1.Y, tiling_value2.Y);
				return result;
			}

			public Vector2 GetRandomOffset()
			{
				Vector2 result = default(Vector2);
				result.X = GD.RandRange(offset_value1.X, offset_value2.X);
				result.Y = GD.RandRange(offset_value1.Y, offset_value2.Y);
				return result;
			}

			public RandomizeTextureProp Clone()
			{
				return MemberwiseClone() as RandomizeTextureProp;
			}
		}

		[Serializable]
		public partial class RandomizeFloatProp
		{
			public string name;
			public float value1;
			public float value2;

			public float GetRandom()
			{
				return GD.RandRange(value1, value2);
			}

			public RandomizeFloatProp Clone()
			{
				return MemberwiseClone() as RandomizeFloatProp;
			}
		}

		[Serializable]
		public partial class RandomizeVectorProp
		{
			public string name;
			public Vector4 value1;
			public Vector4 value2;

			public Vector4 GetRandom()
			{
				Vector4 result = default(Vector4);
				result.X = GD.RandRange(value1.X, value2.X);
				result.Y = GD.RandRange(value1.Y, value2.Y);
				result.Z = GD.RandRange(value1.Z, value2.Z);
				result.W = GD.RandRange(value1.W, value2.W);
				return result;
			}

			public RandomizeVectorProp Clone()
			{
				return MemberwiseClone() as RandomizeVectorProp;
			}
		}

		[Serializable]
		public partial class RandomizeColorProp
		{
			public string name;
			public Color value1;
			public Color value2;

			public Color GetRandom()
			{
				return value1.Lerp(value2, GD.RandRange(0f, 1f));
			}

			public RandomizeColorProp Clone()
			{
				return MemberwiseClone() as RandomizeColorProp;
			}
		}

		[Export]
		public bool randomizeOnEnable = true;
		public List<RandomizeIntProp> intProperties = new List<RandomizeIntProp>();
		public List<RandomizeFloatProp> floatProperties = new List<RandomizeFloatProp>();
		public List<RandomizeColorProp> colorProperties = new List<RandomizeColorProp>();
		public List<RandomizeVectorProp> vectorProperties = new List<RandomizeVectorProp>();
		public List<RandomizeTextureProp> textureProperties = new List<RandomizeTextureProp>();
		public ShaderMaterial currentMaterial;
		private bool _isInited;

		public void Randomize()
		{
			if (!_isInited)
			{
				MeshInstance3D mesh = GetNodeOrNull<MeshInstance3D>(".");
				if (mesh != null)
				{
					currentMaterial = mesh.MaterialOverride as ShaderMaterial;
				}
				_isInited = true;
			}
			if (currentMaterial == null)
			{
				return;
			}
			foreach (RandomizeIntProp intProperty in intProperties)
			{
				currentMaterial.SetShaderParameter(intProperty.name, intProperty.GetRandom());
			}
			foreach (RandomizeFloatProp floatProperty in floatProperties)
			{
				currentMaterial.SetShaderParameter(floatProperty.name, floatProperty.GetRandom());
			}
			foreach (RandomizeColorProp colorProperty in colorProperties)
			{
				currentMaterial.SetShaderParameter(colorProperty.name, colorProperty.GetRandom());
			}
			foreach (RandomizeVectorProp vectorProperty in vectorProperties)
			{
				currentMaterial.SetShaderParameter(vectorProperty.name, vectorProperty.GetRandom());
			}
			foreach (RandomizeTextureProp textureProperty in textureProperties)
			{
				currentMaterial.SetShaderParameter(textureProperty.name + "_offset", textureProperty.GetRandomOffset());
				currentMaterial.SetShaderParameter(textureProperty.name + "_scale", textureProperty.GetRandomTiling());
			}
		}

		public override void _EnterTree()
		{
			if (randomizeOnEnable)
			{
				Randomize();
			}
		}
	}
}
