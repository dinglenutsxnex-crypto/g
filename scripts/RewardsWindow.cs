using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Color = Godot.Color;
using Nekki.UI;
using SF3;
using SF3.Effects;
using SF3.Items;
using sf3DTO;

public partial class RewardsWindow : NekkiUIModule
{
	private enum States
	{
		None = 0,
		CoinAnimation = 1,
		CoinAnimationFinish = 2,
		RewardAnimation = 3,
		RewardAnimationFinish = 4
	}

	public Action onCloseAction;

	[Export]
	private Button _breakBtn;

	[Export]
	private RewardsWindowUnit _baseUnit;

	[Export]
	private Button _btnContinue;

	[Export]
	private Label _header;

	[Export]
	private Label _experience;

	[Export]
	private Node3D _expBarPlaceholder;

	[Export]
	private Node _expBarPrf;

	[Export]
	private Label _totalGold;

	[Export]
	private Label _totalBonus;

	[Export]
	private Node _totalRewardObj;

	[Export]
	private Node _rewardCoinPrf;

	[Export]
	private Node _rewardBasePrf;

	[Export]
	private Label _shadowCurrencyLbl;

	[Export]
	private Node3D _coinPlaceholder;

	[Export]
	private float _coinStepX = 0.5f;

	[Export]
	private float _coinStepY = 1f;

	[Export]
	private float _coinStepYBasePadding = 10f;

	[Export]
	private Node _reelItemPrf;

	[Export]
	private Node _boosterpackPrf;

	[Export]
	private Node3D _reelItemsPlaceholder;

	[Export]
	private RewardInAnimation _rewardInAnimation;

	[Export]
	private CustomSlideInput _customSlideInput;

	[Export]
	private RewardInfo _rewardInfo;

	private float _xOffset;

	private float _yOffset;

	private IAnimatedExpBar _expBar;

	private List<RewardCoin> _rewardCoinList;

	private States _state;

	private List<BaseItem> _rewardItem;

	private ICardAnimationPlayer _cardAnimation;

	private SF3.FightResult _fightResult;

	private Dictionary<string, long> _fightRewardBonus;

	private long _coins;

	private long _bonus;

	private long _totalCoins;

	private long _totalBonuses;

	private long _exp;

	private float _totalGoldAnimationDuration = 1.2f;

	private float _startCoinAnimationDuration = 0.5f;

	private Camera3D _mainCamera;

	private static readonly Dictionary<string, string> RewardVsImageNames = new Dictionary<string, string>
	{
		{ "Reward_Bonus_HeadHit", "bonus_headhits" },
		{ "Reward_Bonus_FirstStrike", "bonus_first_strike" },
		{ "Reward_Bonus_Critical", "bonus_criticals" },
		{ "Reward_Bonus_Combo", "bonus_combo" },
		{ "Reward_Bonus_ShadowAbilities", "bonus_shadow_abilities" }
	};

	protected long totalGold
	{
		get
		{
			return _totalCoins;
		}
		set
		{
			_totalCoins = value;
			_totalGold.Text = _totalCoins.ToString();
		}
	}

	protected long totalBonus
	{
		get
		{
			return _totalBonuses;
		}
		set
		{
			_totalBonuses = value;
			_totalBonus.Text = _totalBonuses.ToString();
		}
	}

