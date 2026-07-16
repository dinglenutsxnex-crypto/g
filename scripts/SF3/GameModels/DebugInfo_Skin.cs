using Godot;

namespace SF3.GameModels
{
	public partial class DebugInfo_Skin : Node
	{
		[Export] public bool logInfo;

		public override void _Ready()
		{
		}

		public override void _Process(double delta)
		{
			if (logInfo)
			{
				logInfo = false;
				WriteSkinInfo();
			}
		}

		private void WriteSkinInfo()
		{
			SkinnedMeshInstance3D component = GetNode<SkinnedMeshInstance3D>(".");
			GD.Print("----------  " + component.Name + "  ----------");
			GD.Print("rootBone : " + component.Skeleton);
		}
	}
}
