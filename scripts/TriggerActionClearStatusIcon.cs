using Godot;
using Nekki.Yaml;
using Nekki.Yaml;
using Node = Nekki.Yaml.Node;
using SF3.GameModels;
using SF3.Moves;

public class TriggerActionClearStatusIcon : TriggerAction
{
	public TriggerActionClearStatusIcon(Node yamlNode)
		: base(EActionType.CLEAR_STATUS_ICON, yamlNode)
	{
	}

	protected override void ApplyAction(object modelData)
	{
		PerkCarousel.ClearStatusIcon(((Model)modelData).id, this);
	}
}
