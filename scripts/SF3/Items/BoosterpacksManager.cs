using System;
using System.Collections.Generic;
using SF3.Settings;
using SF3.UserData;
using Godot;
using sf3DTO;

namespace SF3.Items
{
	public class BoosterpacksManager : UIModuleHolder
	{
		private const float HIDE_MODELS_TIME = 1f;
		private const float BOOSTERPACK_FADE_TIME = 0.5f;
		private const float SLICE_SIMULATION_TIME = 1f;
		private const float SCROLL_FADE_TIME = 0.5f;
		private const float STACKED_CARD_FADEIN_TIME = 0.5f;
		private const float CARD_MOVE_AND_FLIP_TIME = 0.6f;
		private const float SHINING_ADDED_TIME = 0.2f;
		private const float TIME_FOR_FREE_SLICING = 2.5f;
		private const float TIME_BEFORE_SLICE_ENDING = 1f;
		private const float CELL_COLLIDER_WIDTH = 150f;
		private const float CELL_COLLIDER_HEIGHT = 190f;
		private const float CELL_BACKGROUND_HEIGHT = 165f;
		private const float OPEN_CARD_SCALE = 1.4f;
		private const float EXPLODE_FORCE = 5f;
		private const float EXPLODE_RADIUS = 100f;
		private const float CARD_Y_OFFSET = -35f;
		private const int BASE_CARD_DEPTH = 10;
		private const float CARD_MOVE_DURATION = 0.3f;
		private const float CARD_MOVE_DELAY = 0.2f;
		private const float CARD_MOVE_TIME_OFFSET = 0.1f;

		private readonly Vector3 _openedCardPosition = new Vector3(15f, 0f, 0f);
		private readonly Vector3 _selectedBoosterpackPosition = new Vector3(0f, 0f, 0f);
		private readonly Vector3 _firstCardPosition = new Vector3(-410f, -35f, 0f);
		private readonly Vector3 _stackCardPosition = new Vector3(115f, -100f, 400f);
		private readonly Vector3 _slicingStartPoint = new Vector3(-0.5f, -0.3f, 0f);
		private readonly Vector3 _slicingEndPoint = new Vector3(0.5f, 0.3f, 0f);

		[Export]
		private float _cutForceMultiplier;
		[Export]
		private float _slowingFactor;
		[Export]
		private float _swipeLengthMultiplier;
		[Export]
		private Node _slashPrf;
		[Export]
		private UIScrollViewCustom _boosterpacksScroll;
		[Export]
		private BoosterpackScrollItem _boosterpackScrollItem;
		[Export]
		private Boosterpack _boosterpack;
		[Export]
		private Node3D _selectedBoosterpackAnchor;
		[Export]
		private Node3D _scrollAnchor;
		[Export]
		private TextureRect _scrollBG;
		[Export]
		private SpriteSlicerSFManager _spriteSlicer;
		[Export]
		private CardItem _reelItem;
		[Export]
		private Node3D _reelItems;
		[Export]
		private RewardInfo _rewardInfo;
		[Export]
		private Button _nextCardButton;
		[Export]
		private Control _unblockableInputPanel;
		[Export]
		private Node _reelItemPlaceholder;
		[Export]
		private Node _commonStackFx;
		[Export]
		private Node _rareStackFx;
		[Export]
		private Node _epicStackFx;
		[Export]
		private Node _legendaryStackFx;

		private Control _scrollerPanel;
		private CardItem _selectedCard;
		private BoosterpackItemProvider _selectedBoosterItem;
		private Boosterpack _selectedBoosterpack;
		private Booster _openedBooster;
		private CardItem _underlayedCard;
		private bool _isSpawnPlaying;
		private bool _scrollDragEnabled = true;
		private bool _firstSliceDone;
		private static bool _cardsCanBeOpen;
		private BoosterpackScrollItem _nextBoosterpack;
		private readonly List<CardItem> _cards = new List<CardItem>();
		private readonly List<BoosterpackScrollItem> _scrollItems = new List<BoosterpackScrollItem>();
		private BoosterIntentModule _intent;
		private Action _callbackOnOpen;
		private Action _callbackOnClosed;
		private List<BoosterpackItemProvider> _boosterItems;
		private EquipmentSaver _equipedItem = new EquipmentSaver();