	public void Init(SF3.FightResult result, RewardDataProvider rewardDataProvider)
	{
		if (_customSlideInput == null)
		{
			GD.PrintErr("Slide Input not found");
			return;
		}
		_customSlideInput.onPress += CardSelect;
		_fightResult = result;
		_fightRewardBonus = _fightResult.GetFightRewardBonus();
		_exp = _fightResult.GetRewardExperience();
		_coins = _fightResult.GetRewardCurrency(CurrencyType.Coin);
		_bonus = _fightResult.GetRewardCurrency(CurrencyType.Bonus);
		_rewardItem = new List<BaseItem>();
		_rewardItem.AddRange(_fightResult.GetRewardEquipment().Select(equipment => (BaseItem)equipment));
		_rewardItem.AddRange(_fightResult.GetRewardPerks().Select(equipment => (BaseItem)equipment));
		_rewardItem.AddRange(_fightResult.GetRewardBoosters().Select(equipment => (BaseItem)equipment));
		_rewardInfo.Init();
		_cardAnimation = GetNode<CardAnimator>(new NodePath("CardAnimator"));
		_cardAnimation.onAnimationEnd += delegate
		{
			_state = States.RewardAnimationFinish;
			_breakBtn.Visible = false;
		};
		_cardAnimation.Init(rewardDataProvider, CreateCardsForAnimator(rewardDataProvider.rewardItemProvider), _rewardInfo);
		FillHeader(result);
		_rewardInAnimation.onAnimationEnd += StartInfoAnimation;
		_expBar = CreateExpBar();
		_expBar.CurrentLevel = rewardDataProvider.startLevel;
		_expBar.FromExp = rewardDataProvider.startExp;
		_expBar.AddedExp = _exp;
		_expBar.onAnimationEnd += StartContinueBtnAnimation;
		_expBar.Hide();
		_btnContinue.Visible = false;
		long rewardCurrency = _fightResult.GetRewardCurrency(CurrencyType.Shadow);
		if (rewardCurrency > 0)
		{
			ShowShadowCurrency(rewardCurrency);
		}
		_rewardCoinList = new List<RewardCoin> { CreateBaseReward("base_reward", _coins.ToString(), _bonus.ToString()) };
		foreach (string value in RewardMultipyerCounter.Instance.RewardMultipliersMap.Values)
		{
			long num = 0L;
			if (_fightRewardBonus.ContainsKey(value))
			{
				num = _fightRewardBonus[value];
			}
			if (!RewardVsImageNames.ContainsKey(value))
			{
				GD.PrintErr(string.Format("No key: [{0}] found in <RewardVsImageNames>!", value));
			}
			_rewardCoinList.Add(CreateRewardCoin(RewardVsImageNames[value], num.ToString(), (int)result.GetRewardMultiplierUsagesQuantity(value)));
			_coins += num;
		}
		totalGold = _coins;
		totalBonus = _bonus;
		_totalRewardObj.Visible = false;
		_mainCamera = GetViewport().GetCamera3D();
		_btnContinue.Pressed += delegate
		{
			switch (_state)
			{
			case States.CoinAnimationFinish:
				if (_rewardItem.Count > 0)
				{
					StartItemAnimation();
					_state = States.RewardAnimation;
				}
				else
				{
					Quit();
				}
				break;
			case States.RewardAnimationFinish:
				Quit();
				break;
			}
		};
		_breakBtn.Pressed += OnBreakBtnClick;
	}

	private void FillHeader(SF3.FightResult result)
	{
		List<HeaderFiller> list = new List<HeaderFiller>();
		list.Add(new FightFiller());
		list.Add(new SurvivalFiller());
		List<HeaderFiller> list2 = list;
		foreach (HeaderFiller item in list2)
		{
			item.Set(result, _header);
			if (item.IsFilled())
			{
				return;
			}
		}
		Messenger.Error("Header has not been filled!", this);
	}

	private void Quit()
	{
		NekkiUIRootModules.Instance.UnmountModule(this);
		BattleController.SystemResume();
		if (onCloseAction != null)
		{
			onCloseAction();
		}
	}

	public override void _Ready()
	{
		base._Ready();
		StartAnimation();
	}

	public static void Show(SF3.FightResult result, RewardDataProvider rewardDataProvider, Action onClose)
	{
		UIModulesController.Instance.EnableAllModules(false);
		RewardsWindow rewardsWindow = NekkiUIRootModules.Instance.MountNGUIModule<RewardsWindow>("Rewards");
		if (onClose != null)
		{
			rewardsWindow.onCloseAction = onClose;
		}
		rewardsWindow.Init(result, rewardDataProvider);
		EffectsManager.Reset();
	}

	private RewardCoin CreateRewardCoin(string name, string value, int count)
	{
		RewardCoin rewardCoin = CreateEmptyRewardField(_rewardCoinPrf);
		rewardCoin.SetCoins(name, value, count);
		return rewardCoin;
	}

	private RewardCoin CreateBaseReward(string name, string coinsValue, string bonusValue)
	{
		RewardCoin rewardCoin = CreateEmptyRewardField(_rewardBasePrf);
		rewardCoin.Set(name, coinsValue, bonusValue);
		_yOffset += _coinStepYBasePadding;
		return rewardCoin;
	}

	private RewardCoin CreateEmptyRewardField(Node prefab)
	{
		Node node = prefab.Duplicate();
		node.SetParent(_coinPlaceholder);
		node.Scale = Vector3.One;
		node.Position = new Vector3(_xOffset, _yOffset, 0f);
		node.Visible = false;
		_xOffset += _coinStepX;
		_yOffset += _coinStepY;
		return node.GetNode<RewardCoin>(new NodePath("RewardCoin"));
	}

	private IAnimatedExpBar CreateExpBar()
	{
		Node node = _expBarPrf.Duplicate();
		node.SetParent(_expBarPlaceholder);
		node.Position = Vector3.Zero;
		node.Scale = Vector3.One;
		return node.GetNode<IAnimatedExpBar>(new NodePath("IAnimatedExpBar"));
	}

	private void StartAnimation()
	{
		_rewardInAnimation.Play();
	}

	private void StartInfoAnimation()
	{
		Tween tween = CreateTween();
		tween.TweenCallback(Callable.From(StartCoinAnimation));
		tween.TweenInterval(_startCoinAnimationDuration);
		tween.TweenCallback(Callable.From(StartTotalRewardAnimation));
		tween.TweenInterval(_totalGoldAnimationDuration);
		tween.TweenCallback(Callable.From(StartExpAnimation));
	}

