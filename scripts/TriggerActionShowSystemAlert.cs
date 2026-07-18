using Nekki.Yaml;
using Node = Nekki.Yaml.Node;
using SF3.Moves;
public partial class TriggerActionShowSystemAlert : TriggerActionQuest
{
	private readonly Mapping _data;
	public TriggerActionShowSystemAlert(Node yamlNode)
		: base(EActionType.SHOW_SYSTEM_ALERT, yamlNode, false)
	{
		_data = ((Mapping)yamlNode).GetMapping("SystemAlert");
	}
	protected override void ApplyAction(object modelData)
	{
		base.ApplyAction(modelData);
		SystemAlertManager.Show(_data, base.CloseAction);
	}
}

