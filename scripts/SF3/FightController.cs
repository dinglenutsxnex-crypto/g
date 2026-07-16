using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SF3.GameModels;
using SF3.Items;
using SF3.Moves;
using SF3.UserData;
using Godot;
using sf3DTO;

namespace SF3
{
	public class FightController
	{
		public enum EFightStage
		{
			None = 0,
			FightStart = 1,
			FightEnd = 2,
			RoundStart = 3,
			RoundEnd = 4,
			RoundFightStart = 5,
			RoundFightEnd = 6
		}

		public RewardMultipyerCounter RewardMultipyerCounter;
		private FightResult _fightResult;
		private int _modelsAnimationEndCounter;

		public static FightControllerSettings Settings { get; private set; }
		public static FightController Instance { get; private set; }
		public RoundController RoundController { get; private set; }
		public EFightStage FightStage { get; private set; }
		public FightInfo CurrentFight { get; private set; }

		private static SceneTree _tree => (SceneTree)Engine.GetMainLoop();

		public FightController()
		{
			Instance = this;
			RoundController = new RoundController();
			RewardMultipyerCounter = new RewardMultipyerCounter();
		}

		public void Initialize()
		{
			RoundController.Initialize();
			FightStage = EFightStage.None;
			_fightResult = null;
			_modelsAnimationEndCounter = 0;
		}

		public async Task InitFight()
		{
			GD.Print("[InitFight]");
			Settings = new FightControllerSettings();
			CurrentFight = BattlesManager.currentBattle.GetCurrentFight();
			_fightResult = null;
			await SetFightStage(EFightStage.RoundStart);
		}

		public void Update()
		{
			if (FightStage != 0)
			{
				RoundController.Update();
				if (FightStage == EFightStage.RoundFightStart && RoundController.CheckEndRound() != 0)
				{
					_ = SetFightStage(EFightStage.RoundFightEnd);
				}
			}
		}

		private async Task SetFightStage(EFightStage stageValue, bool surrender = false, int? winnerId = null)
		{
			FightStage = stageValue;
			switch (stageValue)
			{
			case EFightStage.FightEnd:
				SetFightEnd(surrender, winnerId);
				break;
			case EFightStage.RoundStart:
				await RoundStartCoroutine();
				break;
			case EFightStage.RoundEnd:
				SetRoundEnd();
				break;
			case EFightStage.RoundFightStart:
				SetRoundFightStart();
				break;
			case EFightStage.RoundFightEnd:
				SetRoundFightEnd();
				break;
			}
			BattleController.ThrowEvent(new BattleEventArgs(ETriggerEvents.EVENT_STAGE_CHANGE, -1, stageValue));
			GD.Print(string.Format("SetFightStage [{0}] ", stageValue));
		}

		private void SetRoundFightEnd()
		{
			RoundController.EndRoundFight();
			BattleKeyManager.Instance.ActivateBattleKeys(false);
			BattleKeyManager.Instance.EnableBattleKeysEvents(false);
			_modelsAnimationEndCounter = 2;
		}

		private void SetRoundFightStart()
		{
			BattleKeyManager.Instance.EnableBattleKeysEvents(true);
			RoundController.StartFight();
		}

		private async Task RoundStartCoroutine()
		{
			BattleLog.RoundStart(RoundController.CurrentRoundNumber);
			await RoundStartProcess();
		}

		private void RoundFightStartProcess()
		{
			_ = SetFightStage(EFightStage.RoundFightStart);
		}

		private async Task DojoRound()
		{
			using (TimerNode timerNode2 = new TimerNode("EntireDojo", "InitBattle"))
			{
				RoundController.ClearRoundData(CurrentFight);
				using (new TimerNode("InitRound", "EntireDojo"))
				{
					RoundController.InitNewRound(CurrentFight);
				}
				RoundController.InitBattleCamera(true);
				BattleController.RegisterEventCallback(ETriggerEvents.EVENT_ANIMATION_END, OnStageAnimationsEnd);
				if (UserDataController.waitingForRefreshBattles)
				{
					BattleController.Instance.EventsEnable(false);
					BattleController.Instance.BattleEnable(false);
					while (UserDataController.waitingForRefreshBattles)
					{
						await _tree.ToSignal(_tree, "process_frame");
					}
					BattleController.Instance.BattleEnable(true);
					BattleController.Instance.EventsEnable(true);
				}
				LoadScreen.HideLoader(null);
				_ = SetFightStage(EFightStage.RoundFightStart);
			}
		}

