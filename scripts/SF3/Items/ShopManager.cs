using System;
using System.Collections;
using System.Collections.Generic;
using Nekki;
using SF3.Audio;
using SF3.GameModels;
using SF3.Moves;
using SF3.Settings;
using SF3.UserData;
using Godot;
using Color = Godot.Color;
using sf3DTO;

namespace SF3.Items
{
	public partial class ShopManager : UIModuleHolder
	{
		private enum EShopState
		{
			OPEN = 0,
			CLOSE = 1,
			OPENING = 2,
			CLOSING = 3
		}

		private class ShopCategoryInfo
		{
			private int _selectedIndex;
			private UserShopCategory _categoryData;
			private readonly Control _categoryObject;
			private readonly Button _categoryButton;
			private readonly TextureRect _categorySprite;
			private readonly TextureRect _categoryBg;
			private readonly Control _lockIcon;
			private readonly Control _newItemNotificationIcon;
			private readonly Label _newItemNumLabel;

			public int SelectedIndex
			{
				get { return _selectedIndex; }
				set
				{
					if (_categoryData != null)
					{
						List<BaseItem> items = _categoryData.GetItems();
						if (items.Count > 0)
						{
							_selectedIndex = Mathf.Clamp(value, 0, _categoryData.items.Count - 1);
							SelectedShopItem = _categoryData.items[_selectedIndex];
						}
					}
					else
					{
						_selectedIndex = 0;
						SelectedShopItem = null;
					}
				}
			}

			public List<BaseItem> Items
			{
				get { return (_categoryData == null) ? null : _categoryData.GetItems(); }
			}

			public SF3.UserData.ShopItem SelectedShopItem { get; private set; }
			public ShopCategoryType CategoryType { get; private set; }
			public long NewItems { get; protected set; }

			public bool IsLock
			{
				get { return _lockIcon.Visible; }
				set
				{
					_lockIcon.Visible = value;
					if (value)
					{
						DisableCategory();
						_categorySprite.Modulate = CategoryDisableColor;
					}
					else
					{
						UnSelectCategory();
					}
				}
			}

			public ShopCategoryInfo(ShopCategoryType categoryTypeValue, Control tranfs, UserShopCategory categoryDataValue = null)
			{
				CategoryType = categoryTypeValue;
				_categoryObject = tranfs;
				_categoryButton = tranfs.GetNode<Button>(".");
				_newItemNotificationIcon = tranfs.GetChild(0) as Control;
				_newItemNumLabel = _newItemNotificationIcon.GetNode<Label>(".");
				_lockIcon = tranfs.GetChild(1) as Control;
				_categoryBg = tranfs.GetChild(2).GetNode<TextureRect>(".");
				_categorySprite = tranfs.GetChild(3).GetNode<TextureRect>(".");
				UpdateCategoryData(categoryDataValue);
			}

			public void SelectCategory()
			{
				_categorySprite.Modulate = CategoryPressedColor;
				_categoryBg.Modulate = CategoryPressedColorBg;
			}

			public void UnSelectCategory()
			{
				_categorySprite.Modulate = CategoryDefaultColor;
				_categoryBg.Modulate = CategoryDefaultColorBg;
			}

			private void DisableCategory()
			{
				SelectedIndex = 0;
			}

			public void UpdateCategoryData(UserShopCategory categoryDataValue)
			{
				_categoryData = categoryDataValue;
				if (_categoryData != null && _categoryData.items.Count > 0)
				{
					_categoryData.CalculateItemsCurrency();
					NewItems = UserBadgesManager.Instance.WhichItemsisNew(UserBadgesManager.BadgeTypes.Shop, categoryDataValue.items);
					_newItemNotificationIcon.Visible = NewItems > 0;
					_newItemNumLabel.Text = NewItems.ToString();
					IsLock = false;
				}
				else
				{
					_newItemNotificationIcon.Visible = false;
					IsLock = true;
				}
				if (SelectedShopItem == null)
				{
					SelectedIndex = 0;
				}
			}

			public bool SelectItem(int itemID)
			{
				List<BaseItem> items = _categoryData.GetItems();
				for (int i = 0; i < items.Count; i++)
				{
					if (items[i].id.Equals(itemID))
					{
						SelectedIndex = i;
						return true;
					}
				}
				SelectedIndex = 0;
				return false;
			}

