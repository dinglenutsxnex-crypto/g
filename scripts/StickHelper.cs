using System.Collections.Generic;
using Godot;
using SF3.GameModels;
using SF3.Items;

public partial class StickHelper : Node
{
	private bool _isShadowHint;
	private bool _isInverced;
	private bool _isDisarmed;

	private static StickHelper _instance;
	public static StickHelper Instance => _instance;

	private Dictionary<EquipmentType, EQuadrants> shadowQuadrantsNorm = new Dictionary<EquipmentType, EQuadrants>
	{
		{ EquipmentType.Helmet, EQuadrants.One },
		{ EquipmentType.Armor,  EQuadrants.Five },
		{ EquipmentType.Weapon, EQuadrants.Three },
		{ EquipmentType.Ranged, EQuadrants.Seven }
	};

	private Dictionary<EquipmentType, EQuadrants> shadowQuadrantsInv = new Dictionary<EquipmentType, EQuadrants>
	{
		{ EquipmentType.Helmet, EQuadrants.One },
		{ EquipmentType.Armor,  EQuadrants.Five },
		{ EquipmentType.Weapon, EQuadrants.Seven },
		{ EquipmentType.Ranged, EQuadrants.Three }
	};

	private Dictionary<EquipmentType, int> _cooldownsId = new Dictionary<EquipmentType, int>
	{
		{ EquipmentType.Helmet, 0 },
		{ EquipmentType.Armor,  1 },
		{ EquipmentType.Weapon, 2 },
		{ EquipmentType.Ranged, 3 }
	};

	private bool[] _cooldowns = new bool[4];
	private Dictionary<EquipmentType, EQuadrants> shadowQuadrants;

	public override void _Ready()
	{
		_instance = this;
		shadowQuadrants = shadowQuadrantsNorm;
	}

	public void ClearDisarmCooldowns()
	{
		_isDisarmed = false;
		foreach (EquipmentType key in _cooldownsId.Keys)
			_cooldowns[_cooldownsId[key]] = false;
		if (_isShadowHint) SetHints();
	}

	public void SetDisarm()
	{
		_isDisarmed = true;
		if (_isShadowHint) SetHints();
	}

	public void UpdateDirection(bool inverte)
	{
		if (_isInverced != inverte)
		{
			_isInverced = inverte;
			shadowQuadrants = _isInverced ? shadowQuadrantsInv : shadowQuadrantsNorm;
			if (_isShadowHint) SetHints();
		}
	}

	public void ShowShadowHint()
	{
		if (!_isShadowHint)
		{
			_isShadowHint = true;
			SetHints();
		}
	}

	private void SetHints()
	{
		// Requires ModelsManager and Stick — stub until those are ported
		GD.Print("StickHelper.SetHints: not fully implemented yet");
	}

	public void HideShadowHint()
	{
		_isShadowHint = false;
		foreach (EquipmentType key in shadowQuadrants.Keys)
			_cooldowns[_cooldownsId[key]] = false;
	}

	public void SetShadowHintByType(EquipmentType type, bool active)
	{
		if (!_isShadowHint)
		{
			_cooldowns[_cooldownsId[type]] = false;
			return;
		}
		_cooldowns[_cooldownsId[type]] = !active;
	}
}