		public static BoosterpacksManager Instance { get; private set; }

		public override void ShowModule(IntentModule intent, Action callbackOnOpen)
		{
			_intent = (BoosterIntentModule)intent;
			_callbackOnOpen = callbackOnOpen;
			if (_intent.IsInstant())
			{
				BattleCamera.Instance.changeClipPlane = true;
				BattleCamera.Instance.toClipPlane = 10f;
			}
			else
			{
				BattleCamera.Instance.MainCamera.Near = 10f;
			}
			HolderModule.EnableControls(false);
			BattleCamera.Move(BattleCamera.Instance.boosterpackCameraPosition, null, _intent.IsInstant(), true);
			LocationColorAnimation.Instance.Animate(GameSettings.DojoSettings.LocationColorChangeTime, GameSettings.DojoSettings.DefaultLocationColor, GameSettings.DojoSettings.LocationColorInModule);
			LocationColorAnimation.Instance.EnableRays(false, GameSettings.DojoSettings.LocationColorChangeTime);
			ModelsManager.Instance.HideModels(1f);
			Visible = true;
			InitBoosterpacks();
			SelectNextBoosterpack(_selectedBoosterpackPosition);
			InitScroller();
			_scrollerPanel.Modulate = new Color(1, 1, 1, 0);
			FadePanel(_scrollerPanel, 1f, 0.5f);
			FadePanel(_scrollBG, 1f, 0.5f);
			UpdateModule(intent);
			_nextCardButton.Visible = false;
		}

		public override void UpdateModule(IntentModule intent)
		{
			_intent = (BoosterIntentModule)intent;
			_callbackOnOpen.InvokeSafe();
		}

		public override void HideModule(Action callbackOnClosed)
		{
			_callbackOnClosed = callbackOnClosed;
			HolderModule.EnableControls(true);
			LocationColorAnimation.Instance.Animate(GameSettings.DojoSettings.LocationColorChangeTime, GameSettings.DojoSettings.LocationColorInModule, GameSettings.DojoSettings.DefaultLocationColor);
			LocationColorAnimation.Instance.EnableRays(true, GameSettings.DojoSettings.LocationColorChangeTime);
			ClearBoosterpacks();
			ClearCards();
			ResetFlags();
			Visible = false;
			_callbackOnClosed.InvokeSafe();
		}

		public override void Initialize()
		{
			Instance = this;
			SubtypeButton.Reset();
			Visible = false;
			InitSelectedBoosterpack();
			_rewardInfo.Init();
			_scrollerPanel = _boosterpacksScroll.GetNode<Control>(".");
			InitSpriteSlicer();
			_nextCardButton.Pressed += OpenCard;
		}

		private void InitSelectedBoosterpack()
		{
			_selectedBoosterpack = _boosterpack.Duplicate() as Boosterpack;
			_selectedBoosterpackNode();
		}

		private void _selectedBoosterpackNode()
		{
			_selectedBoosterpack.GetParent()?.RemoveChild(_selectedBoosterpack);
			_selectedBoosterpackAnchor.AddChild(_selectedBoosterpack);
			_selectedBoosterpack.Scale = Vector3.One;
			_selectedBoosterpack.Position = _selectedBoosterpackPosition;
			_selectedBoosterpack.Visible = false;
			_spriteSlicer.slashParent = _selectedBoosterpack;
		}

		private void InitSpriteSlicer()
		{
			_spriteSlicer.cutForceMultiplier = _cutForceMultiplier;
			_spriteSlicer.slowingFactor = _slowingFactor;
			_spriteSlicer.swipeLengthMultiplier = _swipeLengthMultiplier;
			_spriteSlicer.slashPrf = _slashPrf;
			_spriteSlicer.slashParent = _selectedBoosterpack;
			_spriteSlicer.onSliced = OnBoosterpackSliced;
		}

		private void InitBoosterpacks()
		{
			Dictionary<int, List<Booster>> boostersAll = UserManager.UserModelInfo.GetBoostersAll();
			foreach (KeyValuePair<int, List<Booster>> item in boostersAll)
			{
				InitBoosterpack(item.Key, item.Value.Count);
			}
		}