			public void RemoveBadges()
			{
				_newItemNotificationIcon.Visible = false;
				_categoryData.SetCategoryNotification(false);
			}
		}

		private const float REFRESH_TIMEOUT = 3f;
		private static ShopManager _instance;

		[Export]
		private string _onOpenSoundName = string.Empty;
		[Export]
		private string _onCloseSoundName = string.Empty;
		[Export]
		private Color _categoryPressedColor;
		[Export]
		private Color _categoryPressedColorBg;
		[Export]
		private Color _categoryDefaultColor;
		[Export]
		private Color _categoryDefaultColorBg;
		[Export]
		private Color _categoryDisableColor;

		private EShopState _shopState;
		private Vector3 _openRightPosition;
		private Vector3 _openLeftPosition;
		private Vector3 _closedRightPosition;
		private Vector3 _closedLeftPosition;

		[Export]
		private float _openCloseTime = 60f;
		[Export]
		private Curve _openCloseCurve;
		private float _nextOpenCloseTime;

		[Export]
		private Control _shopHolder;
		private List<ReelItem> _reelItems;
		[Export]
		private Control _contentPanel;
		[Export]
		private Control _categoriesPanel;
		[Export]
		private Control _cardsPanel;

		[Export]
		private Control _categories;
		private Dictionary<int, ShopCategoryInfo> _shopCategoriesSenders;

		[Export]
		private Control _refreshBtn;
		[Export]
		private Label _timeNow;
		[Export]
		private Control _refreshLight;
		[Export]
		private Node _itemCardPrefab;
		[Export]
		private Node _itemBoosterPrefab;
		[Export]
		private Node3D _itemsParent;
		[Export]
		private Vector2 _cardsOffset = new Vector2(50f, 50f);
		[Export]
		private Vector2 _boosterOffset = new Vector2(10f, 10f);

		private bool _refreshInCooldown;

				private readonly Vector3 _playerOffsetPosition = new Vector3(0f, 0f, -500f);

		private UIScrollViewCustom _cardsScroll;
		private Equipment _currentEquipmentSaved;
		private bool _tryOnActive;

		[Export]
		private ShopItemDescription _itemDescription;
		[Export]
		private Control _arrowUp;
		[Export]
		private Control _arrowDown;
		[Export]
		private float _cardScale = 0.6f;

		private int _columnsCount = 3;
		private UserShopConfiguration _currentShopConfiguration;
		private Dictionary<ShopCategoryType, ShopCategoryInfo> _shopCategories;
		private ShopCategoryInfo _currentCategoryInfo;
		private float _itemsPadding;

		[Export]
		private float _timeToShowTryOn = 2f;
		[Export]
		private float _itemDarken = 0.5f;
		[Export]
		private string _buyIconSprite = "buy_icon";
		[Export]
		private string _upIconSprite = "up-icon";
		[Export]
		private float _upgradeAttrDuration = 0.3f;

		private const float BOOSTER_REQUEST_TIMEOUT = 10f;
		private readonly string SERVER_WAIT_ALIAS = "server_response_wait";
		private AiMode _aiModeCurrent;
		private ShopIntentModule _intent;
		private Action _callbackOnOpen;
		private Action _callbackOnClosed;

		public static ShopManager Instance { get { return _instance; } }

		public static Color CategoryPressedColor { get { return Instance._categoryPressedColor; } }
		public static Color CategoryPressedColorBg { get { return Instance._categoryPressedColorBg; } }
		public static Color CategoryDefaultColor { get { return Instance._categoryDefaultColor; } }
		public static Color CategoryDefaultColorBg { get { return Instance._categoryDefaultColorBg; } }
		public static Color CategoryDisableColor { get { return Instance._categoryDisableColor; } }

		private Model _playerModel { get { return ModelsManager.Instance.Player; } }

		public event Action OnBuySuccessful;

		public override void _Ready()
		{
			base._Ready();
			_instance = this;
		}