		private async Task ShowFoggedScreenshot()
		{
			bool loadscreenActive = true;
			LoadScreen.Instance.ShowFightStart(delegate
			{
				QualityManager.Instance.SetShadowForcedOff(false);
				loadscreenActive = false;
			});
			await _tree.ToSignal(_tree, "process_frame");
			while (loadscreenActive)
			{
				await _tree.ToSignal(_tree, "process_frame");
			}
		}

		private async Task RoundControllerLoading()
		{
			using (new TimerNode("ClearData", "Loading"))
			{
				RoundController.ClearRoundData(CurrentFight);
			}
			await _tree.ToSignal(_tree, "process_frame");
			using (new TimerNode("InitRound", "Loading"))
			{
				RoundController.InitNewRound(CurrentFight);
			}
			BattleController.ThrowEvent(new BattleEventArgs(ETriggerEvents.QEVENT_ROUND_LOAD));
			await _tree.ToSignal(_tree, "process_frame");
			while (DialogsManager.IsDialog)
			{
				await _tree.ToSignal(_tree, "process_frame");
			}
		}

		private async Task ShowRoundUI()
		{
			bool loadscreenActive = true;
			RoundController.Instance.ShowStartRoundGUI(delegate
			{
				loadscreenActive = false;
				PauseWindow.IncrementShowCounter();
			});
			while (loadscreenActive)
			{
				await _tree.ToSignal(_tree, "process_frame");
			}
		}

		private async Task HideRoundUI()
		{
			bool loadscreenActive = true;
			LoadScreen.Instance.HideRoundEnd(delegate
			{
				loadscreenActive = false;
			});
			while (loadscreenActive)
			{
				await _tree.ToSignal(_tree, "process_frame");
			}
		}

		private async Task Round()
		{
			using (TimerNode timerNode4 = new TimerNode("Entire Round", "InitBattle"))
			{
				using (TimerNode timerNode = new TimerNode("Camera Movement", "Entire Round"))
				{
					if (RoundController.Instance.CurrentRoundNumber > 0)
					{
						BattleCamera.Instance.RoundEndTweenMotion();
						InteractiveModelObject.HideAll();
						ShadowFormController.Instance.ClearShadowEffect();
						GlowEffectController.instance.DisableGlow();
						while (!BattleCamera.Instance.RoundEndTweenIsReady())
						{
							await _tree.ToSignal(_tree, "process_frame");
						}
						QualityManager.Instance.SetShadowForcedOff(true);
						await _tree.ToSignal(_tree, "process_frame");
					}
					else
					{
						await _tree.ToSignal(_tree, "process_frame");
						BattleCamera.MoveToSpawnCentre(true);
					}
				}
				PauseWindow.ResetShowCounter();
				BattleCamera.Instance.SetCameraBlocked(true);
				await _tree.ToSignal(_tree, "process_frame");
				using (TimerNode timerNode2 = new TimerNode("Screenshot", "Entire Round"))
				{
					await ShowFoggedScreenshot();
				}
				using (TimerNode timerNode3 = new TimerNode("Loading", "Entire Round"))
				{
					await RoundControllerLoading();
				}
			}
			BattleController.Instance.BattleEnable(true);
			using (TimerNode timerNode5 = new TimerNode("RoundUI", "Entire Round"))
			{
				await ShowRoundUI();
				await HideRoundUI();
				BattleCamera.Instance.SetCameraBlocked(false);
				RoundController.InitBattleCamera(false);
				BattleController.RegisterEventCallback(ETriggerEvents.EVENT_ANIMATION_END, OnStageAnimationsEnd);
				_modelsAnimationEndCounter = 2;
			}
		}

		private async Task RoundStartProcess()
		{
			if (!IsDojo())
				await Round();
			else
				await DojoRound();
		}

		public bool IsDojo()
		{
			return BattlesManager.currentBattleType == BattleType.Dojo;
		}

		private void SetRoundEnd()
		{
			RoundController.UpdateRewardCounters();
			if (RoundController.PlayerWinCount == RoundController.RoundsTotal || RoundController.EnemyWinCount == CurrentFight.roundsToLose)
			{
				_ = SetFightStage(EFightStage.FightEnd);
			}
			else
			{
				_ = SetFightStage(EFightStage.RoundStart);
			}
		}

