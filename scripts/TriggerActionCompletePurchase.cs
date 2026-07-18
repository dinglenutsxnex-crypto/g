using Nekki.Yaml;
using Node = Nekki.Yaml.Node;
using SF3.Items;
using SF3.Moves;
public partial class TriggerActionCompletePurchase : TriggerActionQuest
{
	public TriggerActionCompletePurchase(Node yamlNode)
		: base(EActionType.SELECT_BATTLE, yamlNode)
	{
	}
	protected override void ApplyAction(object modelData)
	{
		base.ApplyAction(modelData);
		ItemBuyingController.Instance.CompleteCurrentTransaction();
		ShopTransaction.Clear();
	}
}