		public override void Initialize()
		{
			UserShopManager.OnUserShopUpdated += UpdateShopData;
			_currentShopConfiguration = UserShopManager.Instance.shopConfiguration;
			ItemBuyingController.Instance.OnSuccessBuy += OnBuyItemSuccess;
			ItemBuyingController.Instance.OnFailureBuy += OnBuyItemFailure;
			_cardsScroll = _itemsParent.GetNode<UIScrollViewCustom>(".");
			_itemsPadding = _cardsOffset.Y;
			_cardsScroll.padding = new Vector2(0f, _itemsPadding);
			InitShopCategories();
			_itemDescription.Initialize();
			_itemDescription.AddProgressAnimationCallback(OnDescriptionProgressAnimationEnd);
			InitShopPartsPositions();
			InitButtons();
			_shopState = EShopState.CLOSING;
			ChangeShopState(EShopState.CLOSE);
			AudioManager.Instance.LoadSound(_onOpenSoundName, _onCloseSoundName);
			_currentShopConfiguration.OnShopRefreshUpdated.Add(UpdateRefreshTimerState);
			_shopHolder.Visible = false;
			Visible = false;
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			UserShopManager.OnUserShopUpdated -= UpdateShopData;
			ItemBuyingController.Instance.OnSuccessBuy -= OnBuyItemSuccess;
			ItemBuyingController.Instance.OnFailureBuy -= OnBuyItemFailure;
			if (_currentShopConfiguration != null)
			{
				_currentShopConfiguration.OnShopRefreshUpdated.Remove(UpdateRefreshTimerState);
			}
		}

		public void UpdateShopData()
		{
			if (_shopState != 0 && _shopState != EShopState.OPENING)
			{
				return;
			}
			UpdateRefreshTimerState();
			foreach (ShopCategoryInfo value in _shopCategories.Values)
			{
				value.UpdateCategoryData(UserShopManager.Instance.GetShopCategoryData(value.CategoryType));
			}
			ShopCategoryType categoryType = _currentCategoryInfo.CategoryType;
			if (_currentCategoryInfo.IsLock)
			{
				foreach (ShopCategoryInfo value2 in _shopCategories.Values)
				{
					if (!value2.IsLock)
					{
						categoryType = value2.CategoryType;
						break;
					}
				}
			}
			else if (!_currentCategoryInfo.SelectItem(_currentShopConfiguration.selectedItem))
			{
				_currentShopConfiguration.SetSelectedItem(_currentCategoryInfo.SelectedShopItem.item.id);
			}
			SelectCategory(categoryType);
		}

		private void InitShopCategories()
		{
			_shopCategories = new Dictionary<ShopCategoryType, ShopCategoryInfo>();
			_shopCategoriesSenders = new Dictionary<int, ShopCategoryInfo>();
			foreach (ShopCategoryType enumerator3 in EnumsCompliancer.GetEnumerators<ShopCategoryType>())
			{
				if (enumerator3 == ShopCategoryType.None)
				{
					continue;
				}
				string value = enumerator3.ToString().ToUpper();
				foreach (Node item in _categories.GetChildren())
				{
					if (item.Name.ToUpper().Contains(value))
					{
						ShopCategoryInfo value2 = new ShopCategoryInfo(enumerator3, item as Control);
						_shopCategories.Add(enumerator3, value2);
						_shopCategoriesSenders.Add((int)item.GetInstanceId(), value2);
						break;
					}
				}
			}
			_currentCategoryInfo = _shopCategories[_currentShopConfiguration.currentCategory];
		}

		private void InitButtons()
		{
			_itemDescription.BuyForCoinsBtn.Pressed += () => BuyItem(CurrencyType.Coin);
			_itemDescription.BuyForBonusBtn.Pressed += () => BuyItem(CurrencyType.Bonus);
		}

		private void InitShopPartsPositions()
		{
			InitCards(_currentCategoryInfo.CategoryType == ShopCategoryType.Booster);
			_openLeftPosition = _contentPanel.Position;
			_openRightPosition = _itemDescription.Position;
			_closedLeftPosition = _openLeftPosition;
			_closedLeftPosition.X -= GetViewportRect().Size.X;
			_closedRightPosition = _openRightPosition;
			_closedRightPosition.X += GetViewportRect().Size.X;
		}