		private void SetFightEnd(bool surrender, int? winnerId = null)
		{
			GD.Print(string.Format("FightEnd -- CurrentBattle[{0}] -- CurrentFight [{1}] -- PlayerRoundWins [{2}] ", BattlesManager.currentBattle.GetBattleInfo().name, CurrentFight.battleID, RoundController.PlayerWinCount));
			int num = RoundController.PlayerWinCount;
			int num2 = RoundController.EnemyWinCount;
			if (winnerId.HasValue)
			{
				num = ((winnerId == 1) ? CurrentFight.roundsToWin : 0);
				num2 = ((winnerId != 1) ? CurrentFight.roundsToWin : 0);
			}
			_fightResult = new FightResult(BattlesManager.currentBattle, CurrentFight, num, num + num2, surrender, RewardMultipyerCounter);
			List<BaseItem> list = new List<BaseItem>();
			list.AddRange(((IEnumerable<Equipment>)_fightResult.GetRewardEquipment()).Select((Func<Equipment, BaseItem>)((Equipment equipment) => equipment)));
			list.AddRange(((IEnumerable<SF3.Items.Perk>)_fightResult.GetRewardPerks()).Select((Func<SF3.Items.Perk, BaseItem>)((SF3.Items.Perk equipment) => equipment)));
			list.AddRange(((IEnumerable<SF3.Items.Booster>)_fightResult.GetRewardBoosters()).Select((Func<SF3.Items.Booster, BaseItem>)((SF3.Items.Booster equipment) => equipment)));
			RewardDataProvider rewardDataProvider = new RewardDataProvider(list, UserManager.GetLevel(), UserManager.GetExperience());
			FightEnd(_fightResult);
			if (!surrender)
			{
				ShowDialog(_fightResult, rewardDataProvider, CloseFight);
			}
			else
			{
				CloseFight();
			}
			if (TutorialManager.Instance.tutorialPanel != null)
			{
				TutorialManager.Instance.HidePanel();
			}
		}

		public void FightEnd(FightResult fightResult)
		{
			GameTimeController.ResetTimeScale();
			BattlesManager.CompleteFight(fightResult);
			BattleLog.EndFight(fightResult.resultType == sf3DTO.FightResult.Win);
			RewardMultipyerCounter.Clear();
			QuestController.Instance.ThrowEvent(ETriggerEvents.QEVENT_FIGHT_END, fightResult.currentFight.battleID, fightResult.currentFight.fightID, fightResult.resultType);
		}

		public void ShowDialog(FightResult fightResult, RewardDataProvider rewardDataProvider, Action callback)
		{
			BattleInterface.Instance.ShowEndGame(delegate
			{
				switch (fightResult.resultType)
				{
				case sf3DTO.FightResult.Win:
				case sf3DTO.FightResult.Loss:
					RewardsWindow.Show(fightResult, rewardDataProvider, callback);
					break;
				case sf3DTO.FightResult.Surrender:
					callback();
					break;
				}
			}, fightResult);
		}

		public void CloseFight()
		{
			RoundResetableManager.Instance.ResetRules();
			LoadScreen.ShowLoader(delegate
			{
				if (QuestController.Instance.IsQueueEmpty())
				{
					ModuleController.GoToMap();
				}
				else
				{
					QuestController.Instance.RunForciblyQueue();
				}
			}, 0f, true);
		}

		private void OnStageAnimationsEnd(BattleEventArgs eventArgs)
		{
			if (eventArgs == null || eventArgs.EventData == null || _modelsAnimationEndCounter <= 0 || ((ModelInfoAnimation)eventArgs.EventData).animation == null || !((ModelInfoAnimation)eventArgs.EventData).animation.StageExist(FightStage))
			{
				return;
			}
			_modelsAnimationEndCounter--;
			if (_modelsAnimationEndCounter != 0)
			{
				return;
			}
			switch (FightStage)
			{
			case EFightStage.RoundStart:
				ShowGuiFightStart();
				break;
			case EFightStage.RoundFightEnd:
				RoundController.ShowEndRoundGUI(delegate
				{
					_ = SetFightStage(EFightStage.RoundEnd);
				});
				break;
			}
		}

		private void ShowGuiFightStart()
		{
			if (Settings.ShowFightStart)
			{
				RoundController.ShowStartFightGUI(RoundFightStartProcess);
			}
			else
			{
				RoundFightStartProcess();
			}
		}

		public void SetFightResult(int winnerId, bool surrender)
		{
			_ = SetFightStage(EFightStage.FightEnd, surrender, winnerId);
		}

		public void WinCurrentRound(ERoundResult winner)
		{
			RoundController.SetRoundWinner(winner);
			_ = SetFightStage(EFightStage.RoundFightEnd);
		}

		public static bool TacticsCanReact()
		{
			return Instance.FightStage == EFightStage.RoundFightStart;
		}
	}
}
