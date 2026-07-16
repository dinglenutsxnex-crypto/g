using SF3;
public partial class DojoHolderModule : FightHolderModule
{
	protected void OpenModule(IntentModule intent)
	{
		if (!FightHolderModule.CreateDojo)
		{
			CurrencyUI.SetActive(true);
			base.OpenModule(intent);
		}
		else
		{
			OpenedCallback();
		}
		FightHolderModule.CreateDojo = true;
	}
	protected void OpenedCallback()
	{
		CurrencyUI.SetActive(true);
		BattleCamera3D.MoveToDojo(base.OpenedCallback, base.Intent.IsInstant());
	}
	protected void EnterDarkPocket()
	{
	}
	protected void ExitDarkPocket()
	{
	}
}