		private void InitCards(bool isBoosterpack)
		{
			if (_reelItems != null)
			{
				foreach (ReelItem reelItem in _reelItems)
				{
					reelItem.QueueFree();
				}
			}
			Vector2 vector = (!isBoosterpack) ? _cardsOffset : _boosterOffset;
			_reelItems = new List<ReelItem>();
			Vector2 viewportSize = GetViewportRect().Size;
			float num = viewportSize.X;
			float num2 = 0f;
			Node prefab = (!isBoosterpack) ? _itemCardPrefab : _itemBoosterPrefab;
			Vector2 vector4 = Vector2.Zero;
			_columnsCount = 0;
			while (num > num2)
			{
				num2 += vector4.X + vector.X;
				if (_columnsCount == 0)
				{
					num2 -= vector.X;
				}
				_columnsCount++;
			}
			_cardsScroll.cellSize = new Vector2(vector4.X + vector.X, vector4.Y + vector.Y);
			int shopCategoryItemCount = UserShopManager.Instance.GetShopCategoryItemCount(_currentCategoryInfo.CategoryType);
			for (int i = 0; i < shopCategoryItemCount; i++)
			{
				CreateNewItem(isBoosterpack);
				_reelItems[i].UpdateDepth(i);
			}
			_cardsScroll.InvalidateBounds();
		}

		private void CreateNewItem(bool isBoosterpack)
		{
			int num = _reelItems.Count / _columnsCount;
			int num2 = _reelItems.Count - num * _columnsCount;
			Vector2 vector = (!isBoosterpack) ? _cardsOffset : _boosterOffset;
			Node prefab = (!isBoosterpack) ? _itemCardPrefab : _itemBoosterPrefab;
			Node gameObject = prefab.Duplicate();
			gameObject.GetParent()?.RemoveChild(gameObject);
			_itemsParent.AddChild(gameObject);
			Vector3 vector2 = (gameObject as ReelItem).LocalSize() * _cardScale;
			((Node3D)gameObject).Position = new Vector3(vector2.X * num2 + vector.X * num2 + vector2.X / 2f, -(vector2.Y * num + vector.Y * num), 0f);
			int i = _reelItems.Count;
			((Node3D)gameObject).Scale = new Vector3(_cardScale, _cardScale, _cardScale);
			ReelItem newItem = gameObject as ReelItem;
			Action newCallBack = delegate
			{
				if (_currentCategoryInfo.SelectedIndex == i)
				{
					if (newItem is CardItem)
					{
						(newItem as CardItem).ShowTryOnBtn();
					}
				}
				else
				{
					SelectItemCard(i);
					SF3UiLogger.instance.AddShopSelectItemEvent(_reelItems[i].Item);
				}
			};
			newItem.AddClickCallback(newCallBack);
			if (newItem is CardItem)
			{
				CardItem cardItem = newItem as CardItem;
				cardItem.OnFlipEnded += OnCardFlipEnded;
				cardItem.AddTryOnCallback(TryOnItem);
				cardItem.UpdateShade(_itemDarken);
			}
			newItem.ActivateSelectionBorder(false);
			newItem.Visible = false;
			_reelItems.Add(newItem);
		}

		public void RefreshCategories()
		{
			SF3UiLogger.instance.AddButtonClickEvent(ConstantsSF3.ELocationSceneModule.Shop, "refresh");
			UserBadgesManager.Instance.Clear(UserBadgesManager.BadgeTypes.Shop);
			UserDataController.Send_GenerateShop(3f);
			UnBlockRefreshScreenUI();
		}

		private void BlockRefreshScreenUI()
		{
			UIBlocker.Instance.Block(UIBlocker.Priority.Preloader);
			LoadingIcon.Instance.EnableLoadingScreen(SERVER_WAIT_ALIAS);
		}

		private void UnBlockRefreshScreenUI()
		{
			LoadingIcon.Instance.DisableLoadingScreen(0.5f, delegate
			{
				UIBlocker.Instance.Unblock(UIBlocker.Priority.Preloader);
			}, SERVER_WAIT_ALIAS);
			SelectItemCard(0);
		}

