using Godot;
using System;

namespace SF3
{
	public class PauseButton : UIModuleHolder
	{
		public void OnPauseButtonClicked()
		{
			PauseWindow.Pause();
		}
	}
}
