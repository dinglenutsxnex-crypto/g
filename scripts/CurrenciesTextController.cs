using Godot;
using System;
using SF3.UserData;
using sf3DTO;

public partial class CurrenciesTextController : Node
{
	public Label coinsBalanceLbl;
	public Label bonusBalanceLbl;

	public override void _Ready()
	{
		UserManager.AddActionForCurrency(CurrencyType.Coin, UpdateCoinsLbl);
		UserManager.AddActionForCurrency(CurrencyType.Bonus, UpdateBonusLbl);
		UpdateCoinsLbl(UserManager.GetCurrencyValue(CurrencyType.Coin));
		UpdateBonusLbl(UserManager.GetCurrencyValue(CurrencyType.Bonus));
	}

	private void UpdateCoinsLbl(long coins)
	{
		coinsBalanceLbl.Text = coins.ToString();
	}

	private void UpdateBonusLbl(long bonus)
	{
		bonusBalanceLbl.Text = bonus.ToString();
	}

	public override void _ExitTree()
	{
		UserManager.RemoveActionForCurrency(CurrencyType.Coin, UpdateCoinsLbl);
		UserManager.RemoveActionForCurrency(CurrencyType.Bonus, UpdateBonusLbl);
	}
}
