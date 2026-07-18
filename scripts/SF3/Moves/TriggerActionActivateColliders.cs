using Nekki.Yaml;
using Node = Nekki.Yaml.Node;
using SF3.GameModels;
namespace SF3.Moves
{
	public partial class TriggerActionActivateColliders : TriggerAction
	{
		public TriggerActionActivateColliders(Node yamNode)
			: base(EActionType.ACTIVATE_COLLIDERS, yamNode)
		{
		}
		protected override void ApplyAction(object modelData)
		{
			((Model)modelData).modelComponents.modelPhysics.EnableColliders(true);
		}
	}
}

