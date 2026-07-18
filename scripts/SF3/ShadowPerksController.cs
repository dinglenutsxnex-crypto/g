using System.Collections.Generic;
using Godot;
using SF3.Items;

namespace SF3
{
	public partial class ShadowPerksController : Node
	{
		[Export] private ShadowPerksSlotsHolder _playerSlots;
		[Export] private ShadowPerksSlotsHolder _foeSlots;
		[Export] private float _readyAlpha = 0.4f;
		[Export] private float _notActiveAlpha = 0.2f;
		[Export] private float _fadeDuration = 0.3f;

		private Dictionary<int, ShadowPerksSlotsHolder> _slotsHolder;
		private bool _visible = true;

		private static ShadowPerksController _instance;
		public static float FadeDuration => _instance?._fadeDuration ?? 0.3f;

		public bool Visible
		{
			get => _visible;
			set
			{
				_visible = value;
				if (_playerSlots != null) _playerSlots.Visible = value;
				if (_foeSlots != null) _foeSlots.Visible = value;
			}
		}

		public override void _Ready()
		{
			_instance = this;
			_playerSlots?.Init();
			_foeSlots?.Init();
		}

		public void Init(SF3.GameModels.Model player, SF3.GameModels.Model foe)
		{
			_slotsHolder = new Dictionary<int, ShadowPerksSlotsHolder>
			{
				{ player.id, _playerSlots },
				{ foe.id, _foeSlots }
			};
			_slotsHolder[player.id].SetModel(player);
			_slotsHolder[foe.id].SetModel(foe);
			SetShadowPerksState(ShadowPerksState.Ready);
		}

		public void Invert(bool invert)
		{
			if (_playerSlots != null) _playerSlots.Inverted = invert;
			if (_foeSlots != null) _foeSlots.Inverted = !invert;
		}

		public void SetShadowPerksState(ShadowPerksState state)
		{
			SetShadowPerkState(1, state);
			SetShadowPerkState(2, state);
		}

		public void ClearShadowPerkSlot(int modelId, EquipmentType equipmentType)
		{
			if (_slotsHolder != null && _slotsHolder.TryGetValue(modelId, out var holder))
				holder.ClearShadowPerkSlot(equipmentType);
		}

		public void SetShadowPerkState(int modelId, ShadowPerksState state)
		{
			if (_slotsHolder != null && _slotsHolder.TryGetValue(modelId, out var holder))
				holder.SetShadowPerkState(state);
		}

		public void StartCooldown(int modelId, EquipmentType equipmentType, int framesDuration)
		{
			if (_slotsHolder != null && _slotsHolder.TryGetValue(modelId, out var holder))
				holder.StartCooldown(equipmentType, framesDuration);
		}

		public static float GetAlpha(ShadowPerksState state)
		{
			if (_instance == null) return 1f;
			switch (state)
			{
				case ShadowPerksState.Active:   return 1f;
				case ShadowPerksState.Ready:    return _instance._readyAlpha;
				case ShadowPerksState.NotActive: return _instance._notActiveAlpha;
				default: return 0f;
			}
		}
	}
}
