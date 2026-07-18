using Godot;
using System;
using SF3.Audio;

namespace SF3.DebugGame
{
	public partial class SetGameTime : UIModuleHolder
	{
		public static float currentGameTime;

		public Label valueLbl;
		public ScrollBar scrollBar;

		private bool visible = true;
		public Node objToDisable;

		public void GameTimeChanged(float value)
		{
			if (GameTimeController.gamePaused)
			{
				currentGameTime = GameTimeController.timeScale;
				scrollBar.Value = currentGameTime;
			}
			else
			{
				currentGameTime = value;
				GameTimeController.ChangeGameTime(value);
				AudioManager.Instance.SetPitch(value);
			}
			valueLbl.Text = Mathf.RoundToInt(currentGameTime * 100f) + string.Empty;
		}

		public void ShowHide()
		{
			visible = !visible;
			if (visible)
			{
				scrollBar.Value = GameTimeController.timeScale;
				valueLbl.Text = Mathf.RoundToInt((float)scrollBar.Value * 100f) + string.Empty;
			}
			objToDisable.Visible = visible;
		}
	}
}