		public override void ShowModule(IntentModule intent, Action callbackOnOpen)
		{
			_intent = (ShopIntentModule)intent;
			_callbackOnOpen = callbackOnOpen;
			if (ModelsManager.Instance.Enemy != null)
			{
				_aiModeCurrent = ModelsManager.Instance.Enemy.GetAiMode();
				ModelsManager.Instance.Enemy.SetAiMode(AiMode.NoneMode);
			}
			Visible = true;
			_shopHolder.Visible = true;
			LocationColorAnimation.Instance.Animate(GameSettings.DojoSettings.LocationColorChangeTime, GameSettings.DojoSettings.DefaultLocationColor, GameSettings.DojoSettings.LocationColorInModule);
			LocationColorAnimation.Instance.EnableRays(false, GameSettings.DojoSettings.LocationColorChangeTime);
			if (ModelsManager.Instance.Player != null)
			{
				BattleCamera.MoveToPlayer(_playerOffsetPosition, null, _intent.IsInstant());
			}
			HolderModule.EnableControls(false);
			ModelsManager.Instance.HideModels(1f);
			ChangeShopState(EShopState.OPENING);
			UpdateShopData();
			UpdateModule(intent);
			AudioManager.Instance.PlaySound(_onOpenSoundName);
		}

		public override void UpdateModule(IntentModule intent)
		{
			_intent = (ShopIntentModule)intent;
			if (_intent.Category != 0)
			{
				SelectCategory(_intent.Category);
				if (_intent.ItemId != -1 && _currentCategoryInfo.SelectedShopItem.item.id != _intent.ItemId)
				{
					UnselectCurrentItemCard();
					_currentCategoryInfo.SelectItem(_intent.ItemId);
					SelectItemCard(_currentCategoryInfo.SelectedIndex);
				}
			}
			else
			{
				SelectCategory(_currentCategoryInfo.CategoryType);
			}
			if (_shopState == EShopState.OPEN)
			{
				StoreOpened();
			}
		}

		private void StoreOpened()
		{
			_callbackOnOpen.InvokeSafe();
		}

		public override void HideModule(Action callbackOnClosed)
		{
			_callbackOnClosed = callbackOnClosed;
			LocationColorAnimation.Instance.Animate(GameSettings.DojoSettings.LocationColorChangeTime, GameSettings.DojoSettings.LocationColorInModule, GameSettings.DojoSettings.DefaultLocationColor);
			LocationColorAnimation.Instance.EnableRays(true, GameSettings.DojoSettings.LocationColorChangeTime);
			if (_tryOnActive)
			{
				EndTryOnItem();
			}
			ChangeShopState(EShopState.CLOSING);
			AudioManager.Instance.PlaySound(_onCloseSoundName);
			if (ModelsManager.Instance.Enemy != null)
			{
				ModelsManager.Instance.Enemy.SetAiMode(_aiModeCurrent);
			}
		}

		private void StoreClosed()
		{
			_shopHolder.Visible = false;
			Visible = false;
			_callbackOnClosed.InvokeSafe();
		}

		private string FormatRefreshTime()
		{
			TimeSpan timeSpan = _currentShopConfiguration.nextRefreshAvailableTime - NetworkConnection.current.getCurrentServerDateTime();
			if (timeSpan.Days > 0)
			{
				return string.Format("{0}d {1:00}:{2:00}", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes);
			}
			if (timeSpan.Hours > 0)
			{
				return string.Format("{0:00}:{1:00}", timeSpan.Hours, timeSpan.Minutes);
			}
			return string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
		}

		private void UpdateRefreshTimerState()
		{
			_refreshInCooldown = UserShopManager.Instance.shopConfiguration.IsShopRefreshInCooldown;
			if (_refreshInCooldown)
			{
				_timeNow.Visible = true;
				FormatRefreshTime();
				_refreshLight.Visible = false;
			}
			else
			{
				_timeNow.Visible = false;
				_refreshLight.Visible = true;
			}
		}

		public override void _Process(double delta)
		{
			UpdateRefreshTimer();
			UpdateShopState();
		}

		private void UpdateRefreshTimer()
		{
			if (_refreshInCooldown)
			{
				_timeNow.Text = FormatRefreshTime();
			}
		}

