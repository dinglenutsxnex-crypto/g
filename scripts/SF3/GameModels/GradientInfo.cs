using System;
using Godot;

namespace SF3.GameModels
{
	[Serializable]
	public partial class GradientInfo
	{
		public Color gradientStartColor;
		public Color gradientEndColor;
		public float gradientStart;
		public float gradientEnd;
	}
}
