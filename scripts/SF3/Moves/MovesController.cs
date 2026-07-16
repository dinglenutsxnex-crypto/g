using System.Collections.Generic;
using SF3.GameModels;
using Godot;
using SF3.Moves;
namespace SF3.Moves
{
	public partial class MovesController : Node
	{
		private static readonly SplitController SplitController = new SplitController();
		private static readonly TriggerActionController TriggerActionController = new TriggerActionController();
		public static void Init()
		{
			SplitController.Init();
		}
		public static void ApplyAction(TriggerAction action, object modelData)
		{
			TriggerActionController.ApplyAction(action, modelData);
		}
		public static void ClearActionsCallbacks()
		{
			TriggerActionController.ClearActionsCallbacks();
		}
		public static InfoAnimationPlayer GetAnimationByName(string name)
		{
			return SplitController.GetAnimationByName(name);
		}
		public static AnimationBinaries GetBinariesByName(string name)
		{
			return SplitController.GetBinariesByName(name);
		}
		public static List<InfoTriggerSet> LoadTriggers(Model model)
		{
			Condition.ClearFunctionResults();
			return SplitController.LoadTriggers(model);
		}
		public static List<InfoAnimationPlayer> GetAnimations()
		{
			return SplitController.GetAnimations();
		}
		public static void UnloadUnusedTriggers()
		{
			SplitController.UnloadUnusedTriggers();
		}
	}
}

