using Nekki.Yaml;
using Node = Nekki.Yaml.Node;
using SF3.Moves;
using SF3.UserData;
using Godot;
public partial class TriggerActionRemoveUserVariable : TriggerActionQuest
{
	[Export]
	public string VariableName;
	public TriggerActionRemoveUserVariable(Node node)
		: base(EActionType.REMOVE_USER_VARIABLE, node)
	{
		TryGetString(out VariableName, "Name", string.Empty, string.Empty, null, false);
	}
	protected override void ApplyAction(object modelData)
	{
		base.ApplyAction(modelData);
		UserManager.RemoveGlobalVariable(VariableName.AsRpn());
	}
}

