// ⚠️ STUB: needs full port — original used NGUI UITable, NGUITools.AddChild, Object.Destroy
using System.Collections.Generic;
using SF3_Attributes;
using Godot;

namespace SF3
{
	public partial class AttributesDrawer : Control
	{
		[Export]
		private Node attributePrf;
		[Export]
		private Color _iconColor;
		[Export]
		private Color _valueColor;
		[Export]
		private Color _arrowColor;

		public void Draw(SortedDictionary<AttributeType, float> attrs, SortedDictionary<AttributeType, float> playerAttrs = null)
		{
		}

		public override void _Ready()
		{
		}
	}
}