		private void UpdateShopState()
		{
			if (_shopState == EShopState.OPENING)
			{
				float num = (_nextOpenCloseTime - GameTimeController.battleTime) / _openCloseTime;
				if (num <= 0f)
				{
					ChangeShopState(EShopState.OPEN);
					StoreOpened();
				}
				else
				{
					_contentPanel.Position = GetNewPosition(_closedLeftPosition, _openLeftPosition, _openCloseCurve.Sample(num));
					_itemDescription.Position = GetNewPosition(_closedRightPosition, _openRightPosition, _openCloseCurve.Sample(num));
				}
			}
			else
			{
				if (_shopState != EShopState.CLOSING)
				{
					return;
				}
				float num2 = (_nextOpenCloseTime - GameTimeController.battleTime) / _openCloseTime;
				if (num2 <= 0f)
				{
					ChangeShopState(EShopState.CLOSE);
					if (!_tryOnActive)
					{
						StoreClosed();
					}
				}
				else
				{
					_contentPanel.Position = GetNewPosition(_openLeftPosition, _closedLeftPosition, _openCloseCurve.Sample(num2));
					_itemDescription.Position = GetNewPosition(_openRightPosition, _closedRightPosition, _openCloseCurve.Sample(num2));
				}
			}
		}

		private Vector3 GetNewPosition(Vector3 firstPosition, Vector3 secondPosition, float newTime)
		{
			return firstPosition.Lerp(secondPosition, newTime);
		}

		private void ChangeShopState(EShopState newState)
		{
			if (_shopState == newState)
			{
				return;
			}
			switch (newState)
			{
			case EShopState.OPENING:
			case EShopState.CLOSING:
				if (_shopState == EShopState.OPENING || _shopState == EShopState.CLOSING)
				{
					_nextOpenCloseTime = _openCloseTime - (_nextOpenCloseTime - GameTimeController.battleTime) + GameTimeController.battleTime;
				}
				else
				{
					_nextOpenCloseTime = GameTimeController.battleTime + _openCloseTime;
				}
				break;
			case EShopState.OPEN:
				_contentPanel.Position = _openLeftPosition;
				_itemDescription.Position = _openRightPosition;
				break;
			case EShopState.CLOSE:
				_contentPanel.Position = _closedLeftPosition;
				_itemDescription.Position = _closedRightPosition;
				break;
			}
			_shopState = newState;
		}

		private void UnselectCurrentItemCard()
		{
			if (_currentCategoryInfo.SelectedShopItem != null)
			{
				if (_currentCategoryInfo.SelectedIndex >= _reelItems.Count)
				{
					return;
				}
				_reelItems[_currentCategoryInfo.SelectedIndex].ActivateSelectionBorder(false);
				if (_reelItems[_currentCategoryInfo.SelectedIndex] is CardItem)
				{
					CardItem cardItem = _reelItems[_currentCategoryInfo.SelectedIndex] as CardItem;
					cardItem.HideTryOnBtn();
					cardItem.UpdateShade(_itemDarken);
				}
			}
			_itemDescription.ClearDescription();
		}

		private void SelectCategory(ShopCategoryType category)
		{
			List<BaseItem> items = _shopCategories[category].Items;
			if (items == null || items.Count == 0)
			{
				foreach (ShopCategoryInfo value in _shopCategories.Values)
				{
					List<BaseItem> items2 = value.Items;
					if (items2 != null && items2.Count > 0)
					{
						category = value.CategoryType;
						break;
					}
				}
			}
			UnselectCurrentItemCard();
			_currentCategoryInfo.UnSelectCategory();
			_currentCategoryInfo = _shopCategories[category];
			_currentCategoryInfo.SelectCategory();
			UserBadgesManager.Instance.Reset(UserBadgesManager.BadgeTypes.Shop, items);
			_currentCategoryInfo.RemoveBadges();
			_currentShopConfiguration.SetCurrentCategory(category, _currentCategoryInfo.SelectedShopItem.item.id);
			UpdateIntent(category, _currentCategoryInfo.SelectedShopItem.item.id);
			UpdateItemCards(category == ShopCategoryType.Booster);
		}

		private void UpdateIntent(ShopCategoryType category, int itemId)
		{
			if (_intent.Category != category)
			{
				_intent.SetCategory(category);
				_intent.SetItemId(itemId);
			}
			else if (_intent.ItemId != itemId)
			{
				_intent.SetItemId(itemId);
			}
		}

