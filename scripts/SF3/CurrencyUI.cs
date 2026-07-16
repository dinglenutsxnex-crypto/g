using SF3.UserData;
using Godot;
using sf3DTO;
namespace SF3
{
	public partial class CurrencyUI : UIModuleHolder
	{
		private const float animationCurrencyDuration = 1f;
		[Export]
		private Label bonusLbl;
		[Export]
		private Label coinsLbl;
		[Export]
		private Label shadowsLbl;
		[Export]
		private Label level;
		[Export]
		public Label levelProgress;
		[Export]
		public Label userName;
		[Export]
		private RectNode3D rectNode3D;
		private static CurrencyUI _instance;
		private SpriteAlphaAnimationPlayer spriteAlphaAnimationPlayer;
		private bool isShowing;
		public static CurrencyUI instance
		{
			get
			{
				return _instance;
			}
		}
		public float height
		{
			get
			{
				return rectNode3D.rect.height;
			}
		}
		public static float GetHeight()
		{
			if (_instance != null)
			{
				return 0f;
			}
			return 0f;
		}
		public static void SetActive(bool value)
		{
			if (_instance != null)
			{
				if (value)
				{
					_instance.Show();
				}
				else
				{
					_instance.Hide();
				}
			}
		}
		public override void Initialize()
		{
			_instance = this;
			isShowing = true;
			SetUserName(UserManager.GetName());
			SetBonusText(UserManager.GetCurrencyValue(CurrencyType.Bonus), false);
			SetCoinsText(UserManager.GetCurrencyValue(CurrencyType.Coin), false);
			SetShadowsText(UserManager.GetCurrencyValue(CurrencyType.Shadow), false);
			UpdateLevelAndExperience(0L);
			UserManager.AddActionForCurrency(CurrencyType.Coin, SetCoinsText);
			UserManager.AddActionForCurrency(CurrencyType.Bonus, SetBonusText);
			UserManager.AddActionForCurrency(CurrencyType.Shadow, SetShadowsText);
			UserManager.OnExperienceChanged += UpdateLevelAndExperience;
		}
		protected override void _ExitTree()
		{
			UserManager.RemoveActionForCurrency(CurrencyType.Coin, SetCoinsText);
			UserManager.RemoveActionForCurrency(CurrencyType.Bonus, SetBonusText);
			UserManager.RemoveActionForCurrency(CurrencyType.Shadow, SetShadowsText);
			UserManager.OnExperienceChanged -= UpdateLevelAndExperience;
		}
		private void SetUserName(string name)
		{
			userName.text = name;
		}
		private void SetBonusText(long bonus)
		{
			SetCurrencyText(bonusLbl, bonus, true);
		}
		private void SetBonusText(long bonus, bool animate)
		{
			SetCurrencyText(bonusLbl, bonus, animate);
		}
		private void SetCoinsText(long coins)
		{
			SetCurrencyText(coinsLbl, coins, true);
		}
		private void SetCoinsText(long coins, bool animate)
		{
			SetCurrencyText(coinsLbl, coins, animate);
		}
		private void SetShadowsText(long shadows)
		{
			SetCurrencyText(shadowsLbl, shadows, true);
		}
		private void SetShadowsText(long shadows, bool animate)
		{
			SetCurrencyText(shadowsLbl, shadows, animate);
		}
		private void SetCurrencyText(UnityEngine.UI.Text label, long value, bool anim)
		{
			if (anim)
			{
				long startCoins = 0L;
				long.TryParse(label.text, out startCoins);
				DOTween.To(() => startCoins, delegate(long x)
				{
					startCoins = x;
					label.text = startCoins.ToString();
				}, value, 1f).SetEase(Ease.Linear);
			}
			else
			{
				label.text = value.ToString();
			}
		}
		private void UpdateLevelAndExperience(long expValue)
		{
			level.text = UserManager.UserModelInfo.level.ToString();
			levelProgress.value = (float)UserManager.UserModelInfo.experience / (float)UserManager.UserModelInfo.levelExperience;
		}
		private void Show()
		{
			if (!_instance.isShowing)
			{
				_instance.isShowing = true;
			}
		}
		private void Hide()
		{
			if (_instance.isShowing)
			{
				_instance.isShowing = false;
			}
		}
	}
}

