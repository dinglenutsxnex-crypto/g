using Godot;

public partial class SystemAlertButton : Node
{
	[Export]
	private LocalizationText label;

	public void SetLabel(string alias)
	{
		label.SetAlias(alias);
	}
}