		private void SelectItemCard(int itemCardIndex)
		{
			_itemDescription.BreakProgressAnimation();
			UnselectCurrentItemCard();
			_currentCategoryInfo.SelectedIndex = itemCardIndex;
			_reelItems[_currentCategoryInfo.SelectedIndex].ActivateSelectionBorder(true);
			_itemDescription.ShowDescription(_currentCategoryInfo.SelectedShopItem);
			if (_reelItems[_currentCategoryInfo.SelectedIndex] is CardItem)
			{
				CardItem cardItem = _reelItems[_currentCategoryInfo.SelectedIndex] as CardItem;
				cardItem.UpdateShade(0f);
			}
			_cardsScroll.ScrollVerticalToCell((int)Mathf.Floor((float)_currentCategoryInfo.SelectedIndex / _columnsCount));
			_currentShopConfiguration.SetSelectedItem(_currentCategoryInfo.SelectedShopItem.item.id);
			UpdateIntent(_currentCategoryInfo.CategoryType, _currentCategoryInfo.SelectedShopItem.item.id);
		}

		public void BuyItem(CurrencyType currencyType)
		{
			if (_currentCategoryInfo.SelectedShopItem != null)
			{
				if (_currentCategoryInfo.SelectedShopItem.item is Booster)
				{
					Currency currency = new Currency { CurrencyType = currencyType, Value = _currentCategoryInfo.SelectedShopItem.GetCurrencyValue(currencyType) };
					BuyBoosterRequest request = new BuyBoosterRequest
					{
						ShopBoosterModelId = _currentCategoryInfo.SelectedShopItem.BoosterpackID,
						Currency = currency
					};
					UserDataController.Send_BuyBooster(request, 10f);
				}
				else
				{
					ItemBuyingController.Instance.BuyItem(_currentCategoryInfo.SelectedShopItem, currencyType);
				}
			}
		}

		internal void OnBuyItemSuccess()
		{
			if (_currentCategoryInfo != null)
			{
				if (ModelsManager.Instance.Player != null && !ModelsManager.Instance.Player.IsEquipmentEquipped(_currentCategoryInfo.SelectedShopItem.item.id))
				{
					ModelsManager.Instance.Player.EquipItem(_currentCategoryInfo.SelectedShopItem.item.id);
				}
				else
				{
					UserManager.UserModelInfo.EquipItem(_currentCategoryInfo.SelectedShopItem.item.id);
				}
				this.OnBuySuccessful.InvokeSafe();
				_itemDescription.AnimateProgressOnSelectedItem();
				if (_reelItems[_currentCategoryInfo.SelectedIndex] is CardItem)
				{
					(_reelItems[_currentCategoryInfo.SelectedIndex] as CardItem).ShowShopIcon(_upIconSprite);
				}
				_reelItems[_currentCategoryInfo.SelectedIndex].UpdateItem();
			}
		}

		internal void OnBuyItemFailure()
		{
		}

		private void OnDescriptionProgressAnimationEnd()
		{
			_itemDescription.BreakProgressAnimation();
			_itemDescription.ShowDescription(_currentCategoryInfo.SelectedShopItem, _upgradeAttrDuration);
			_itemDescription.AnimateAddedProgressOnSelectedItem();
		}

		private void OnCardFlipEnded()
		{
			if (_currentCategoryInfo.SelectedShopItem != null && !_cardsScroll.isScrolling)
			{
				_itemDescription.SetBuyButtonsEnabling(true);
			}
		}

		public void TryOnItem()
		{
			SF3UiLogger.instance.AddButtonClickEvent(ConstantsSF3.ELocationSceneModule.Shop, "try_on");
			if (!_tryOnActive)
			{
				ModelsManager.Instance.ShowPlayer(1f);
				_tryOnActive = true;
				_currentEquipmentSaved = _playerModel.GetEquippedForType((_currentCategoryInfo.SelectedShopItem.item as Equipment).GetEquipmentType());
				_playerModel.EquipItemNotExisted(_currentCategoryInfo.SelectedShopItem.item as Equipment, false);
				BattleController.ThrowEvent(new BattleEventArgs(ETriggerEvents.EVENT_TRY_ON, ModelsManager.Instance.Player.id, (Equipment)_currentCategoryInfo.SelectedShopItem.item));
				ChangeShopState(EShopState.CLOSING);
				if (!UserManager.IsTutorialComplete())
				{
					UIBlocker.Instance.Block();
				}
			}
		}

