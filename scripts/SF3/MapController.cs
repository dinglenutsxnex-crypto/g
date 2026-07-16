using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nekki.UI;
using SF3.Audio;
using SF3.Moves;
using SF3.UserData;
using Godot;
using sf3DTO;

namespace SF3
{
	public class MapController : UIModuleHolder
	{
		public delegate void OnMapUpdateEventHandler();

		[Export]
		private string _onOpenSoundName = string.Empty;
		[Export]
		private string _onCloseSoundName = string.Empty;
		[Export]
		private Node3D _mapTransform;
		[Export]
		private MapBattleUI _mapBattleUI;
		[Export]
		private InfoBattleUI _infoBattleUI;
		[Export]
		private float _centeringVerticalOffset = 20f;
		[Export]
		private Button _mapBtn;
		[Export]
		private Vector3 _mapButtonStackOffsetCollapsed = new Vector2(5f, 0f);
		[Export]
		private Vector3 _mapButtonStackOffsetExpanded = new Vector2(80f, 0f);

		public Curve MapButtonStackCurve;
		public float MapButtonStackTransitionTime = 0.5f;
		public Curve CenteringCurve;
		public float CenteringSpeed;

		// TODO: port UIPanel, NekkiUIDragObject, NekkiUIDragScaleWidget to Godot equivalents
		private Vector2 _mapSize;
		private float _centeringTimer;
		private float _centeringTime;
		private Vector3 _startCenteringPoint;
		private Vector3 _centeringPoint;
		private Action _onFinishCentering;
		private IBattleInfo _selectedBattle;

		public static MapController Instance { get; private set; }

		public static IBattleInfo SelectedBattle
		{
			get
			{
				return Instance._selectedBattle;
			}
		}

		public static IBattleInfo SelectedBattleUI
		{
			get
			{
				return Instance._mapBattleUI.selectedBattle;
			}
		}

		public Vector3 MapButtonStackOffsetCollapsed
		{
			get
			{
				return _mapButtonStackOffsetCollapsed;
			}
		}

		public Vector3 MapButtonStackOffsetExpanded
		{
			get
			{
				return _mapButtonStackOffsetExpanded;
			}
		}

		public bool CenteringActive { get; private set; }
		public bool IsStackAnimatingProcess { get; set; }

		public static event OnMapUpdateEventHandler OnMapUpdate;

		public override void _Ready()
		{
			base._Ready();
			Instance = this;
		}

		public override void _Process(double delta)
		{
			if (CenteringActive)
			{
				_centeringTimer += GameTimeController.unscaledDeltaTime;
				if (_centeringTime > 0f)
				{
					SetMapPosition(Vector3.Lerp(_startCenteringPoint, _centeringPoint, CenteringCurve.Sample(_centeringTimer / _centeringTime)));
				}
				if (_centeringTimer >= _centeringTime)
				{
					OnEndMove();
				}
			}
			UpdateDragBounds();
		}

		public void UpdateDragBounds()
		{
			// TODO: port _drag.ConstrainToBounds()
		}

		public override void _ExitTree()
		{
			BattlesManager.OnBattlesUpdate -= InitMap;
			base._ExitTree();
		}

		public void SelectBattleByClick(int battleId)
		{
			if (!_mapBattleUI.IsSelectBattle(battleId))
			{
				StartMapp(battleId);
			}
		}

		public void MissClick()
		{
			if (_selectedBattle != null)
			{
				_mapBattleUI.UnSelectBattle();
				_infoBattleUI.ShowBattleInfo(false);
				QuestController.Instance.ThrowEvent(ETriggerEvents.QEVENT_MAP_MISS_CLICK, _selectedBattle);
			}
		}

		public override void Initialize()
		{
			// TODO: port UIPanel alpha/SetRect, UIWidget localSize, NekkiUIDragObject init
			_infoBattleUI.Initialize();
			_mapBattleUI.Initialize();
			MapBattleStack.Initialize(_mapBattleUI);
			AudioManager.Instance.LoadSound(_onOpenSoundName, _onCloseSoundName);
			BattlesManager.OnBattlesUpdate += InitMap;
		}

		private IBattleInfo GetSelectedBattle()
		{
			int selectedBattleID = UserManager.GetSelectedBattleID();
			return _mapBattleUI.GetBattleMapp(selectedBattleID);
		}

		private Vector3 CalculateCameraPosition(Vector3 battleMapPosition)
		{
			// TODO: port widget scale logic
			Vector3 result = default(Vector3);
			result.x = (0f - _mapSize.x + battleMapPosition.x);
			result.y = (_mapSize.y - battleMapPosition.y);
			result.z = 0f;
			return result;
		}

		public void GoToCamera(IBattleInfo battle, bool instantFocus = false, Action onFinishCamera = null)
		{
			if (battle != null)
			{
				GoToCamera(battle.GetLocation().Position, instantFocus, onFinishCamera);
			}
		}

		public void GoToCamera(Vector3 battleMapPosition, bool instantFocus = false, Action onFinishCamera = null)
		{
			_onFinishCentering = (Action)Delegate.Combine(_onFinishCentering, onFinishCamera);
			Vector3 vector = CalculateCameraPosition(battleMapPosition);
			if (instantFocus || _mapTransform.Position == vector)
			{
				SetMapPosition(vector);
				OnEndMove();
				return;
			}
			_startCenteringPoint = _mapTransform.Position;
			_centeringPoint = vector;
			_centeringTime = _startCenteringPoint.DistanceTo(_centeringPoint) / CenteringSpeed;
			CenteringActive = true;
		}

