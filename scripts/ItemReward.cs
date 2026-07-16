// ⚠️ STUB: needs full port — original used NGUI NekkiUISprite
using Godot;
using SF3.Items;
using SF3.Settings;

public partial class ItemReward : Node
{
	[Export]
	private TextureRect sprite;

	public void Init(BaseRewardInfo item)
	{
		SetRarable(item as IRarable);
	}

	private void SetRarable(IRarable item)
	{
		if (item != null)
		{
		}
	}
}