		public void EndTryOnItem()
		{
			if (_tryOnActive)
			{
				ModelsManager.Instance.HidePlayer(1f);
				_tryOnActive = false;
				_playerModel.EquipItem(_currentEquipmentSaved.id, false);
				_currentEquipmentSaved = null;
				ChangeShopState(EShopState.OPENING);
			}
		}

		public void ScrollCardsUp()
		{
			_cardsScroll.ScrollUpByCell();
		}

		public void ScrollCardsDown()
		{
			_cardsScroll.ScrollDownByCell();
		}

		public void ShowCategory(Node senderObject)
		{
			ulong instanceID = senderObject.GetInstanceId();
			foreach (var kvp in _shopCategoriesSenders)
			{
				if (kvp.Key == (int)instanceID && _currentCategoryInfo.CategoryType != kvp.Value.CategoryType)
				{
					string srcSlot = _currentCategoryInfo.CategoryType.ToString();
					string dstSlot = kvp.Value.CategoryType.ToString();
					long newItems = kvp.Value.NewItems;
					SF3UiLogger.instance.AddShopSlotChangeEvent(dstSlot, srcSlot, newItems);
					SelectCategory(kvp.Value.CategoryType);
				}
			}
		}

		private void UpdateItemCards(bool changeToBoosterpack)
		{
			List<BaseItem> items = _currentCategoryInfo.Items;
			for (int i = 0; i < _reelItems.Count; i++)
			{
				_reelItems[i].Visible = false;
			}
			UnselectCurrentItemCard();
			if (items == null || items.Count == 0)
			{
				return;
			}
			if ((_reelItems[0] is CardItem && changeToBoosterpack) || (_reelItems[0] is BoosterpackItem && !changeToBoosterpack))
			{
				InitCards(changeToBoosterpack);
			}
			else if (_reelItems.Count < items.Count)
			{
				int num = _currentCategoryInfo.Items.Count - _reelItems.Count;
				for (int j = 0; j < num; j++)
				{
					CreateNewItem(changeToBoosterpack);
					_reelItems[j].UpdateDepth(j);
				}
			}
			for (int k = 0; k < _reelItems.Count; k++)
			{
				_reelItems[k].Visible = true;
				if (_reelItems[k] is CardItem)
				{
					CardItem cardItem = _reelItems[k] as CardItem;
					cardItem.ResetFlip();
					if (k < items.Count)
					{
						cardItem.Init(items[k]);
						cardItem.ActivateSelectionBorder(false);
						cardItem.ActivateButton(true);
						BaseItem itemByID = UserManager.UserModelInfo.GetItemByID(items[k].id);
						if (itemByID != null)
						{
							cardItem.ShowShopIcon(_upIconSprite);
						}
						else
						{
							cardItem.ShowShopIcon(_buyIconSprite);
						}
						cardItem.SetImage();
					}
					else
					{
						cardItem.FlipHorizontally();
						cardItem.HideShopIcon();
						cardItem.HideSale();
					}
				}
				else
				{
					BoosterpackItem boosterpackItem = _reelItems[k] as BoosterpackItem;
					if (k < items.Count)
					{
						boosterpackItem.Init(items[k]);
						boosterpackItem.ActivateButton(true);
						boosterpackItem.ActivateSelectionBorder(false);
					}
				}
			}
			SelectItemCard(_currentCategoryInfo.SelectedIndex);
			_cardsScroll.ResetPosition();
			_cardsScroll.InvalidateBounds();
			if (_cardsScroll.panel.baseClipRegion.W > _cardsScroll.bounds.Size.Y)
			{
				_cardsScroll.Enabled = false;
				_arrowUp.Visible = false;
				_arrowDown.Visible = false;
			}
			else
			{
				_cardsScroll.Enabled = true;
				_arrowUp.Visible = true;
				_arrowDown.Visible = true;
			}
		}
	}
}