		private void SetMapPosition(Vector3 position)
		{
			_mapTransform.Position = position;
		}

		private void ResetMove()
		{
			CenteringActive = false;
			_centeringTimer = 0f;
			_centeringTime = 0f;
			_onFinishCentering = delegate
			{
			};
		}

		private void OnEndMove()
		{
			_onFinishCentering.InvokeSafe();
			ResetMove();
			QuestController.Instance.ThrowEvent(ETriggerEvents.QEVENT_BATTLE_SELECTION_END, _selectedBattle);
		}

		public override void HideModule(Action callbackOnClosed)
		{
			ModelsManager.Instance.ShowModels();
			// TODO: port DOTween fade to Godot Tween
			base.Visible = false;
			AudioManager.Instance.PlaySound(_onCloseSoundName);
			callbackOnClosed.InvokeSafe();
		}

		public override void ShowModule(IntentModule intent, Action callbackOnOpen)
		{
			callbackOnOpen.InvokeSafe();
			InitMap();
			_ = InitMapCoroutine(_selectedBattle);
		}

		private async Task InitMapCoroutine(IBattleInfo battle)
		{
			SceneTree tree = (SceneTree)Engine.GetMainLoop();
			int waitFrames = 11;
			for (int i = 0; i < waitFrames; i++)
			{
				await tree.ToSignal(tree, "process_frame");
			}
			while (DialogsManager.IsDialog)
			{
				await tree.ToSignal(tree, "process_frame");
			}
			await tree.ToSignal(tree, "process_frame");
			QuestController.Instance.ThrowEvent(ETriggerEvents.QEVENT_MAP_OPENED, battle);
		}

		private void InitMap()
		{
			UpdateMapBattles();
			IBattleInfo selectedBattle = GetSelectedBattle();
			IBattleInfo battleInfo = selectedBattle;
			if (BattlesManager.LastCompleteBattle != null)
			{
				battleInfo = BattlesManager.LastCompleteBattle;
				BattlesManager.LastCompleteBattle = null;
			}
			Vector3 mapPosition = CalculateCameraPosition(battleInfo.GetLocation().Position);
			SetMapPosition(mapPosition);
			SelectBattle(selectedBattle, false);
			if (MapController.OnMapUpdate != null)
			{
				MapController.OnMapUpdate();
			}
		}

		public void StartMapp(int battleID, bool instantFocus = false)
		{
			StartMapp(BattlesManager.instance.GetBattle(battleID), instantFocus);
		}

		public void StartMapp(IBattleInfo battle, bool instantFocus = false)
		{
			SelectBattle(battle);
			GoToCamera(battle, instantFocus);
		}

		public void SelectBattle(IBattleInfo battle, bool isThrowEvent = true)
		{
			_selectedBattle = battle;
			_infoBattleUI.LoadBattleInfo(battle);
			_mapBattleUI.SelectBattle(battle);
			UserManager.SetSelectedBattle(battle.GetID());
			if (isThrowEvent)
			{
				QuestController.Instance.ThrowEvent(ETriggerEvents.QEVENT_BATTLE_SELECTION_START, battle);
			}
		}

		public void ShowBattleInfo(bool show)
		{
			_infoBattleUI.ShowBattleInfo(show);
		}

		private void ClearBattles()
		{
			ShowBattleInfo(false);
			// TODO: port dragScaleWidget unscalable widget removal
			_mapBattleUI.ClearBattles();
			MapBattleStack.Instance.Clear();
		}

		public void UpdateMapBattles()
		{
			ClearBattles();
			List<IBattleInfo> list = new List<IBattleInfo>();
			List<IBattleInfo> battlesVisible = BattlesManager.instance.GetBattlesVisible();
			List<IBattleInfo> battlesFutureAvailable = BattlesManager.instance.GetBattlesFutureAvailable();
			List<IBattleInfo> list2 = BattlesManager.instance.GetBattles().FindAll((IBattleInfo info) => info.GetBattleType() == sf3DTO.BattleType.Daily);
			foreach (IBattleInfo item in list2)
			{
				if (((DailyBattleInfo)item).DailyUpdateTimePassed)
				{
					RefreshBattlesProcessing.RefreshBattles();
					break;
				}
			}
			list.AddRange(battlesVisible);
			list.AddRange(battlesFutureAvailable);
			foreach (IBattleInfo item2 in battlesVisible)
			{
				AddBattle(item2, (!item2.GetIsAvailable()) ? MapBattleButton.DecorationType.Unavailable : MapBattleButton.DecorationType.Available);
			}
			foreach (IBattleInfo item3 in battlesFutureAvailable)
			{
				AddBattle(item3, MapBattleButton.DecorationType.Available);
			}
			foreach (IBattleInfo item4 in list2)
			{
				if (!list.Contains(item4))
				{
					list.Add(item4);
					AddBattle(item4, MapBattleButton.DecorationType.Unavailable);
				}
			}
		}

		public void AddBattle(IBattleInfo newBattle, MapBattleButton.DecorationType decorationType)
		{
			MapBattleButton mapBattleButton = _mapBattleUI.CreateMapBattle(newBattle, decorationType);
			MapBattleStack.Instance.Add(mapBattleButton, newBattle);
		}
	}
}