		private void InitBoosterpack(int modelID, int count)
		{
			if (count > 0)
			{
				BoosterpackScrollItem boosterpack = _boosterpackScrollItem.Duplicate() as BoosterpackScrollItem;
				Action onClick = delegate { OnBoosterpackSelected(boosterpack); };
				boosterpack.Init(modelID, count, onClick);
				boosterpack.GetParent()?.RemoveChild(boosterpack);
				_boosterpacksScroll.AddChild(boosterpack);
				boosterpack.Position = Vector3.Zero;
				boosterpack.Scale = Vector3.One;
				_scrollItems.Add(boosterpack);
			}
		}

		private void InitScroller()
		{
			_boosterpacksScroll.cellSize = new Vector2(150f, 190f);
			RefreshScroller();
		}

		private void RefreshScroller()
		{
			Node3D transform = _boosterpacksScroll;
			int num = 0;
			for (int i = 0; i < transform.GetChildCount(); i++)
			{
				Node child = transform.GetChild(i);
				if (child.Visible)
				{
					num++;
					((Node3D)child).Position = new Vector3(0f, 190f - (float)num * 190f, 0f);
				}
			}
			_boosterpacksScroll.RecalculateBounds();
			bool flag = num > 3;
			if (_scrollDragEnabled != flag)
			{
				_scrollDragEnabled = flag;
				if (!_scrollDragEnabled)
				{
					_boosterpacksScroll.ResetPosition();
				}
				_boosterpacksScroll.Enabled = _scrollDragEnabled;
			}
		}

		private void MoveScrollToMakeVisible(BoosterpackScrollItem scrollItem)
		{
			switch (_boosterpacksScroll.GetVisibilityForCell(scrollItem, new Vector2(150f, 165f)))
			{
			case UIScrollViewCustom.CellVisibility.TOP_PART:
				_boosterpacksScroll.ScrollDownByCell();
				break;
			case UIScrollViewCustom.CellVisibility.BOTTOM_PART:
				_boosterpacksScroll.ScrollUpByCell();
				break;
			}
		}

		private void EnableSlicer(bool enabled)
		{
			if (!enabled)
			{
				_spriteSlicer.ClearPositions();
			}
			_spriteSlicer.Visible = enabled;
		}

		private void SelectNextBoosterpack(Vector3 moveToPosition)
		{
			if (_scrollItems.Count != 0)
			{
				if (_nextBoosterpack != null)
				{
					SpawnBoosterpack(_nextBoosterpack, moveToPosition);
				}
				else
				{
					SpawnBoosterpack(_scrollItems[0], moveToPosition);
				}
			}
		}

		private void OnBoosterpackSelected(BoosterpackScrollItem scrollItem)
		{
			if (_isSpawnPlaying)
			{
				return;
			}
			MoveScrollToMakeVisible(scrollItem);
			if (!(_selectedBoosterpack.ScrollItem == scrollItem))
			{
				_isSpawnPlaying = true;
				Tween tween = CreateTween();
				tween.TweenProperty(_selectedBoosterpack.SlicableTexture, "modulate:a", 0f, 0.5f);
				tween.TweenCallback(Callable.From(() =>
				{
					SpawnBoosterpack(scrollItem, _selectedBoosterpackPosition);
					RefreshScroller();
				}));
				tween.TweenProperty(_selectedBoosterpack.SlicableTexture, "modulate:a", 1f, 0.5f);
				tween.Finished += () => _isSpawnPlaying = false;
			}
		}

		private void SpawnBoosterpack(BoosterpackScrollItem scrollItem, Vector3 moveToPosition)
		{
			_selectedBoosterpack.Init(scrollItem, SimulateSlicing);
			_selectedBoosterpack.GetParent()?.RemoveChild(_selectedBoosterpack);
			_selectedBoosterpackAnchor.AddChild(_selectedBoosterpack);
			_selectedBoosterpack.Scale = Vector3.One;
			_selectedBoosterpack.Visible = true;
			_selectedBoosterpack.Position = moveToPosition;
		}

