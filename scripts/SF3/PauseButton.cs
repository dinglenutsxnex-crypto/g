using Godot;
using System;

namespace SF3
{
	public partial class PauseButton : UIModuleHolder
	{
		public void OnPauseButtonClicked()
		{
			PauseWindow.Pause();
		}
	}
}
