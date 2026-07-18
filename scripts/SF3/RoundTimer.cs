using Godot;

namespace SF3
{
	public partial class RoundTimer : UIModuleHolder
	{
		[Export]
		private Label _timerLabel;

		public void UpdateLabel(string time)
		{
			_timerLabel.Text = time;
		}
	}
}