		private void SimulateSlicing()
		{
			if (!_firstSliceDone && !_isSpawnPlaying)
			{
				_spriteSlicer.SimulateSlicing(_slicingStartPoint, _slicingEndPoint);
			}
		}

		private void OnBoosterpackSliced()
		{
			if (!_firstSliceDone)
			{
				_firstSliceDone = true;
				EnableScreenBlock(true);
				_nextCardButton.Visible = true;
				FadePanel(_scrollerPanel, 0f, 0.5f);
				FadePanel(_scrollBG, 0f, 0.5f);
				OnBoosterpackUsed();
				_spriteSlicer.EnableTrailParticles(false);
				Tween tween = CreateTween();
				tween.TweenInterval(2.5f);
				tween.TweenCallback(Callable.From(() =>
				{
					_spriteSlicer.Explode(_selectedBoosterpackAnchor.Position, 100f, 5f);
					_selectedBoosterpack.Explode();
					EnableSlicer(false);
				}));
				tween.TweenProperty(_spriteSlicer.SharedMaterial, "modulate:a", 0f, 1f);
				tween.Finished += () =>
				{
					RecreateSelectedBoosterpack();
					CreateCards();
				};
			}
		}

		private void OnBoosterpackUsed()
		{
			_openedBooster = UserManager.UserModelInfo.GetBoosterToSpend(_selectedBoosterpack.ModelID);
			_equipedItem.SaveEquipedItems();
			_boosterItems = BoosterpackItemProvider.CreateFromList(_openedBooster.AllItems);
			UserManager.GiveRewards(_openedBooster);
			SpendBoosterpack();
		}

		private void SpendBoosterpack()
		{
			if (_selectedBoosterpack.ScrollItem.WillBeDeletedAfterSpend())
			{
				_scrollItems.Remove(_selectedBoosterpack.ScrollItem);
				_nextBoosterpack = null;
			}
			else
			{
				_nextBoosterpack = _selectedBoosterpack.ScrollItem;
			}
			_selectedBoosterpack.ScrollItem.SpendBoosterpack();
			UserManager.UserModelInfo.SpendBooster(_openedBooster);
			UserManager.RemoveBoosterFromYAML(_openedBooster);
			UserBadgesManager.Instance.Reset(UserBadgesManager.BadgeTypes.Boosters, _openedBooster);
			OpenBoosterRequest openBoosterRequest = new OpenBoosterRequest();
			openBoosterRequest.InstanceId = _openedBooster.instance_id;
			OpenBoosterRequest request = openBoosterRequest;
			UserDataController.Send_OpenBooster(request);
		}

		private void RecreateSelectedBoosterpack()
		{
			_selectedBoosterpack.Visible = false;
			_selectedBoosterpack.QueueFree();
			_selectedBoosterpack = _boosterpack.Duplicate() as Boosterpack;
			_selectedBoosterpackNode();
		}

		private void OnOpenCardsSequenceEnded()
		{
			_firstSliceDone = false;
			_isSpawnPlaying = true;
			_cardsCanBeOpen = false;
			EnableSlicer(true);
			Tween tween = CreateTween();
			tween.TweenCallback(Callable.From(() =>
			{
				SelectNextBoosterpack(_selectedBoosterpackPosition);
				RefreshScroller();
				FadePanel(_scrollerPanel, 1f, 0.5f);
				FadePanel(_scrollBG, 1f, 0.5f);
			}));
			Node3D target = _selectedBoosterpack;
			Vector3 selectedBoosterpackPosition = _selectedBoosterpackPosition;
			tween.TweenProperty(target, "position:x", selectedBoosterpackPosition.X, 0.5f);
			tween.Finished += () =>
			{
				_isSpawnPlaying = false;
				EnableScreenBlock(false);
				_nextCardButton.Visible = false;
			};
		}