	private void StartContinueBtnAnimation()
	{
		_state = States.CoinAnimationFinish;
		_btnContinue.Visible = true;
		_breakBtn.Visible = false;
		Tween tween = CreateTween();
		tween.TweenProperty(_btnContinue, "modulate:a", 1.0, 0.3);
	}

	private void StartExpAnimation()
	{
		_state = States.CoinAnimation;
		_expBar.Show();
		_expBar.AnimateExp();
	}

	private void StartCoinAnimation()
	{
		if (_rewardCoinList.Count == 0)
		{
			return;
		}
		foreach (RewardCoin rewardCoin in _rewardCoinList)
		{
			rewardCoin.Visible = true;
			rewardCoin.Animate();
		}
	}

	public long getGold()
	{
		return this.totalGold;
	}
	public long getBonus()
	{
		return this.totalBonus;
	}

	private void StartTotalRewardAnimation()
	{
		this._totalRewardObj.Visible = true;
		this.totalGold = 0L;
		this.totalBonus = 0L;
		Tween tween = CreateTween();
		tween.TweenCallback(Callable.From(() => {
			ModulateAlpha(_totalRewardObj, 1.0f);
		}));
		if (this._coins > 0L)
		{
			tween.TweenCallback(Callable.From(() =>
			{
				Tween countTween = CreateTween();
				countTween.TweenMethod(Callable.From((long n) => this.totalGold = n), 0L, this._coins, this._totalGoldAnimationDuration);
			}));
		}
		if (this._bonus > 0L)
		{
			tween.TweenCallback(Callable.From(() =>
			{
				Tween countTween = CreateTween();
				countTween.TweenMethod(Callable.From((long n) => this.totalBonus = n), 0L, this._bonus, this._totalGoldAnimationDuration);
			}));
		}
	}

	private void ModulateAlpha(Node node, float alpha)
	{
		if (node is Control control)
		{
			Color c = control.Modulate;
			c.A = alpha;
			control.Modulate = c;
		}
	}

	private void StartItemAnimation()
	{
		_coinPlaceholder.Visible = false;
		_totalRewardObj.Visible = false;
		HideShadowCurrency();
		_cardAnimation.Animate();
		_breakBtn.Visible = true;
	}

	private void BreakAllAnimation()
	{
		_rewardInAnimation.Break();
		_expBar.BreakAnimation();
		_expBar.Show();
		foreach (RewardCoin rewardCoin in _rewardCoinList)
		{
			rewardCoin.Visible = true;
			rewardCoin.BreakAnimate();
		}
		totalGold = _coins;
		totalBonus = _bonus;
		_totalRewardObj.Visible = true;
		_btnContinue.Visible = true;
	}

	private List<IReelItemAnimation> CreateCardsForAnimator(List<RewardItemProvider> itemProvider)
	{
		List<IReelItemAnimation> list = new List<IReelItemAnimation>();
		foreach (RewardItemProvider item in itemProvider)
		{
			Node node;
			if (item.originItem is SF3.Items.Booster)
			{
				node = _boosterpackPrf.Duplicate();
				BoosterpackItem component = node.GetNode<BoosterpackItem>(new NodePath("BoosterpackItem"));
				component.Init(item.originItem);
			}
			else
			{
				node = _reelItemPrf.Duplicate();
				CardItem component2 = node.GetNode<CardItem>(new NodePath("CardItem"));
				component2.Init(item.originItem);
			}
			node.SetParent(_reelItemsPlaceholder);
			node.Scale = Vector3.One;
			node.Visible = false;
			IReelItemAnimation component3 = node.GetNode<IReelItemAnimation>(new NodePath("IReelItemAnimation"));
			if (component3 != null)
			{
				list.Add(component3);
			}
		}
		return list;
	}

	private void CardSelect(Node target)
	{
		ReelItem component = target.GetNode<ReelItem>(new NodePath("ReelItem"));
		if (component != null && _state == States.RewardAnimationFinish)
		{
			_cardAnimation.AnimateSelectCard(component);
		}
		else if (_state != States.RewardAnimationFinish)
		{
			OnBreakBtnClick();
		}
	}

	private void ShowShadowCurrency(long value)
	{
		_shadowCurrencyLbl.Text = value.ToString();
		_shadowCurrencyLbl.GetParent().Visible = true;
	}

	private void HideShadowCurrency()
	{
		_shadowCurrencyLbl.GetParent().Visible = false;
	}

	private void OnBreakBtnClick()
	{
		switch (_state)
		{
		case States.None:
		case States.CoinAnimation:
			BreakAllAnimation();
			_state = States.CoinAnimationFinish;
			break;
		case States.RewardAnimation:
			_cardAnimation.Break();
			_state = States.RewardAnimationFinish;
			break;
		}
		_breakBtn.Visible = false;
	}
}
