using Godot;
using Nekki.Yaml;
using SF3.Moves;

public class TriggerActionShowDialog : TriggerAction
{
	public TriggerActionShowDialog(Node yamlNode) : base(EActionType.SHOW_DIALOG, yamlNode) { }

	protected override void ApplyAction(object modelData) { }
}
