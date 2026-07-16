using Godot;
using System.Collections.Generic;
using SF3.Effects;
using SF3.Moves;
using SF3.Utils;

namespace SF3.GameModels
{
	public partial class InteractiveModelObject : Node, IShadowFormModel
	{
		private bool _shadowFormActive;

		private BehaviourTimer.SingleTimer _shadowFormBlendTimer;

		private float _kinematicApplyTimer;

		public static List<InteractiveModelObject> droppedInteractiveObjects { get; private set; }

		public ModelObject modelObject { get; private set; }

		static InteractiveModelObject()
		{
			droppedInteractiveObjects = new List<InteractiveModelObject>();
		}

		public void SetModelObject(ModelObject _modelObject, bool shadowFormActive)
		{
			modelObject = _modelObject;
			_shadowFormActive = shadowFormActive;
			droppedInteractiveObjects.Add(this);
			_kinematicApplyTimer = -1f;
			if (!EffectsManager.freezeFrameActive)
			{
				return;
			}
			foreach (SkeletonObject skeleton in modelObject.skeletons)
			{
				skeleton.skeletonRagdoll.SetRagdollSleepState(true, 0);
			}
		}

		public override void _Process(double delta)
		{
			if (modelObject == null || _kinematicApplyTimer == -1f || !(_kinematicApplyTimer < GameTimeController.battleTime))
			{
				return;
			}
			_kinematicApplyTimer = -1f;
			foreach (SkeletonObject skeleton in modelObject.skeletons)
			{
				skeleton.skeletonRagdoll.SetActive(false);
				skeleton.skeletonRagdoll.FreezeObject(true);
			}
		}

		public static void Reset()
		{
			if (droppedInteractiveObjects.Count <= 0)
			{
				return;
			}
			foreach (InteractiveModelObject droppedInteractiveObject in droppedInteractiveObjects)
			{
				if (!(droppedInteractiveObject == null))
				{
					droppedInteractiveObject.modelObject.DestroyItem();
					droppedInteractiveObject.QueueFree();
				}
			}
			droppedInteractiveObjects.Clear();
		}

		public void OnCollision(SkeletonObject skeletonObject, KinematicCollision3D KinematicCollision3D)
		{
			if (FloorController.Instance.IsFloor(KinematicCollision3D.gameObject))
			{
				string materialName = modelObject.modelMaterials.modelMaterials[modelObject.skeletons.IndexOf(skeletonObject)].materialName;
				BattleController.ThrowEvent(new BattleEventArgs(ETriggerEvents.EVENT_ITEM_FLOOR_HIT, 1, new StrikeData
				{
					attackingEdgeMat = materialName
				}));
				skeletonObject.OnCollision -= OnCollision;
				if (_kinematicApplyTimer == -1f)
				{
					_kinematicApplyTimer = GameTimeController.battleTime + 120f;
				}
			}
		}

		public void ActivateShadowForm(bool instant = false)
		{
			if (_shadowFormBlendTimer != null)
			{
				_shadowFormBlendTimer.Stop();
			}
			if (modelObject != null)
			{
				_shadowFormBlendTimer = BehaviourTimer.CreateGameFramesTimer(45, UpdateShadowFormBlend, null, delegate
				{
					_shadowFormBlendTimer = null;
					modelObject.ChangeToNonDissolveShadowForm();
				});
			}
		}

		public void DisableShadowForm()
		{
			if (_shadowFormBlendTimer != null)
			{
				_shadowFormBlendTimer.Stop();
			}
			_shadowFormBlendTimer = BehaviourTimer.CreateGameFramesTimer(45, delegate(float progress)
			{
				UpdateShadowFormBlend(1f - progress);
			}, null, delegate
			{
				if (!ModelsManager.Instance.Player.modelShadowForm.shadowFormActive && !ModelsManager.Instance.Enemy.modelShadowForm.shadowFormActive)
				{
					GlowEffectController.instance.DisableGlow();
				}
				modelObject.ReturnDefaultMaterial();
				_shadowFormBlendTimer = null;
			});
		}

		private void UpdateShadowFormBlend(float progress)
		{
			modelObject.UpdateShadowFormBlend(progress);
		}

		public bool GetShadowFormActive()
		{
			return _shadowFormActive;
		}

		public static void HideAll()
		{
			// STUB: needs UI rebuild with Godot Control nodes
			GD.Print("STUB: InteractiveModelObject.HideAll - coroutine based transparency animation needs porting");
		}

		public void UpdateShadowForm()
		{
		}
	}
}