		private void CreateCards()
		{
			_cards.Clear();
			_cardsCanBeOpen = false;
			List<BaseItem> allItems = _openedBooster.AllItems;
			List<CardItem> list = new List<CardItem>();
			for (int i = 0; i < allItems.Count; i++)
			{
				CardItem cardItem = CreateCard(allItems[i], _openedCardPosition);
				cardItem.UpdateDepth(i + 10);
				list.Add(cardItem);
			}
			Tween masterTween = CreateTween();
			for (int j = 0; j < list.Count; j++)
			{
				Vector3 firstCardPosition = _firstCardPosition;
				float x = firstCardPosition.X + -35f * (float)(list.Count - j);
				float y = _firstCardPosition.Y;
				Vector3 endValue = new Vector3(x, y, _firstCardPosition.Z);
				float interval = 0.2f + 0.1f * (float)j;
				CardItem cardItem2 = list[j];
				Tween t = CreateTween();
				t.TweenInterval(interval);
				t.TweenProperty(cardItem2, "position", endValue, 0.7f);
				t.TweenProperty(cardItem2, "scale", Vector3.One, 0.7f);
				masterTween.TweenCallback(Callable.From(() => t.Play()));
			}
			masterTween.Finished += () => { _cardsCanBeOpen = true; };
		}

		private CardItem CreateCard(BaseItem item, Vector3 position)
		{
			CardItem cardItem = _reelItem.Duplicate() as CardItem;
			cardItem.GetParent()?.RemoveChild(cardItem);
			_reelItems.AddChild(cardItem);
			cardItem.Scale = new Vector3(1.4f, 1.4f, 1.4f);
			cardItem.Position = position;
			cardItem.Rotation = Quaternion.Identity;
			cardItem.Visible = false;
			cardItem.Init(item);
			cardItem.ActivateButton(false);
			cardItem.ActivateCollider(false);
			cardItem.SetBackClothDepth(100);
			cardItem.ShowBackCloth();
			cardItem.Visible = true;
			_cards.Add(cardItem);
			Tween tween = CreateTween();
			tween.TweenCallback(Callable.From(() => { cardItem.Modulate = new Color(1, 1, 1, 0); }));
			tween.TweenProperty(cardItem, "modulate:a", 1f, 0.5f);
			return cardItem;
		}

		private GpuParticles2D CreateStackParticle(CardItem item)
		{
			IRarable rarable = item.Item as IRarable;
			if (rarable == null)
			{
				GD.PrintErr(string.Format("item {0} is not rarable", item.Name));
				return null;
			}
			Node fxPrefab = null;
			switch (rarable.GetRarityType())
			{
			case Rarity.Common: fxPrefab = _commonStackFx; break;
			case Rarity.Rare: fxPrefab = _rareStackFx; break;
			case Rarity.Epic: fxPrefab = _epicStackFx; break;
			case Rarity.Legendary: fxPrefab = _legendaryStackFx; break;
			}
			if (fxPrefab == null)
			{
				GD.PrintErr(string.Format("item {0} has no fx", item.Name));
				return null;
			}
			Node fx = fxPrefab.Duplicate();
			item.AddChild(fx);
			((Node3D)fx).Rotation = new Vector3(0f, Mathf.DegToRad(180f), 0f);
			((Node3D)fx).Scale = new Vector3(53f, 53f, 53f);
			return fx.GetNode<GpuParticles2D>(".");
		}

		private void OpenCard()
		{
			if (!_cardsCanBeOpen)
			{
				return;
			}
			_rewardInfo.HideAll();
			if (_selectedCard != null)
			{
				_selectedCard.Visible = false;
				_selectedCard.QueueFree();
				_selectedCard = null;
			}
			if (_cards.Count == 0)
			{
				OnOpenCardsSequenceEnded();
				return;
			}
			_cardsCanBeOpen = false;
			_selectedCard = _cards[_cards.Count - 1];
			_selectedBoosterItem = _boosterItems[_cards.Count - 1];
			_cards.Remove(_selectedCard);
			_rewardInfo.Position = _openedCardPosition;
			_selectedCard.Visible = true;
			Tween tween = CreateTween();
			if (_selectedBoosterItem.ownedItem != null)
			{
				AppendSelectedCardAnimation(tween);
				JoinUnderlayedCardAnimation(tween);
				AppendStackAnimation(tween);
			}
			else
			{
				AppendSelectedCardAnimation(tween);
			}
			tween.TweenCallback(Callable.From(() =>
			{
				if (_selectedBoosterItem.ownedItem == null)
				{
					ShowSelectedCardInfo();
				}
				_cardsCanBeOpen = true;
			}));
		}

