using Godot;
using System;
using System.Collections.Generic;

public class VisualDebugUiUnit : Node
{
	private static List<VisualDebugUiUnit> _units = new List<VisualDebugUiUnit>();

	public UIButton btnExpand;
	public UIButton btnUnExpand;
	public UIButton btnUnit;
	public UILabel lblUnitName;

	private VisualDebugUI.Unit _unit;

	public static void Clear()
	{
		foreach (VisualDebugUiUnit unit in _units)
		{
			unit.QueueFree();
		}
		_units.Clear();
	}

	public void SetUnit(VisualDebugUI.Unit unit)
	{
		_unit = unit;
		_units.Add(this);
	}

	public void OnClick()
	{
		VisualDebugUI.Instance.OnSelect(_unit);
	}
}
