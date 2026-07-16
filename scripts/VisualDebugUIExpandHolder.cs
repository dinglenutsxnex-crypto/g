using Godot;

public class VisualDebugUIExpandHolder : Node
{
	private VisualDebugUI.Unit _unit;

	public void SetUnit(VisualDebugUI.Unit unit)
	{
		_unit = unit;
	}

	public override void _Ready()
	{
	}

	private void OnClick()
	{
		VisualDebugUI.Instance.OnExpand(_unit);
	}
}
