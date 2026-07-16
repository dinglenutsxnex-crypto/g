using SF3.Items;
public partial class ReelDriverComparerDesc : ReelDriverComparer
{
	protected override int CompareEquipment(Equipment first, Equipment second)
	{
		int num = base.CompareEquipment(first, second);
		return -num;
	}
	protected override int ComparePerks(Perk first, Perk second)
	{
		int num = base.ComparePerks(first, second);
		return -num;
	}
}

