using Nekki.Yaml;
using Node = Nekki.Yaml.Node;
namespace SF3.Moves
{
	public partial class TriggerActionHideTutorialPanel : TriggerAction
	{
		public TriggerActionHideTutorialPanel(Node yamlNode)
			: base(EActionType.HIDE_TUTORIAL_PANEL, yamlNode)
		{
		}
		protected override void ApplyAction(object modelData)
		{
			base.ApplyAction(modelData);
			TutorialManager.Instance.HidePanel();
		}
	}
}