		private void AppendSelectedCardAnimation(Tween parent)
		{
			parent.TweenProperty(_selectedCard, "rotation", Vector3.Zero, 0.6f);
			parent.TweenProperty(_selectedCard, "position:x", _openedCardPosition.X, 0.6f);
			parent.TweenProperty(_selectedCard, "position:y", _openedCardPosition.Y, 0.6f);
			parent.TweenProperty(_selectedCard, "scale", new Vector3(1.4f, 1.4f, 1.4f), 0.3f);
		}

		private void JoinUnderlayedCardAnimation(Tween parent)
		{
			_underlayedCard = DuplicateCard(_selectedCard);
			parent.TweenProperty(_underlayedCard, "rotation", Vector3.Zero, 0.5f);
			parent.TweenProperty(_underlayedCard, "position", _openedCardPosition, 0.5f);
			parent.TweenProperty(_underlayedCard, "modulate:a", 0f, 0.5f);
		}

		private void AppendStackAnimation(Tween parent)
		{
			GpuParticles2D stackParticles = CreateStackParticle(_selectedCard);
			_rewardInfo.SetProgress(_selectedBoosterItem.ownedItem);
			parent.TweenCallback(Callable.From(() =>
			{
				_underlayedCard.QueueFree();
				_rewardInfo.ShowAll(_selectedBoosterItem.ownedItem, _equipedItem.GetEquipedAnalogByType(_selectedBoosterItem.ownedItem));
				stackParticles.Emitting = true;
			}));
			parent.TweenInterval(1f);
			parent.TweenCallback(Callable.From(() =>
			{
				_rewardInfo.ShowAttributes(_selectedBoosterItem.itemStacked, _equipedItem.GetEquipedAnalogByType(_selectedBoosterItem.ownedItem));
				_rewardInfo.ShowDescription(_selectedBoosterItem.itemStacked);
			}));
			parent.TweenCallback(Callable.From(() => { stackParticles.QueueFree(); }));
		}

		private void ShowSelectedCardInfo()
		{
			if (_selectedCard.IsPerk)
			{
				_rewardInfo.ShowAll(_selectedCard.Item);
				return;
			}
			Equipment equipped = UserManager.UserModelInfo.GetEquipped((_selectedCard.Item as Equipment).GetEquipmentType());
			_rewardInfo.ShowAll(_selectedCard.Item, equipped);
		}

		private CardItem DuplicateCard(CardItem card)
		{
			CardItem cardItem = card.Duplicate() as CardItem;
			cardItem.GetParent()?.RemoveChild(cardItem);
			card.GetParent().AddChild(cardItem);
			cardItem.Position = _stackCardPosition;
			cardItem.Scale = Vector3.One * 1.4f;
			cardItem.UpdateDepth(1);
			cardItem.FlipHorizontally();
			cardItem.Rotation = CardAnimator.RotationStackedItem;
			return cardItem;
		}

		private void ClearBoosterpacks()
		{
			List<Node> list = new List<Node>();
			foreach (Node child in _boosterpacksScroll.GetChildren())
			{
				list.Add(child);
			}
			foreach (Node child in list)
			{
				child.QueueFree();
			}
			_scrollItems.Clear();
		}

		private void ClearCards()
		{
			foreach (CardItem card in _cards)
			{
				card.QueueFree();
			}
			if (_selectedCard != null)
			{
				_selectedCard.QueueFree();
			}
			_cards.Clear();
		}

		private void ResetFlags()
		{
			_isSpawnPlaying = false;
			_scrollDragEnabled = true;
			_firstSliceDone = false;
			_cardsCanBeOpen = false;
		}

		private void EnableScreenBlock(bool enabled)
		{
			if (enabled)
			{
				UIBlocker.Instance.Block();
				UIBlocker.Instance.AddIgnoreObject(_unblockableInputPanel);
			}
			else
			{
				UIBlocker.Instance.Unblock();
			}
		}

		private void FadePanel(CanvasItem panel, float alpha, float duration)
		{
			Tween t = CreateTween();
			t.TweenProperty(panel, "modulate:a", alpha, duration);
		}
	}
}
