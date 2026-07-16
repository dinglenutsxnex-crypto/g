using Godot;

namespace SF3
{
	public class RoundTimer : UIModuleHolder
	{
		[Export]
		private Label _timerLabel;

		public void UpdateLabel(string time)
		{
			_timerLabel.Text = time;
		}
	}
}
