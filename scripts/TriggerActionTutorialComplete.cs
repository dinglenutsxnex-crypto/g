using Nekki.Yaml;
using Node = Nekki.Yaml.Node;
using SF3.Moves;
using SF3.UserData;
public partial class TriggerActionTutorialComplete : TriggerActionQuest
{
	public TriggerActionTutorialComplete(Node node)
		: base(EActionType.TUTORIAL_COMPLETE, node)
	{
	}
	protected override void ApplyAction(object modelData)
	{
		base.ApplyAction(modelData);
		UserManager.TutorialComplete();
	}
}

