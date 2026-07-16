using System;
using Godot;
namespace SF3.Items
{
	[Serializable]
	public partial class AtlasForType
	{
		[Export]
		private ShopCategoryType _subType;
		[Export]
		private Texture2D _atlas;
		public ShopCategoryType SubType
		{
			get
			{
				return _subType;
			}
		}
		public Texture2D Atlas
		{
			get
			{
				return _atlas;
			}
		}
	}
}

