// ⚠️ STUB: needs full port — original used NGUI ImageWrapper, RectTransform, BehaviourTimer fill animation
using System;
using SF3.Items;
using Godot;

namespace SF3
{
	public partial class ShadowPerkSlot : Node
	{
		[Export]
		private Color _emptyColor;
		[Export]
		private Color _fullColor;
		[Export]
		private Color _arrowColor;
		[Export]
		private TextureRect _ability;
		[Export]
		private TextureRect _foreground;
		[Export]
		private TextureRect _background;
		[Export]
		private TextureRect _arrow;

		public void Init()
		{
		}

		public void SetPerk(string perkName)
		{
		}

		public void Cooldown(int framesDuration, EquipmentType type, Action onFinish = null)
		{
		}

		public void FlipArrowTexture(bool isFlipped)
		{
		}

		public void ClearPerk()
		{
			SetPerk(string.Empty);
		}
	}
}
