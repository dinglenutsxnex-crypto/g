using Nekki.Yaml;
using Node = Nekki.Yaml.Node;
using SF3.Moves;
public partial class TriggerActionCallEvent : TriggerActionQuest
{
	private readonly string _id;
	public TriggerActionCallEvent(Node yamlNode)
		: base(EActionType.CALL_EVENT, yamlNode)
	{
		TryGetString(out _id, "ID", string.Empty, string.Empty, null, false);
	}
	protected override void ApplyAction(object modelData)
	{
		base.ApplyAction(modelData);
		QuestController.Instance.CallEvent(_id);
	}
}

